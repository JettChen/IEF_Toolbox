using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Grasshopper;
using Grasshopper.Kernel;

using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace IEF_Toolbox.Utility
{
    public class MergePDF_by_part : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MergePDF by Part class.
        /// </summary>
        public MergePDF_by_part()
          : base("MergePDF by Part", "MergePDF by Part",
              "Look for the individual PDFs in the given directories, combine the PDF sheets, and export a single PDF per Part to the taget directory",
              "IEF Toolbox", "01_Utility")
        {
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.septenary; }
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("partNumber", "pN", "List of parts to run the copy", GH_ParamAccess.item);
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
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string pN = null;
            List<string> sourceFolderDir = new List<string>();
            string targetFolderDir = null;
            bool run = false;
            string outputMessage = null;

            bool success0 = DA.GetData<string>(0, ref pN);
            bool success1 = DA.GetDataList<string>(1, sourceFolderDir) ;
            bool success2 = DA.GetData<string>(2, ref targetFolderDir);
            bool success3 = DA.GetData<bool>(3, ref run);

            if (success0 & success1 & success2 & success3)
            {
                // get file path from inputs
                List<string> sourceDir = new List<string>();
                foreach (string dir in sourceFolderDir) { sourceDir.Add(dir + "\\" + pN + ".pdf"); }
                string targetDir = targetFolderDir + "\\" + pN + ".pdf";

                // run the merge
                if (run) { MergePDFs(targetDir, sourceDir);
                    outputMessage = pN + " PDF successfully merged";
                }

                DA.SetData(0, outputMessage);
            }
        }

        public static void MergePDFs(string targetPath, List<string> pdfs)
        {
            using (var targetDoc = new PdfDocument())
            {
                foreach (var pdf in pdfs)
                {
                    using (var pdfDoc = PdfReader.Open(pdf, PdfDocumentOpenMode.Import))
                    {
                        for (var i = 0; i < pdfDoc.PageCount; i++)
                            targetDoc.AddPage(pdfDoc.Pages[i]);
                    }
                }
                targetDoc.Save(targetPath);
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
            get { return new Guid("9A40E630-2B0F-4D6D-B904-3B4796F1967D"); }
        }

    }
}
