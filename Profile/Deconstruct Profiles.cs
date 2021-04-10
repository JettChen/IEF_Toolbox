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
          : base("Deconstruct Profile Object", "Deconstruct",
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
            pManager.AddTextParameter("Profile Type", "Type", "The profile name", GH_ParamAccess.item);
            pManager.AddCurveParameter("Profile Curves", "Crv", "The profile name", GH_ParamAccess.list);
            pManager.AddCurveParameter("Profile Outside Curves", "oCrv", "The profile name", GH_ParamAccess.item);
            pManager.AddCurveParameter("Profile Inside Curves", "iCrv", "The profile name", GH_ParamAccess.list);
            pManager.AddPointParameter("Anchor", "A", "The profile name", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "P", "The profile name", GH_ParamAccess.item);
            pManager.AddNumberParameter("Version", "v", "The verison number", GH_ParamAccess.item);
            pManager.AddTextParameter("Unique ID", "uID", "The profile name", GH_ParamAccess.item);

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
            string profileType = foo.ProfileType;
            List<Curve> profileCrv = foo.ProfileCrv;
            Point3d anchor = foo.Anchor;
            Plane basePlane = foo.BasePlane;

            Curve outsideCrv = foo.OutsideCrv;
            List<Curve> insideCrv = foo.InsideCrv;

            int versionNum = foo.VersionNumber;
            string uniqueID = foo.uniqueID;


            DA.SetData(0, profileID);
            DA.SetData(1, profileType);
            DA.SetDataList(2, profileCrv);
            DA.SetData(3, outsideCrv);
            DA.SetDataList(4, insideCrv);
            DA.SetData(5, anchor);
            DA.SetData(6, basePlane);
            DA.SetData(7, versionNum);
            DA.SetData(8, uniqueID);
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