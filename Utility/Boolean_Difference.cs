using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace IEF_Toolbox
{
    public class Boolean_Difference : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Boolean_Difference class.
        /// </summary>
        public Boolean_Difference()
          : base("Boolean Difference", "BD",
              "Slightly faster Boolean Difference operation than the built-in Solid Difference component",
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
            pManager.AddNumberParameter("Tolerance", "t", "Tolerance of the boolean operation. The default value is set" +
                " to the RhinoDoc.ModelAbsoluteTolerance", GH_ParamAccess.item);
            pManager[2].Optional = true;
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
            List<Brep> baseG = new List<Brep>();
            List<Brep> removeG = new List<Brep>();
            double tol = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;


            bool success1 = DA.GetDataList(0, baseG);
            bool success2 = DA.GetDataList(1, removeG);
            bool success3 = DA.GetData(2, ref tol);

            if (!success1) { return; }

            Brep[] result = Brep.CreateBooleanDifference(baseG, removeG, tol);

            DA.SetDataList(0, result);
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
            get { return new Guid("20195927-24cd-4523-9e7c-1c9a8cc69784"); }
        }
    }
}