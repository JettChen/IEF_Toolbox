using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace IEF_Toolbox
{
    public class Mass_Concatenate : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Mass_Concatenate()
          : base("Mass Concatenate", "MassConcat",
              "Concatenate all items in a list with a specified separator",
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
            pManager.AddTextParameter("Items", "I", "List of items for concatenation", GH_ParamAccess.list);
            pManager.AddTextParameter("Separator", "s", "Specified item separator", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Concatenation", "C", "Concatenated items", GH_ParamAccess.item); 
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> items = new List<string>();
            string sep = "";

            bool success1  = DA.GetDataList(0, items);
            bool success2 = DA.GetData(1, ref sep);

            if (!success1) { return; }

            List<string> newString = new List<string>();
            for (int i = 0; i < items.Count; i++)
            {
                newString.Add(items[i]);
                if (i < items.Count - 1)
                {
                    newString.Add(sep);
                }
            }

            string concatenation = String.Concat(newString);

            DA.SetData(0, concatenation);
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
            get { return new Guid("f067cd5f-f938-4560-b808-71c92a02bb11"); }
        }
    }
}