using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace IEF_Toolbox
{
    public class Flip_Srf_UV : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Flip_Srf_UV class.
        /// </summary>
        public Flip_Srf_UV()
          : base("Flip Surface UV", "Flip UV",
              "Flip the surface UV",
              "IEF Toolbox", "01_Utility")
        {
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quinary; }
        }


        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "S", "The surface for UV flipping", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Flipped Surface", "F", "The flipped surface", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Surface> output = new List<Surface>();
            List<Surface> srfs = new List<Surface>();

            bool success1 = DA.GetDataList(0, srfs);
            if (!success1) { return; }


            for (int i = 0; i < srfs.Count; i++)
            {
                Surface srf = srfs[i].Transpose();
                output.Add(srf);
            }

            DA.SetDataList(0, output);
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
            get { return new Guid("b68a8af8-51d2-4fb9-bb85-3b34c99e6427"); }
        }
    }
}