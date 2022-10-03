using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects;
using IEF_Toolbox.Class;


namespace IEF_Toolbox.Profile
{
    public class Acquire_Profile : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Acquire_Die_Profile class.
        /// </summary>
        /// 
        GH_Document GrasshopperDocument;
        IGH_Component Component;
        RhinoDoc RhinoDocument;
        public Acquire_Profile()
          : base("Acquire Profile", "Acquire",
              "Use the Profile ID to acquire the latest die profile in the Profile Curve Layer",
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
            pManager.AddTextParameter("Profile ID","ID","The Profile ID",GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
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

            string iProfileID = null;
            bool success1 = DA.GetData(0, ref iProfileID);
            if(!success1) { return; }

            // get curve geometries in the Profile Curve Layer
            List<Curve> ProfileCurves = new List<Curve>();
            string profileBaseLayers = "Profile Curve";
            int layer_index = RhinoDocument.Layers.FindByFullPath(profileBaseLayers, -1);
            bool layerExist = layer_index >= 0;
            if (!layerExist)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Profile Curve layer does not exist in the document. Please reference the Profile Library");
                return;
            }
            RhinoObject[] currentCrvs = RhinoDocument.Objects.FindByLayer(profileBaseLayers);
            List<RhinoObject> objs = new List<RhinoObject>();
            foreach (RhinoObject crv in currentCrvs)
            {
                GeometryBase gb = crv.Geometry;
                Curve c = gb as Curve;
                if (crv.Attributes.Name == iProfileID && c != null)
                {
                    ProfileCurves.Add(c);
                    objs.Add(crv);
                }
            }

            if (ProfileCurves.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Profile Curve not found. Please check if the profile exists in the Profile Curve layer or if the ID input is correct");
                return;
            }


            //Construct the profile Object
            FrameProfile profile = new FrameProfile();
            Point3d orig = Point3d.Origin;
            Vector3d planeX = new Vector3d(0.0, 0.0, -1.0);
            Vector3d planeY = new Vector3d(0.0, 1.0, 0.0);
            Plane defPlane = new Plane(orig, planeX, planeY);
            RhinoObject obj = objs[0];

            profile.ProfileID = iProfileID;
            profile.ProfileCrv = ProfileCurves;
            profile.ProfileDescription = obj.Attributes.GetUserString("Profile Description");
            profile.ProfileType = obj.Attributes.GetUserString("Profile Type");
            profile.Material = obj.Attributes.GetUserString("Material");
            profile.Finish = obj.Attributes.GetUserString("Finish");
            profile.Anchor = orig;
            profile.BasePlane = defPlane;
            profile.ResetProfileType();
            profile.SortInsideOutside();
            profile.CalcArea();
            profile.CalcTopBottomPlane();

            int ver;
            int.TryParse(obj.Attributes.GetUserString("Bake Version"),out ver);
            profile.VersionNumber = ver;
            profile.uniqueID = obj.Attributes.GetUserString("Unique ID");

            DA.SetData(0, profile);
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
            get { return new Guid("b4e46819-63bf-4336-afe7-eefeafd06b84"); }
        }
    }
}