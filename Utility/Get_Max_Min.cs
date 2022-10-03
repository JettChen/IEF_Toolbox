using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace IEF_Toolbox
{
    public class Get_Max_Min : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Get_Max_Min class.
        /// </summary>
        public Get_Max_Min()
          : base("Get Max Min", "Max Min",
              "Get the maximum and minimum value from a list of number values",
              "IEF Toolbox", "01_Utility")
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
            pManager.AddNumberParameter("Number Value", "N", "List of number values", GH_ParamAccess.list);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Max", "Max", "Maxium value from the list", GH_ParamAccess.item);
            pManager.AddNumberParameter("Min", "Min", "Minimum value from the list", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> nums = new List<double>();
            bool success1 = DA.GetDataList(0, nums);

            if (!success1) { return; }

            double max = nums.Max();
            double min = nums.Min();

            DA.SetData(0, max);
            DA.SetData(1, min);
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
            get { return new Guid("fa1ab0eb-726b-4508-b64d-b56952844512"); }
        }
    }
}