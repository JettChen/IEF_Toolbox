using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Grasshopper.GUI;
using Rhino.Geometry;

namespace IEF_Toolbox
{
    public class AccessDrillSizeChart : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        /// 
        GH_Document GrasshopperDocument;
        IGH_Component Component;

        bool IsThereInput = false;



        public AccessDrillSizeChart()
          : base("Access Drill Size Chart", "DrillChart",
              "Insert button and click to accsess the Tap & Clearance Drill Size Chart on LittleMachineShop.com",
              "IEF Toolbox", "03_Hem Cut")
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
            pManager.AddBooleanParameter("Button", "B", "Insert button and click to accsess the Tap & Clearance Drill Size Chart on LittleMachineShop.com", GH_ParamAccess.item,false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Component = this;
            GrasshopperDocument = this.OnPingDocument();


            //System.Guid componentGUID = this.Component.InstanceGuid;
            // bool IsComponentAdded = Grasshopper.GUI.Canvas.GH_Canvas.Document_ObjectsAddedEventHandler(GrasshopperDocument, this);



            ////////////////////////////////////////////////////////////////
            /// This part doesn't work. What is the condition to trigger the function?

            //int inputCount = 0;

            //if (inputCount == 0)
            //{
            //    var button = new Grasshopper.Kernel.Special.GH_ButtonObject();
            //    button.CreateAttributes();
            //    button.Attributes.Pivot = new PointF((float)this.Component.Attributes.DocObject.Attributes.Bounds.Left - button.Attributes.Bounds.Width - 10, (float)this.Component.Params.Input[0].Attributes.Bounds.Y);
            //    GrasshopperDocument.AddObject(button, false);
            //    this.Component.Params.Input[0].AddSource(button);
            //    inputCount += 1;
            //    if (Component.Params.Input[0].SourceCount == 0)
            //    {
            //        inputCount -= 1;
            //    }
            //}

            ////////////////////////////////////////////////////////////////

            bool buttonClicked = new bool();
            bool Success = DA.GetData(0, ref buttonClicked);
            if (!Success) { return; }

            //if (!DA.GetData(0, ref buttonClicked)) {
            //    var button = new Grasshopper.Kernel.Special.GH_ButtonObject();
            //    button.CreateAttributes();
            //    button.Attributes.Pivot = new PointF((float)this.Component.Attributes.DocObject.Attributes.Bounds.Left - button.Attributes.Bounds.Width - 10, (float)this.Component.Params.Input[0].Attributes.Bounds.Y);
            //    GrasshopperDocument.AddObject(button, false);
            //    this.Component.Params.Input[0].AddSource(button);
            //} 

            if (buttonClicked)
            {
                string URL = "https://littlemachineshop.com/reference/tapdrill.php";
                System.Diagnostics.Process.Start(URL);
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
            get { return new Guid("79cd9565-5927-4522-bca5-ddfdfdd95b39"); }
        }


        /// reference this forum thread to build the custom attributes in the battery:
        /// https://discourse.mcneel.com/t/how-to-script-button-on-the-component/56431
        /// https://discourse.mcneel.com/t/programming-on-c-additional-radio-buttons-drop-down-menus-sliders-etc-in-the-grasshopper-node/118907




    }
}