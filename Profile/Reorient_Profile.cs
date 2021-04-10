using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.DocObjects;
using Rhino.Geometry;
using IEF_Toolbox.Class;

namespace IEF_Toolbox.Profile
{
    public class DieProfilePrep : GH_Component
    {
        GH_Document GrasshopperDocument;
        IGH_Component Component;
        RhinoDoc RhinoDocument;
        /// <summary>
        /// Initializes a new instance of the DieProfilePrep class.
        /// </summary>
        public DieProfilePrep()
          : base("Prep Profile", "Prep Profile",
              "Align Framing Extrusion die profiles to the world coordinate origin",
              "IEF Toolbox", "02_Profile")
        {
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("string_Referenced Block Name", "B", "Name of the die profile", GH_ParamAccess.item);
            pManager.AddBooleanParameter("bool_Run to Bake", "R", "Set to True to bake the reoriented Curve to the Rhino Space", GH_ParamAccess.item);
            pManager.AddTextParameter("_string_Layer", "_l", "Override the default bake Layer. The default layer is 'Profile Curve'", GH_ParamAccess.item, "Profile Curve");
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Successfully Reoriented?", "s?", "True if the profile is successfully reoriented", GH_ParamAccess.item);
            pManager.AddTextParameter("string_Baked Crv Unique ID", "UID", "Unique ID of the baked profile Curve", GH_ParamAccess.item);
            pManager.AddGenericParameter("Profile Object", "P", "The profile object package, Use Deconstruct Profile Object to extract information", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Component = this;
            GrasshopperDocument = this.OnPingDocument();
            RhinoDocument = RhinoDoc.ActiveDoc;
            //INSTANTIATE
            double tolerance = 0.001;
            double overlap_tolerance = 0.0;
            double profileCurveMinimalLength = 0.3;
            bool _Bake = false;

            List<Curve> profileCrvReoriented = new List<Curve>();
            List<string> notReorientedCrv = new List<string>();
            List<string> reorientedID = new List<string>();
            FrameProfile profile = new FrameProfile();
            bool successMap = false;

            // get input value
            string BlockName = null;
            string ProfileID = null;
            string BakeLayer = null;
            bool success1 = DA.GetData<string>(0, ref BlockName);
            if (!success1) { return; }
            else { ProfileID = BlockName; }
            bool success2 = DA.GetData<string>(2, ref BakeLayer);
            if (!success2) { BakeLayer = "Profile Curve"; }

            //get the block full path
            string fullBName = "";
            for (int i = 0; i < RhinoDocument.InstanceDefinitions.Count;i++) 
            {
                var foo = RhinoDocument.InstanceDefinitions[i].Name;
                string n = foo.ToString();
              if (n.Contains(BlockName))
                {
                    fullBName = n;
                }
            }
            bool bNameNotExist = fullBName == "";
            if (bNameNotExist)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, BlockName + " not found in the instance table");
                return;
            }


