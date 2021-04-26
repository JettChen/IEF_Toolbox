using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace IEF_Toolbox.Utility
{
    public class Flip_BrepFace_Normal : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Flip_BrepFace_Normal class.
        /// </summary>
        public Flip_BrepFace_Normal()
          : base("Flip BrepFace_Normal", "Flip",
              "Reverse the normal direction of all faces of the input brep",
              "IEF Toolbox", "01_Utility")
        {
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.senary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("brep_Brep to flip", "B", "The brep to flip", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("brep_Brep flipped", "B", "The brep flipped", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep brep = new Brep();

            bool success1 = DA.GetData(0, ref brep);
            if (!success1) { return; }

            brep.Flip();

            DA.SetData(0, brep);
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
            get { return new Guid("921f0611-449e-462d-90cc-975ca4e74ddf"); }
        }
    }
}