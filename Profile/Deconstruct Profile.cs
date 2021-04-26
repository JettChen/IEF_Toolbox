using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;

using IEF_Toolbox.Class;

namespace IEF_Toolbox.Profile
{
    public class Deconstruct_Profiles : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Deconstruct_Profiles()
          : base("Deconstruct Profile Object", "Deconstruct Profile",
              "Deconstruct Profile Objects and get the stored information",
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
            pManager.AddGenericParameter("Profile Object", "P", "The Profile Object collected through the IEF Toolbox components", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Profile Name", "Name", "The profile name", GH_ParamAccess.item);
            pManager.AddTextParameter("Profile Type", "Type", "The profile type", GH_ParamAccess.item);
            pManager.AddTextParameter("Profile Description", "D", "The profile descrition", GH_ParamAccess.item);
            pManager.AddCurveParameter("Profile Curves", "Crv", "The profile curves", GH_ParamAccess.list);
            pManager.AddCurveParameter("Profile Outside Curves", "oCrv", "The profile outside curve", GH_ParamAccess.item);
            pManager.AddCurveParameter("Profile Inside Curves", "iCrv", "The profile inside curves", GH_ParamAccess.list);
            pManager.AddPointParameter("Anchor", "A", "The datum anchor for the profile from Keymark", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "P", "The profile base plane", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Top Plane", "tP", "The profile top plane", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Bottom Plane", "bP", "The profile bottom plane", GH_ParamAccess.item);
            pManager.AddNumberParameter("Version", "v", "The verison number", GH_ParamAccess.item);
            pManager.AddTextParameter("Unique ID", "uID", "The unique ID to call the part", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FrameProfile foo = new FrameProfile();

            bool success1 = DA.GetData(0, ref foo);

            if (!success1) { return; }

            string profileID = foo.ProfileID;
            string profileDes = foo.ProfileDescription;
            foo.DetermineProfileTypeFromProfileDescription();
            string profileType = foo.ProfileType;

            List<Curve> profileCrv = foo.ProfileCrv;
            Point3d anchor = foo.Anchor;
            Plane basePlane = foo.BasePlane;
            Plane topPlane = foo.TopPlane;
            Plane bottomPlane = foo.BottomPlane;

            Curve outsideCrv = foo.OutsideCrv;
            List<Curve> insideCrv = foo.InsideCrv;

            int versionNum = foo.VersionNumber;
            string uniqueID = foo.uniqueID;


            DA.SetData(0, profileID);
            DA.SetData(1, profileType);
            DA.SetData(2, profileDes);
            DA.SetDataList(3, profileCrv);
            DA.SetData(4, outsideCrv);
            DA.SetDataList(5, insideCrv);
            DA.SetData(6, anchor);
            DA.SetData(7, basePlane);
            DA.SetData(8, topPlane);
            DA.SetData(9, bottomPlane);
            DA.SetData(10, versionNum);
            DA.SetData(11, uniqueID);
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
            get { return new Guid("c1596fbb-3015-4147-be38-4f8b65d7129b"); }
        }
    }
}