using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino;

namespace IEF_Toolbox.Utility
{
    public class Extrude_Left_Right : GH_Component
    {
        GH_Document GrasshopperDocument;
        IGH_Component Component;
        RhinoDoc RhinoDocument;
        /// <summary>
        /// Initializes a new instance of the Extrude_Left_Right class.
        /// </summary>
        public Extrude_Left_Right()
          : base("Extrude Left Right", "Extrude LR",
              "Extrude the input planar curve object to the left and right value",
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
            pManager.AddCurveParameter("curve_Planar Curves", "C", "The base profile curve", GH_ParamAccess.list);
            pManager.AddNumberParameter("double_Cut Left", "L", "The Cut Left extrusing value", GH_ParamAccess.item);
            pManager.AddNumberParameter("double_Cut Right", "R", "The Cut Right extrusing value", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("brep_Extrusion Base", "E", "The extrusion", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Component = this;
            GrasshopperDocument = this.OnPingDocument();
            RhinoDocument = RhinoDoc.ActiveDoc;

            List<Curve> curves = new List<Curve>();
            double Left = 0.0;
            double Right = 0.0;

            bool success1 = DA.GetDataList(0, curves);
            bool success2 = DA.GetData(1, ref Left);
            bool success3 = DA.GetData(2, ref Right);

            if (!success1 || !success2 || !success3) { return; }

            Brep baseExtrusion = ExtrudeCrvLR(curves, Left, Right) ;

            DA.SetData(0, baseExtrusion);


            /// Extra Methods
            Brep ExtrudeCrvLR(List<Curve> Curves, double CutLeft, double CutRight)
            {
                Vector3d moveL = new Vector3d(-CutLeft, 0, 0);

                List<Curve> moveLCrv = new List<Curve>(Curves);
                foreach (Curve crv in moveLCrv) { crv.Translate(moveL); }

                Point3d PtLeft = new Point3d(-CutLeft, 0, 0);
                Point3d PtRight = new Point3d(CutRight, 0, 0);

                Curve extrusionPath = new LineCurve(PtLeft, PtRight);
                Brep[] baseSrf = Brep.CreatePlanarBreps(moveLCrv, 0.001);
                BrepFace brepFace = baseSrf[0].Faces[0];

                Brep BaseExtrusion = brepFace.CreateExtrusion(extrusionPath, true);
                
                if (BaseExtrusion.SolidOrientation == BrepSolidOrientation.Inward) {
                    BaseExtrusion.Flip();
                }

                return BaseExtrusion;
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
            get { return new Guid("5fa75ee3-f38c-4390-b1fa-1b7886efbf8a"); }
        }
    }
}