using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using IEF_Toolbox.Class;

namespace IEF_Toolbox.Profile
{
    public class Deconstruct_Skeleton : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Deconstruct_Skeleton class.
        /// </summary>
        public Deconstruct_Skeleton()
          : base("Deconstruct Skeleton Object", "Deconstruct Skeleton",
              "Deconstruct Skeleton Objects and get the stored information",
              "IEF Toolbox", "02_Profile")
        {
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Skeleton Object", "S", "The Profile Object collected through the IEF Toolbox components", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Profile Name", "Name", "The profile name", GH_ParamAccess.item);
            pManager.AddTextParameter("Profile Type", "Type", "The profile type", GH_ParamAccess.item);
            pManager.AddPointParameter("Anchor", "A", "The datum anchor for the profile from Keymark", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Base Plane", "P", "The base plane", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Top Plane", "tP", "The top plane", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Base Plane", "bP", "The base plane", GH_ParamAccess.item);
            pManager.AddCurveParameter("Screw Chase Hole", "H", "Screw chase holes", GH_ParamAccess.list);
            pManager.AddPointParameter("Screw Chase Hole Location", "L", "Screw chase hole location", GH_ParamAccess.list);
            pManager.AddNumberParameter("Screw Chase Hole Diameter", "D", "Screw chase hole diameter", GH_ParamAccess.list);
            pManager.AddRectangleParameter("Bounding Box", "B", "Profile boundingbox", GH_ParamAccess.item);
            pManager.AddNumberParameter("Profile Width/Height", "W", "The width or height of the profile", GH_ParamAccess.item);
            pManager.AddNumberParameter("Profile Depth", "D", "The Depth of the profile", GH_ParamAccess.item);
            pManager.AddNumberParameter("Profile Area", "A", "The area for the profile", GH_ParamAccess.item);
            pManager.AddNumberParameter("Version", "v", "The verison number", GH_ParamAccess.item);
            pManager.AddTextParameter("Unique ID", "uID", "The unique ID to call the part", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ProfileSkeleton skr = new ProfileSkeleton();
            bool success1 = DA.GetData(0, ref skr);
            if (!success1) { return; }

            string name = skr.ProfileID;
            string type = skr.ProfileType;
            Point3d anchor = skr.Anchor;
            Plane p = skr.BasePlane;
            Plane tp = skr.TopPlane;
            Plane bp = skr.BottomPlane;
            List<Circle> screwChaseHole = skr.ScrewChaseHole;
            List<Point3d> screwChaseLocation = skr.ScrewChaseLocation;
            List<double> screwChaseDiameter = skr.ScrewChaseDiameter;
            int ver = skr.VersionNumber;
            string uniqueID = skr.uniqueID;
            Rectangle3d bbx = skr.BoundingRectangle;
            double w = skr.WidthOrHeight;
            double d = skr.Depth;
            double area = skr.Area;


            DA.SetData(0, name);
            DA.SetData(1, type);
            DA.SetData(2, anchor);
            DA.SetData(3, p);
            DA.SetData(4, tp);
            DA.SetData(5, bp);
            DA.SetDataList(6, screwChaseHole);
            DA.SetDataList(7, screwChaseLocation);
            DA.SetDataList(8, screwChaseDiameter);
            DA.SetData(9, bbx);
            DA.SetData(10, w);
            DA.SetData(11, d);
            DA.SetData(12, area);
            DA.SetData(13, ver);
            DA.SetData(14, uniqueID);
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
            get { return new Guid("e2838a64-f754-40b2-a585-b73c3e5b569b"); }
        }
    }
}