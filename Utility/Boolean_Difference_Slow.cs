using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace IEF_Toolbox.Utility
{
    public class Boolean_Difference_Slow : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Boolean_Difference_Slow class.
        /// </summary>
        public Boolean_Difference_Slow()
          : base("Boolean Difference_Slow", "BD",
              "Slightly slower Boolean Difference operation by testing the correct tolerance value",
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
            pManager.AddBrepParameter("Base Geometry", "B", "List of base geometries", GH_ParamAccess.list);
            pManager.AddBrepParameter("Remove Geometry", "R", "List of remove geometries", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Result", "R", "The result geometry of the boolean difference operation", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double tol = 1;

            List<Brep> baseG = new List<Brep>();
            List<Brep> removeG = new List<Brep>();

            bool success1 = DA.GetDataList(0, baseG);
            bool success2 = DA.GetDataList(1, removeG);

            if (!success1) { return; }

            Brep[] result = CalcBDResultToTheBestTol(baseG, removeG, tol, 15);

            DA.SetDataList(0, result);


            Brep[] CalcBDResultToTheBestTol(List<Brep> BaseExtrusion, List<Brep> RemoveGeometry, double tolerance, int iteration) {
                Brep[] a = Brep.CreateBooleanDifference(BaseExtrusion, RemoveGeometry, tolerance);
                if (a.Length != BaseExtrusion.Count && iteration > 0)
                {
                    double NewTolerance = tolerance / 2;
                    int NewIteration = iteration - 1;
                    a = CalcBDResultToTheBestTol(BaseExtrusion, RemoveGeometry, NewTolerance, NewIteration);
                }
                return a;
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
            get { return new Guid("fadddf57-6be9-48a6-bbba-1f6efccac624"); }
        }
    }
}