            //REFERENCE OBJECTS IN THE BLOCK AND PUT TOGETHER BY NAME
            var BlockObjects = new List<GeometryBase>();
            var def = RhinoDocument.InstanceDefinitions.Find(fullBName,false);
            if (!bNameNotExist && def == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Can't reference the block: " + BlockName);
                return;
            }
            RhinoObject[] objects = def.GetObjects();
            if (objects.Length == 0)
            {
                BlockObjects.Add(null);
            }
            else
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    GeometryBase gb = objects[i].DuplicateGeometry();
                    BlockObjects.Add(gb);
                }
            }

            //GET ALL THE CURVE OBJECTS AND PROJECT ALL CURVES TO XY PLANE
            List<Curve> crvObjs = new List<Curve>();
            for (int i = 0; i < BlockObjects.Count; i++)
            {
                GeometryBase obj = BlockObjects[i];
                string objType = obj.GetType().ToString();

                if (objType.Contains("Curve"))
                {
                    Curve crv = null;
                    GH_Convert.ToCurve(BlockObjects[i], ref crv, GH_Conversion.Both);
                    crv = Curve.ProjectToPlane(crv, Plane.WorldXY);
                    crvObjs.Add(crv);
                }
            }


            //JOIN ALL FRAGMENTED CURVES/LINES
            Curve[] crvJoined = Curve.JoinCurves(crvObjs);
            //FIND THE DATUM POINT AS EITHER THE INTERSECITON OF THE TWO "LINE-LIKE CURVE"S OR THE AREA CENTER OF THE "CIRCLE-LIKE CURVE"
            List<Curve> profileCrvRaw = new List<Curve>();
            List<Curve> datumCross = new List<Curve>();
            List<Circle> datumCircles = new List<Circle>();

            for (int i = 0; i < crvJoined.Length; i++)
            {
                string crvType = crvJoined[i].GetType().ToString();
                Curve crv = crvJoined[i];
                if (crv.IsCircle())
                {
                    Circle circle;
                    crv.TryGetCircle(out circle, tolerance);
                    datumCircles.Add(circle);
                }
                else if (crv.IsLinear() || crv.IsPolyline())
                {
                    datumCross.Add(crv);
                }
                else if (crv.GetLength() >= profileCurveMinimalLength)
                {
                    profileCrvRaw.Add(crv);
                }
            }


            //FIND THE DATUM POINT AS EITHER THE INTERSECITON OF THE TWO "LINE-LIKE CURVE"S OR THE AREA CENTER OF THE "CIRCLE-LIKE CURVE"    
            List<Point3d> datumIntersectPoints = new List<Point3d>();
            Point3d defaultDatum = new Point3d(10000, 10000, 10000);
            Point3d datumPoint = new Point3d(defaultDatum);
            if (datumCross.Count >= 2)
            {
                for (int i = 0; i < datumCross.Count - 1; i++)
                {
                    var datumIntersect = Rhino.Geometry.Intersect.Intersection.CurveCurve(datumCross[i], datumCross[i + 1], tolerance, overlap_tolerance);
                    foreach (var pt in datumIntersect)
                    {
                        datumIntersectPoints.Add(pt.PointA);
                    }
                }
            }
            if (datumIntersectPoints.Count != 0) { datumPoint = datumIntersectPoints[0]; }
            else if (datumCircles.Count > 0) { datumPoint = datumCircles[0].Center; }
            else { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, BlockName + " datum invalid. Please fix datum anchor in the referenced block. "); }

            // Compare the area of the boundingbox of the regions and select the largest region with the same index i
            //// instantiate
            int regionIndex = 0;
            CurveBooleanRegions intersectRegion = Curve.CreateBooleanRegions(profileCrvRaw, Plane.WorldXY, false, tolerance);
            List<double> allRegionAreas = new List<double>();
            List<Curve> leanProfile = new List<Curve>();
            //// get boundingbox and calc the area, make area a list. Then find the index of the max area, use the index to call the region
            for (int i = 0; i < intersectRegion.RegionCount; i++)
            {
                Curve[] regions = intersectRegion.RegionCurves(i);
                List<double> Area = new List<double>();
                foreach (Curve r in regions)
                {
                    BoundingBox bbx = r.GetBoundingBox(Plane.WorldXY);
                    double a = bbx.Area;
                    Area.Add(a);
                }
                double regionArea = Area.Max();
                allRegionAreas.Add(regionArea);
            }
            if (allRegionAreas.Count > 0)
            {
                regionIndex = allRegionAreas.IndexOf(allRegionAreas.Max());
                Curve[] leanprofile = intersectRegion.RegionCurves(regionIndex);
                foreach (Curve crv in leanprofile)
                {
                    leanProfile.Add(crv);
                }
            }



            //PASSED^
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //CHECK IF THE PROFILE CURVES ARE CLOSED. IF NOT, TRY CLOSE THE CURVE, THEN GET ALL THE CLOSED CURVES INTO A LIST
            //List<Curve> profileCrvClosed = new List<Curve>();
            //for (int i = 0; i < profileCrvRaw.Count; i++)
            //{
            //    Curve crv = profileCrvRaw[i];
            //    if (crv.MakeClosed(tolerance) || crv.IsClosed)
            //    {
            //        crv.MakeClosed(tolerance);
            //        profileCrvClosed.Add(crv);
            //    }
            //}
            //SEND ERROR MESSAGE IF profileCrvRaw CONTAINS UNWANTED CURVE SEGMENTS
            //if (crv.ToString() != "Closed Planar Curve") {
            //for (int i = 0; i < profileCrvClosed.Count; i++)
            //{
            //    Curve crv = profileCrvClosed[i];
            //    if (!crv.IsClosed || !crv.IsPlanar())
            //    {
            //        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Block: " + BlockName + "'s die profile contains unwanted curve(s). " +
            //            "All profile curves should be Closed Planar Curves. Please check the profile curves or the datum anchor to fix.");
            //        break;
            //    }
            //}



            //CURVEBOOLEAN THE PLANAR CURVE TO GET THE LEAN PROFILE CURVES
            //List<Point3d> ClickPoints = new List<Point3d>();
            ////Calc all the curve area and sort the area, get the index of the greatest area and then find the outside curve
            //List<double> areas = new List<double>();
            //foreach(Curve crv in profileCrvClosed) {
            //    AreaMassProperties calc = AreaMassProperties.Compute(crv);
            //    double area = calc.Area;
            //    areas.Add(area); 
            //};
            //int iOutsideCurve = areas.IndexOf(areas.Max());
            //for (int i = 0; i < areas.Count; i++)
            //{
            //    AreaMassProperties calc = AreaMassProperties.Compute(profileCrvClosed[i]);
            //    Point3d icenter = calc.Centroid;
            //    if (i == iOutsideCurve) {
            //        Point3d outSidePoint = new Point3d(icenter.X + 100, icenter.Y + 100, icenter.Z);
            //        ClickPoints.Add(outSidePoint);
            //    }
            //    else {
            //        ClickPoints.Add(icenter); }
            //}


            //List<Curve> profileCrvBU = new List<Curve>();
            //CurveBooleanRegions profileCrvRegion = Curve.CreateBooleanRegions(profileCrvClosed, Plane.WorldXY, ClickPoints, true, tolerance);
            //for (int i = 0; i < profileCrvRegion.RegionCount; i++) {
            //    for (int j = 0; j < profileCrvRegion.RegionCurves(i).Length; j++)
            //        profileCrvBU.Add(profileCrvRegion.RegionCurves(i)[j]);
            //        }




            //RE-ORIENT THE PROFILE CURVE TO WORLD COORDINATE ORIGIN, YZ PLANE, WHICH EQUALS TO O = (0,0,0), X = (0,0,-1), Y = (0,1,0)
            Point3d fromOrigin = datumPoint;
            Vector3d fromNormal = new Vector3d(0, 0, 1);
            Point3d toOrigin = new Point3d(0, 0, 0);
            Vector3d toX = new Vector3d(0, 0, -1);
            Vector3d toY = new Vector3d(0, 1, 0);
            Plane pfrom = new Plane(fromOrigin, fromNormal);
            Plane pto = new Plane(toOrigin, toX, toY);

            Transform reorient = Transform.PlaneToPlane(pfrom, pto);

            if (fromOrigin.Equals(defaultDatum))
            {
                notReorientedCrv.Add(BlockName);
                successMap = false;
            }
            else
            {
                for (int i = 0; i < leanProfile.Count; i++)
                {
                    Curve crv = leanProfile[i];
                    crv.Transform(reorient);
                    profileCrvReoriented.Add(crv);
                    reorientedID.Add(BlockName);
                }
                successMap = true;
            }

            // mirror the profile at the center if the framing curve's Z < 0
            foreach (Curve crv in profileCrvReoriented)
            {
                Transform mirror = Transform.Mirror(Plane.WorldXY);
                if (crv.GetBoundingBox(false).FurthestPoint(Point3d.Origin).Z > 0)
                {
                    crv.Transform(mirror);
                };
            }

            // Store the information in the profile object
            profile.ProfileID = ProfileID;
            profile.ProfileCrv = profileCrvReoriented;
            profile.Anchor = toOrigin;
            profile.BasePlane = pto;
            profile.SortInsideOutside();

            //// Assign unique
            Guid uniqueID = Guid.NewGuid();
            profile.uniqueID = uniqueID.ToString();


            // Bake profile curves when the Bake is set to True
            List<Guid> ref_ids = new List<Guid>(profileCrvReoriented.Count);

            bool success3 = DA.GetData<bool>(1, ref _Bake);
            string layerActive = "Profile Curve";
            string layerDeposit = "Profile Curve_Archive";

            if (success3)
            {
                if (_Bake) {
                    int currentVersion = 0;
                    int newVersion;
                    bool firstTimeBake = false;
                    // check if the object exists in the Profile Crv layer. If yes, move the objec to the Profile Curve_Archive layer
                    int layer_index = RhinoDocument.Layers.Find(layerActive, true);
                    bool layerExist = layer_index >= 0;
                    if (!layerExist)
                    {
                        RhinoDocument.Layers.Add(layerActive, Color.Black);
                    }

                    RhinoObject[] currentCrvs = RhinoDocument.Objects.FindByLayer(layerActive);

                        foreach (RhinoObject foo in currentCrvs)
                        {
                            bool crvExistInCurrentLayer = foo.Attributes.GetUserString("Profile ID") == ProfileID;
                            if (crvExistInCurrentLayer)
                            {
                                foo.Attributes.LayerIndex = ensureLayer(layerDeposit);
                                foo.CommitChanges();
                                currentVersion = int.Parse(foo.Attributes.GetUserString("Bake Version"));
                            }
                            firstTimeBake = !crvExistInCurrentLayer;
                        }
                        if (!firstTimeBake)
                        {
                            newVersion = currentVersion + 1;
                        }
                        else { newVersion = currentVersion; }
                    


                    // setup attributes
                    ref_ids.Clear();
                    string bakeTime = DateTime.Now.ToString();
                    ObjectAttributes att = new ObjectAttributes();
                    att.Name = ProfileID;
                    att.LayerIndex = ensureLayer(BakeLayer);
                    att.SetUserString("Profile ID", ProfileID);
                    att.SetUserString("Bake Time", bakeTime);
                    att.SetUserString("Bake Version", newVersion.ToString()) ;
                    att.SetUserString("Unique ID", uniqueID.ToString());
                    // bake objects to the canvas
                    foreach (GeometryBase crv in profileCrvReoriented)
                    {
                        Guid ref_id = Guid.NewGuid();
                        ref_id = RhinoDocument.Objects.Add(crv, att);
                        ref_ids.Add(ref_id);
                    }
                }
            }

            // Register output

            DA.SetData(0, successMap);
            DA.SetData(1, profile.uniqueID);
            DA.SetData(2, profile);

            ////////////////////////////////////////////////////////////////////////////////////////////
            /// Additional methods
            int ensureLayer(string lay)
            {
                int i = RhinoDocument.Layers.FindByFullPath(lay, -1);
                if (i < 0) { return RhinoDocument.Layers.Add(lay, Color.Black); }
                else { return i; }
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e565ebbf-b426-4710-a5e9-7c6aa4ce138e"); }
        }
    }
}