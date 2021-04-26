using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace IEF_Toolbox.Utility
{
    public class Get_String_Index : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Get_String_Index class.
        /// </summary>
        public Get_String_Index()
          : base("Get String Indexes", "Index",
              "Returns all indexes of the item from the input list",
              "IEF Toolbox", "01_Utility")
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
            pManager.AddTextParameter("List", "L", "List to search",GH_ParamAccess.list);
            pManager.AddTextParameter("Item", "I", "Item to search", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        { 
            pManager.AddIntegerParameter("Index", "i", "All indexes of the item", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> list = new List<string>();
            string item = null;

            bool success1 = DA.GetDataList(0, list);
            bool success2 = DA.GetData(1, ref item);

            if (!success1 || !success2) { return; }

            List<int> ind = new List<int>();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == item)
                { ind.Add(i); }
                else { continue; }
            }

            DA.SetDataList(0, ind);
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
            get { return new Guid("eabfcb7a-eb26-49ce-807b-9a40f5bb29d0"); }
        }
    }
}