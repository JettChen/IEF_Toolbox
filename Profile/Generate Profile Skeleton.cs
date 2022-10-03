using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;
using IEF_Toolbox.Class;
using IEF_Toolbox;

namespace IEF_Toolbox.Profile
{
    public class GenerateProfileSkeleton : GH_Component
    {
        GH_Document GrasshopperDocument;
        IGH_Component Component;

        /// <summary>
        /// Initializes a new instance of the ProfileSkeleton class.
        /// </summary>
        public GenerateProfileSkeleton()
          : base("Generate Profile Skeleton", "Skeleton",
              "Analyze the profile and export the packaged Skeleton Object",
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
            pManager.AddGenericParameter("Profile Object", "P", "Insert the Profile Object collected through IEF Toolbox components", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Skeleton Object", "S", "The packed Skeleton Object. Use the ", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FrameProfile profile = new FrameProfile();
            bool success1 = DA.GetData(0, ref profile);
            if (!success1) { return; }

            ProfileSkeleton skr = new ProfileSkeleton(profile);
            DA.SetData(0, skr);
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
            get { return new Guid("7bb27b82-4b40-4e57-aa66-129867ec9e1d"); }
        }
    }
}