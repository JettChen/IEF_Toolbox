using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.GUI;
using Rhino.Geometry;

namespace IEF_Toolbox.Utility
{
    public class Copy_File_From_To : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Boolean_Difference_Slow class.
        /// </summary>
        public Copy_File_From_To()
          : base("Copy File From To", "Copy",
              "Copy file from a sourcePath to a targetPath based on partNumber and fileExtension",
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
            pManager.AddTextParameter("partNumber", "pN", "List of parts to run the copy", GH_ParamAccess.item);
            pManager.AddTextParameter("fileExtension", "ext", "file extension", GH_ParamAccess.item);
            pManager.AddTextParameter("sourcePath", "sP", "source path", GH_ParamAccess.item);
            pManager.AddTextParameter("targetPath", "tP", "target path", GH_ParamAccess.item);
            pManager.AddBooleanParameter("run", "run", "Turn to True to activate the copy", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
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
            string sourceDir = null;
            string targetDir = null;
            string ext = null;
            bool run = false;

            bool success0 = DA.GetData<string>(0, ref pN);
            bool success1 = DA.GetData<string>(1, ref ext);
            bool success2 = DA.GetData<string>(2, ref sourceDir);
            bool success3 = DA.GetData<string>(3, ref targetDir);
            bool success4 = DA.GetData<bool>(4, ref run);

            string outputMessage = null;

            string sDir = sourceDir + "\\" + pN + ext;
            string tDir = targetDir + "\\" + pN + ext;


            
            if (run){
                // check if file under path exists // if not, create folder path

                

                File.Copy(sDir, tDir);
            }

            DA.SetData(0, outputMessage);
        }

        private bool Test_Folder_Exists(string folder_path)
        {
            bool folder_exist = false;

            return folder_exist;
        }

        private bool Test_sourceFile_Exists(string file_path)
        {
            bool file_exist = false;

            return file_exist;
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

        public override Guid ComponentGuid
        {
            get { return new Guid("D4F89E9F-D181-4ED4-87E2-EEC82505C6C0"); }


        }

    }
}

