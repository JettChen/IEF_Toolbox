using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace IEF_Toolbox
{
    public class Cull_First_Last : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Cull_First_Last class.
        /// </summary>
        public Cull_First_Last()
          : base("Cull First&Last", "CullFL",
              "Remove the first and the last items in the list",
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
            pManager.AddGenericParameter("List", "L", "List to cull", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Force to run", "f", "Set to True if want to execute on lists with fewer than 2 items", GH_ParamAccess.item, false);
            pManager[1].Optional =true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Culled List", "C", "List with first and last items removed", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<object> items = new List<object>();
            bool force = false;
            bool success1 = DA.GetDataList(0, items);
            bool success2 = DA.GetData(1, ref force);
            if (!success1) { return; }

            List<object> newItems = new List<object>();

            int count = items.Count;

            if (count < 2 && force == false)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "List length fewer than 2. Set f to True if want to cull all items from those lists.");
                newItems = null;
            }
            else if (count < 2 && force == true)
            {
                newItems.Clear();
            }
            else
            {
                for (int i = 1; i < items.Count - 1; i++)
                {
                    newItems.Add(items[i]);
                }
            }

            DA.SetDataList(0, newItems);
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
            get { return new Guid("cbdb52ef-1c0e-4599-8aff-2e0620036c55"); }
        }
    }
}