using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects;
using Grasshopper;
using Grasshopper.Kernel;


namespace IEF_Toolbox.Class

{
    public class FrameProfile
    ///subclass: ChickenHead, HorizontalReceptor, VerticalBlade, VerticalReceiver, BoxMullion, CornerMullion, FlatPerimeter, ShearBlock
    {
        /// <summary>
        /// field
        /// </summary>
        public string ProfileID = string.Empty;
        public string ProfileType = string.Empty;
        public string ProfileDescription = string.Empty;
        public Point3d Anchor = new Point3d();
        public Plane BasePlane = new Plane();

        public string Material = null;
        public string Finish = null;

        public int VersionNumber;
        public string uniqueID;

        public List<Curve> ProfileCrv = new List<Curve>();
        public Curve OutsideCrv = null;
        public List<Curve> InsideCrv = new List<Curve>();

        public bool IsCornerMullion = false;

        public int OrientationValue = 1; // 1 for typical orientation, -1 for flipped orientation

        public double Area;
        public Plane TopPlane;
        public Plane BottomPlane;

        ///constructor
        ///

        public FrameProfile() { }
        public FrameProfile(string id, ref List<Curve> profileCrv, Point3d anchor, Plane bplane) {
            ProfileID = id;
            ProfileCrv = profileCrv;
            Anchor = anchor;
            BasePlane = bplane;
            ResetProfileType();
            SortInsideOutside();
            CalcArea();
            CalcTopBottomPlane();
        }

        public FrameProfile(string id, ref List<Curve> profileCrv, Point3d anchor)
        {
            Point3d orig = Point3d.Origin;
            Vector3d planeX = new Vector3d(0.0, 0.0, -1.0);
            Vector3d planeY = new Vector3d(0.0, 1.0, 0.0);
            Plane defPlane = new Plane(orig, planeX, planeY);

            ProfileID = id;
            ProfileCrv = profileCrv;
            Anchor = anchor;
            BasePlane = defPlane;
            ResetProfileType();
            SortInsideOutside();
            CalcArea();
            CalcTopBottomPlane();
        }

        public FrameProfile(string id, ref List<Curve> profileCrv)
        {
            Point3d orig = Point3d.Origin;
            Vector3d planeX = new Vector3d(0.0, 0.0, -1.0);
            Vector3d planeY = new Vector3d(0.0, 1.0, 0.0);
            Plane defPlane = new Plane(orig, planeX, planeY);

            ProfileID = id;
            ProfileCrv = profileCrv;
            Anchor = orig;
            BasePlane = defPlane;
            ResetProfileType();
            SortInsideOutside();
            CalcArea();
            CalcTopBottomPlane();
        }

        public FrameProfile(string id)
        {
            Point3d orig = Point3d.Origin;
            Vector3d planeX = new Vector3d(0.0, 0.0, -1.0);
            Vector3d planeY = new Vector3d(0.0, 1.0, 0.0);
            Plane defPlane = new Plane(orig, planeX, planeY);

            //need a import module to grab the latest version of profileCrv from the profile library

            ProfileID = id;
            ProfileCrv = GetProfileCurves(id,Rhino.RhinoDoc.ActiveDoc);
            Anchor = orig;
            BasePlane = defPlane;
            ResetProfileType();
            SortInsideOutside();
            CalcArea();
            CalcTopBottomPlane();
        }


        /// <summary>
        /// Methods
        /// </summary>

        public void SortInsideOutside() {
            List<Curve> SortedCrvs = ProfileCrv.OrderBy(i => i.GetBoundingBox(false).Area).ToList();
            SortedCrvs.Reverse();
            if (SortedCrvs.Count > 0)
            {
                OutsideCrv = SortedCrvs[0];
                if (SortedCrvs.Count >1)
                for (int i = 1; i < SortedCrvs.Count; i++)
                {
                    InsideCrv.Add(SortedCrvs[i]);
                }
            }
            else { return; }
        }

        public virtual void ResetProfileType() {
            ProfileType = string.Empty;
        }

        public void UpdateChanges() { 
        
        }

        public Brep CreateBaseExtrusion(double CutLeft, double CutRight) {

            Vector3d moveL = new Vector3d(-CutLeft, 0, 0);

            List<Curve> moveLCrv = new List<Curve>(ProfileCrv);
            foreach (Curve crv in moveLCrv) { crv.Translate(moveL); }

            Point3d Left = new Point3d(-CutLeft, 0, 0);
            Point3d Right = new Point3d(CutRight, 0, 0);

            Curve extrusionPath = new LineCurve(Left,Right);
            Brep[] baseSrf = Brep.CreatePlanarBreps(moveLCrv, 0.001);
            BrepFace brepFace = baseSrf[0].Faces[0];

            Brep baseExtrusion = brepFace.CreateExtrusion(extrusionPath, true);

            if (baseExtrusion.SolidOrientation == BrepSolidOrientation.Inward)
            {
                baseExtrusion.Flip();
            }

            return baseExtrusion;
        }

        public List<Curve> GetProfileCurves(string ProfileID, RhinoDoc RhinoDocument)
        {
            List<Curve> ProfileCurves = new List<Curve>();
            string profileBaseLayers = "Profile Curve";
            int layer_index = RhinoDocument.Layers.FindByFullPath(profileBaseLayers, -1);
            bool layerExist = layer_index >= 0;
            if (!layerExist)
            {
                return null;
            }
            RhinoObject[] currentCrvs = RhinoDocument.Objects.FindByLayer(profileBaseLayers);
            foreach (RhinoObject crv in currentCrvs)
            {
                GeometryBase gb = crv.Geometry;
                Curve c = gb as Curve;
                if (crv.Attributes.Name == ProfileID && c != null)
                {
                    ProfileCurves.Add(c);
                }
            }
            return ProfileCurves;
        }

        public void DetermineProfileTypeFromProfileDescription ()
        {
            if (ProfileDescription != null)
            {
                if (ProfileDescription.Contains("MULLION")) {
                    if (ProfileDescription.Contains("CORNER")){
                        if (ProfileDescription.Contains("SHEAR BLOCK"))
                        {
                            IsCornerMullion = true;
                            ProfileType = "CORNER MULLION SHEAR BLOCK";
                        }
                        else
                        {
                            IsCornerMullion = true;
                            ProfileType = "CORNER MULLION";
                        }
                    } else{ProfileType = "MULLION"; }
                }
                else if (ProfileDescription.Contains("CHICKEN")) { ProfileType = "CHICKEN HEAD"; }
                else if (ProfileDescription.Contains("HORIZONTAL RECEPTOR")) { ProfileType = "HORIZONTAL RECEPTOR"; }
                else if (ProfileDescription.Contains("VERTICAL BLADE")) { ProfileType = "VERTICAL BLADE"; }
                else if (ProfileDescription.Contains("VERTICAL RECEPTOR")) { ProfileType = "VERTICAL RECEPTOR"; }
                else if (ProfileDescription.Contains("SHEAR BLOCK")) { ProfileType = "SHEAR BLOCK"; }
                else if (ProfileDescription.Contains("STARTER SILL")) { ProfileType = "STARTER SILL"; }
                else { ProfileType = ProfileDescription; }
            }
            else { return; }
        }
        public void CalcArea()
        {
            double outsideArea = AreaMassProperties.Compute(OutsideCrv).Area;
            double insideArea = AreaMassProperties.Compute(InsideCrv).Area;
            Area = outsideArea-insideArea;
        }

        public void CalcTopBottomPlane()
        {
            var bbx = OutsideCrv.GetBoundingBox(true);
            Point3d[] bbxPts = bbx.GetCorners();
            double[] PtsY = bbxPts.Select(pt => pt.Y).ToArray();
            double YMax = PtsY.Max();
            double YMin = PtsY.Min();

            TopPlane = new Plane(new Point3d(0, YMax, 0), new Vector3d(1, 0, 0), new Vector3d(0, 0, 1));
            BottomPlane = new Plane(new Point3d(0, YMin, 0), new Vector3d(1, 0, 0), new Vector3d(0, 0, 1));
        }
    }
}
