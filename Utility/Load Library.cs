using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace IEF_Toolbox.Utility
{
    public class Load_Library : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Load_Library class.
        /// </summary>
        public Load_Library()
          : base("Load Library", "Load Library",
              "Description",
              "IEF Toolbox", "01_Utility")
        {
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.septenary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Library Path", "L", "The path of the excel library", GH_ParamAccess.item);
            pManager.AddTextParameter("Worksheet", "W", "Specify the worksheet", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Listen?", "l", "Actively loading the library", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Read", "R", "Turn to true to read the excel sheet", GH_ParamAccess.item, false);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Library Object", "L", "The library object. Use the Deconstruct Library component to extract information", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = null;
            string worksheet = null;
            bool listen = false;
            bool read = false;

            bool success1 = DA.GetData(0, ref path);
            bool success2 = DA.GetData(0, ref worksheet);
            bool success3 = DA.GetData(0, ref listen);
            bool success4 = DA.GetData(0, ref read);



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
            get { return new Guid("6fdd53ff-7acc-4392-b395-a355ef4d4695"); }
        }
    }
}