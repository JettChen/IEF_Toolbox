using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;


namespace IEF_Toolbox.Interface
{
    //public class Test_button : GH_Component
    //{
    //    /// <summary>
    //    /// Initializes a new instance of the Test_button class.
    //    /// </summary>
    //    public Test_button()
    //      : base("Test_button", "Test button",
    //          "Description",
    //          "IEF_Toolbox", "Test")
    //    {
    //    }

    //    /// <summary>
    //    /// Registers all the input parameters for this component.
    //    /// </summary>
    //    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    //    {
    //    }

    //    /// <summary>
    //    /// Registers all the output parameters for this component.
    //    /// </summary>
    //    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    //    {
    //    }

    //    /// <summary>
    //    /// This is the method that actually does the work.
    //    /// </summary>
    //    /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
    //    protected override void SolveInstance(IGH_DataAccess DA)
    //    {
    //    }

    //    /// <summary>
    //    /// Provides an Icon for the component.
    //    /// </summary>
    //    protected override System.Drawing.Bitmap Icon
    //    {
    //        get
    //        {
    //            //You can add image files to your project resources and access them like this:
    //            // return Resources.IconForThisComponent;
    //            return null;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets the unique ID for this component. Do not change this ID after release.
    //    /// </summary>
    //    public override Guid ComponentGuid
    //    {
    //        get { return new Guid("0ec6aaa5-9e82-4368-9a98-6c4125e97719"); }
    //    }
    //}


    public class TestButtonObject : GH_Param<GH_Boolean> {
        public TestButtonObject() :
                base(new GH_InstanceDescription("Test Button", "Access", "smile","IEF Toolbox","Test"))
        { }


        public override void CreateAttributes()
        {
            m_attributes = new TestButtonAttributes(this);
        }

        public override Guid ComponentGuid
        { get { return new Guid("0ec6aaa5-9e82-4368-9a98-6c4125e97719"); } }

        private bool m_button = false;
        public bool Value { 
            get { return m_button; }
            set { m_button = value; }
        }

    }
    public class TestButtonAttributes : GH_Attributes<TestButtonObject> {
        private bool[] m_button;
        public TestButtonAttributes(TestButtonObject owner)
         : base(owner)
        {
            m_button = new bool[2] { true, false };
        }

        public override bool HasInputGrip { get { return false; } }
        public override bool HasOutputGrip { get { return false; } }

        private const int ButtonWidth = 96;
        private const int ButtonHeight = 24;

        // Setup the Component size
        protected override void Layout()
        {
            //Lock this object to the pixel grid. 
            //I.e., do not allow it to be position in between pixels.
            Pivot = GH_Convert.ToPoint(Pivot);
            Bounds = new RectangleF(Pivot, new SizeF(ButtonWidth, ButtonHeight));
        }

        private Rectangle Button() {

            int x = Convert.ToInt32(Pivot.X);
            int y = Convert.ToInt32(Pivot.Y);
            return new Rectangle(x + ButtonWidth, y+ ButtonHeight,ButtonWidth, ButtonHeight);
        }

        private bool bValue(int foo) {
            return m_button[foo];
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                RectangleF button = Button();
                if (button.Contains(e.CanvasLocation)) {
                    bool value = m_button[0];
                    if (value)
                    {
                        Owner.RecordUndoEvent("Accessed");
                        Owner.Value = value;
                        Owner.ExpireSolution(true);
                    }
                    value = m_button[1];
                    return GH_ObjectResponse.Handled;
                }
            }

            return base.RespondToMouseDown(sender, e);
        }

        public override void SetupTooltip(PointF point, GH_TooltipDisplayEventArgs e)
        {
            base.SetupTooltip(point, e);
            e.Description = "Double click to access the drill hole chart";
        }

        // Render the object
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Objects)
            {
                //Render output grip.
                GH_CapsuleRenderEngine.RenderOutputGrip(graphics, canvas.Viewport.Zoom, OutputGrip, true);

                // Render capsule
                bool value = bValue(1);
                Rectangle button = Button();

                GH_Palette palette = GH_Palette.White;
                if (value == false) {
                    palette = GH_Palette.Black;
                }
                string Value = "Access Chart";

                GH_Capsule capsule = GH_Capsule.CreateTextCapsule(button, button, palette, Value, 0, 0);
                capsule.Render(graphics, Selected, Owner.Locked, false);
                capsule.Dispose();
            }
        }
    }
}