using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper;
using Grasshopper.Kernel;


namespace IEF_Toolbox.Utility
{
    public class MergePDFfromPath : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MergePDF from Path class.
        /// </summary>

        public MergePDFfromPath()
          : base("MergePDF from Path", "MergePDF from Path",
              "Feed path for PDF combination and the combined PDF will save to the target directory",
              "IEF Toolbox", "01_Utility")
        {
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.septenary; }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("sourceFolderPath", "sP", "source path", GH_ParamAccess.list);
            pManager.AddTextParameter("targetFolderPath", "tP", "target path", GH_ParamAccess.item);
            pManager.AddBooleanParameter("run", "run", "Turn to True to activate the copy", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Message", "S", "Status message", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA) {
            List<string> sourceDir = new List<string>();
            string targetDir = null;
            bool run = false;
            string outputMessage = null;

            bool success1 = DA.GetDataList<string>(0, sourceDir);
            bool success2 = DA.GetData<string>(1, ref targetDir);
            bool success3 = DA.GetData<bool>(2, ref run);

            if (success1 & success2 & success3)
            {
                // run the merge
                if (run) { MergePDF_by_part.MergePDFs(targetDir, sourceDir); }

                DA.SetData(0, outputMessage);
            }
        }



        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("CD9ACA4B-9541-4E00-AA82-9BE626055307"); }
        }

    }
}
