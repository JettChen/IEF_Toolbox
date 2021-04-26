using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;
using IEF_Toolbox.Class;

namespace IEF_Toolbox.Hem_Cut
{
    public class Deconstruct_Drill_Hole : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Deconstruct_Drill_Hole class.
        /// </summary>
        public Deconstruct_Drill_Hole()
          : base("Deconstruct Drill Hole", "Deconstruct Drill Hole",
              "Deconstruc the drill hole object generated from the TapClearanceDrillHole component",
              "IEF Toolbox", "03_Hem Cut")
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
            pManager.AddGenericParameter("Drill Object", "D", "Input the Drill Object", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Drill Size Decimal", "Ds", "Drill Size Decimal", GH_ParamAccess.item);
            pManager.AddTextParameter("Drill Size", "D", "Drill Size", GH_ParamAccess.item);
            pManager.AddTextParameter("Screw Size", "S", "Screw Size", GH_ParamAccess.item);
            pManager.AddNumberParameter("Minor Diameter", "Mi", "Minor Diameter", GH_ParamAccess.item);
            pManager.AddNumberParameter("Major Diameter", "Ma", "Major Diameter", GH_ParamAccess.item);
            pManager.AddTextParameter("Drill Type", "DT", "Drill Type", GH_ParamAccess.item);
            pManager.AddTextParameter("Fit Type", "FT", "Fit Type", GH_ParamAccess.item);
            pManager.AddTextParameter("Base Material", "M", "Base Material", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            TapClearanceDrillHole hole = new TapClearanceDrillHole();
            bool success1 = DA.GetData(0, ref hole);
            if (!success1) { return; }

            double drillSizeDecim = hole.DrillSizeDecimalEquiv;
            string drillSize = hole.DrillSize;
            string screwSize = hole.ScrewSize;
            double minorDia = hole.MinorDiameter;
            double majorDia = hole.MajorDiameter;
            string DrillType = hole.DrillType;
            string FitType = hole.FitType;
            string baseMaterial = hole.BaseMaterial;

            DA.SetData(0, drillSizeDecim);
            DA.SetData(1, drillSize);
            DA.SetData(2, screwSize);
            DA.SetData(3, minorDia);
            DA.SetData(4, majorDia);
            DA.SetData(5, DrillType);
            DA.SetData(6, FitType);
            DA.SetData(7, baseMaterial);

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
            get { return new Guid("6626aff8-b75d-4ad4-b6d5-4f514268786a"); }
        }
    }
}