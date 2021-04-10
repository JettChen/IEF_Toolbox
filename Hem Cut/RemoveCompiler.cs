using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.DocObjects;

namespace IEF_Toolbox
{
    public class RemoveCompiler : GH_Component
    {
        GH_Document GrasshopperDocument;
        IGH_Component Component;
        RhinoDoc RhinoDocument;
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public RemoveCompiler()
          : base("Remove Compiler", "Remove",
              "Combine remove geometries",
              "IEF_HCG", "Alpha_test")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Drill", "Drill", "Input drill holes from the DrillHoleSolver component",GH_ParamAccess.list);
            pManager.AddBrepParameter("Notch", "Notch", "Input notchs from the NotchSolver component", GH_ParamAccess.list);
            pManager.AddBrepParameter("Trim/Miter", "Trim/Miter", "Input Trim/Miters from the Trim/MitterSolver component", GH_ParamAccess.list);
            pManager.AddBrepParameter("Custom Geometries", "Custom", "Reference the custom remove geometry", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Remove","Remove","Compiled remove geometries",GH_ParamAccess.list);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Instantiate
            List<Brep> AllRemoveBreps = new List<Brep>();
            List<Brep> Drill = new List<Brep>();
            List<Brep> Notch = new List<Brep>();
            List<Brep> TrimMiter = new List<Brep>();
            List<Brep> Custom = new List<Brep>();
            // Get data from input
            DA.GetData(0, ref Drill);
            DA.GetData(1, ref Notch);
            DA.GetData(2, ref TrimMiter);
            DA.GetData(3, ref Custom);

            // Combine all geometries from the input
            addBrepToBrepList(Drill, AllRemoveBreps);
            addBrepToBrepList(Notch, AllRemoveBreps);
            addBrepToBrepList(TrimMiter, AllRemoveBreps);
            addBrepToBrepList(Custom, AllRemoveBreps);


            // output
            DA.SetDataList(0, AllRemoveBreps);

            //////// Methods starts here //////////////////
            void addBrepToBrepList (List<Brep> From, List<Brep> To)
            {
                foreach (Brep B in From)
                {
                    To.Add(B);
                }
            }
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
            get { return new Guid("c408f92a-ef9b-4bcf-8e71-fc103914cb36"); }
        }
    }
}