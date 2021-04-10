using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.GUI.Canvas;
using Grasshopper.GUI;
using System.Net;

namespace IEF_Toolbox
{
    public class ChartUITest : GH_Component
    {
        GH_Document GrasshopperDocument;
        IGH_Component Component;
        /// <summary>
        /// Initializes a new instance of the ChartTest class.
        /// </summary>
        public ChartUITest()
          : base("UI Test_AccessDrillSizeChart", "DrillChart",
              "Insert button and click to accsess the Tap & Clearance Drill Size Chart on LittleMachineShop.com",
              "IEF_HCG", "Alpha_test")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("button", "B", "click to access", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }


        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            Grasshopper.Kernel.Special.GH_ButtonObject button = new Grasshopper.Kernel.Special.GH_ButtonObject();
            button.CreateAttributes();
            button.Attributes.Pivot = new PointF((float)this.Component.Attributes.DocObject.Attributes.Bounds.Left - button.Attributes.Bounds.Width - 10, (float)this.Component.Params.Input[0].Attributes.Bounds.Y);
            GrasshopperDocument.AddObject(button, false);
            this.Component.Params.Input[0].AddSource(button);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            bool buttonClicked = new bool();
            bool Success = DA.GetData(0, ref buttonClicked);
            if (!Success) return; 

            if (buttonClicked)
            {
                string URL = "https://littlemachineshop.com/reference/tapdrill.php";
                System.Diagnostics.Process.Start(URL);
                return;
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
            get { return new Guid("d3b64f59-2a13-4b08-a036-a7f19cc2505c"); }
        }



        //reference

        //public override void CreateAttributes()
        //{
        //    m_attributes = new ChartUITestAttributes(Me);
        //}

        //public class ChartUITestAttributes : GH_Attributes<ChartUITest>
        //{
        //    public ChartUITestAttributes(ChartUITest owner) : base(owner) { }

        //    protected override void Layout()
        //    {
        //        // Compute the width of the NickName of the owner (plus some extra padding), 
        //        // then make sure we have at least 80 pixels.
        //        int width = GH_FontServer.StringWidth(Owner.NickName, GH_FontServer.Standard);
        //        width = Math.Max(width + 10, 80);

        //        // The height of our object is always 60 pixels
        //        int height = 60;

        //        // Assign the width and height to the Bounds property.
        //        // Also, make sure the Bounds are anchored to the Pivot
        //        Bounds = new RectangleF(Pivot, new SizeF(width, height));
        //    }


            //protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
            //{
            //    base.Render(canvas, graphics, channel);



            //}
        //}

    }
}