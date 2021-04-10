using System;
using System.Net;
using System.Drawing;
using System.Collections.Generic;


using Grasshopper.Kernel;
using Rhino.Geometry;
using IEF_Toolbox.Class;
using Grasshopper.Kernel.Special;

namespace IEF_Toolbox
{
    public class DrillHoleSize : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DrillHoleSize class.
        /// </summary>
        /// 

        GH_Document GrasshopperDocument;
        IGH_Component Component;


        public DrillHoleSize()
          : base("Drill Size", "DS",
              "Output the drill hole size based on the input information. Attach Value List component to '[]' to access the preset data. Hole size data from LittleMachineShop.com",
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
            pManager.AddBooleanParameter("Instantiate", "Instantiate", "Link a button and click to get all the value list", GH_ParamAccess.item);
            pManager.AddTextParameter("[]ScrewSize", "[]S_o", "(String)Select the screw size number", GH_ParamAccess.item);
            pManager.AddNumberParameter("[]ScrewMajorDiameter", "[]S", "(Double)Select the screw size in decimal", GH_ParamAccess.item, 0.0);
            pManager.AddIntegerParameter("[]ThreadPerInch", "[]TPI_o", "(Int)Select thread per inch if info available. Default to the smallest value under its screw size", GH_ParamAccess.item, 0);
            pManager.AddTextParameter("[]DrillType", "[]DrillType", "(String)Select the drill type", GH_ParamAccess.item);
            pManager.AddTextParameter("[]FitType", "[]FitType", "(String)Input the proper fit parameter", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("HoleDiameter", "HoleDiameter", "Output the proper hole size", GH_ParamAccess.item);
            pManager.AddTextParameter("DrillSize", "DrillSize", "Size of drillbit used on the machine", GH_ParamAccess.item);
            pManager.AddNumberParameter("ScrewMinorDiameter", "ScrewMinorDiameter","The minimum diameter on the fastener", GH_ParamAccess.item);
            pManager.AddGenericParameter("DrillObject", "D", "Drill Object. Use Drill Deconstructor to get fields", GH_ParamAccess.item);
        }




        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        { 
            // instantiate the TapClearanceDrillHole object
            TapClearanceDrillHole drillHole = new TapClearanceDrillHole();
            //////////////////////////////////////////////////////////////////////////////////////////////
            /// Initializing the value list keys
            /// Load the TapDrillChart and save the VL value for 
            /// _screwsize#, _screwsizedecimal, _threadperinch, _Drill Type, _Fit Type
            TapClearanceDrillChart TapDrillChart = new TapClearanceDrillChart(); //import the TapDrillChart
            // get the initial values from SizeChart to guide the value list value (0 & 1)
            List<string> iSizeNums = new List<string>();
            List<double> iSizeDecims = new List<double>();  
            for (int i = 0; i < TapDrillChart.Chart_ScrewSize.Length; i++) { 
                Tuple <string,double> sSize = TapDrillChart.Chart_ScrewSize[i];
                string sSizeNum = sSize.Item1;
                double sSizeDecim = sSize.Item2;
                iSizeNums.Add(sSizeNum);
                iSizeDecims.Add(sSizeDecim);
            }
            // get value of sorted thread per inch value (2)
            List<int> iThreadValueInt = new List<int>();
            for (int i = 0; i < TapDrillChart.ThreadPerIn.Count; i++) {
                List<int> sublist = TapDrillChart.ThreadPerIn[i];
                for (int j = 0; j < sublist.Count; j++) {
                    if (iThreadValueInt.Contains(sublist[j]))
                        continue;
                    else
                    {
                        int value = sublist[j];
                        iThreadValueInt.Add(value);
                    }
                }
            }
            iThreadValueInt.Sort();
            List<string> iThreadValueString = new List<string>();
            for (int i = 0; i<iThreadValueInt.Count; i++ )
            {
                iThreadValueString.Add(iThreadValueInt[i].ToString());

            }
            // setup the value in Drill Type (3)
            List<string> idrillTypes = new List<string>();
            idrillTypes.Add ("Tapping Hole");
            idrillTypes.Add ("Clearance Hole");
            idrillTypes.Add ("Pilot Hole");
            // setup the value in Fit Type (4)
            List<List<string>> ifitTypes = new List<List<string>>();
            // create separate list
            List<string> tapFit = new List<string>();
            tapFit.Add("Alum, Brass & Plastics");
            tapFit.Add("Steel, Stainless & Iron");
            List<string> clearFit = new List<string>();
            clearFit.Add("Close Fit");
            clearFit.Add( "Free Fit");
            List<string> pilot = new List<string>();
            pilot.Add("Pilot Hole");

            ifitTypes.Add(tapFit);
            ifitTypes.Add(clearFit);
            ifitTypes.Add(pilot);
            //////////////////////////////////////////////////////////////////////////////////////////////
            ///vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
            /// Set up the value list guide
            /// Maybe find solution in this post
            /// https://discourse.mcneel.com/t/how-to-program-a-c-component-in-visual-studio-with-autolist-capability/99486/3
            //prep the value list instantiation
            bool buttonClicked = new bool();
            DA.GetData(0, ref buttonClicked);

            /// If the button clicked, generate the value list and auto attach them to the slider
            if (buttonClicked)
            {
                var screwsizevaluelist1 = new Grasshopper.Kernel.Special.GH_ValueList();
                var screwsizevaluelist2 = new Grasshopper.Kernel.Special.GH_ValueList();
                var tpi = new Grasshopper.Kernel.Special.GH_ValueList();
                var drilltype = new Grasshopper.Kernel.Special.GH_ValueList();
                var fittype = new Grasshopper.Kernel.Special.GH_ValueList();

                //customize the value list


            }
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            //////////////////////////////////////////////////////////////////////////////////////////////
            /// Get the GH input data
            string SSizeNum = string.Empty;
            double SSizeDecimal = double.NaN;
            int ThreadPerIn = 0;
            string DrillType = string.Empty;
            string FitType = string.Empty;
            bool success1 = DA.GetData(1, ref SSizeNum);
            bool success2 = DA.GetData(2, ref SSizeDecimal);
            bool success3 = DA.GetData(3, ref ThreadPerIn);
            bool success4 = DA.GetData(4, ref DrillType);
            bool success5 = DA.GetData(5, ref FitType);

            /// MAIN METHOD
            /// initialize the output value
            double HoleDiameterOutput = double.NaN;
            string DrillSizeOutput = string.Empty;
            double MinorDiameterOutput = double.NaN;
            /// get the row index based on the screw size input
            /// 2 input types are supported: [string: SSizeNum] represents fastener's labelled size, and [double: SSizeDecimal] represents the major diameter of the fastener
            /// out of the two inputs, either one should be able to get the corresponding index value.
            /// when 2 input are available, [double: SSizeDecimal] should be prioritized
            /// (if further features can be introduced to the battery's UI, the [double: SSizeDecimal] will be the major input and [string: SSizeNum] will only be a checking value displayed)
            int index = 0;
            if (success2 == true && success1 == false) {
                for (int i = 0; i < iSizeDecims.Count; i++) {
                    if (SSizeDecimal == iSizeDecims[i]) {
                        index = i;
                        break;
                    }
                }
            }
            else if (success2 == false && success1 == true) {
                for (int i = 0; i < iSizeNums.Count; i++) {
                    if (SSizeNum == iSizeNums[i]) {
                        index = i;
                        break;
                    }
                }
            }
            else if (success2 == true & success1 == true) {
                int i = 0;
                int j = 0;

                foreach (string a in iSizeNums) {
                    if (SSizeNum == a) { break; }
                    i += 1;
                }
                foreach (double b in iSizeDecims) { 
                    if (SSizeDecimal == b) { break; }
                    j += 1;
                }
                if (i == j) { index = i; }
                else { index = 0;
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Screw Size inputs does not match. Please unplug the ScrewSize input"); 
                }
            }
            else {
                index = 0;
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Must input the Screw Diameter information");
            }

            /// find the subindex based on thread per inch
            /// Thread per inch is optional, the default is to select the smallest size for thread cutting to ensure the tightest fit
            int subindex = 0;
            if (success3) {
                for (int i = 0; i < TapDrillChart.ThreadPerIn[index].Count; i++) {
                    if (ThreadPerIn == TapDrillChart.ThreadPerIn[index][i]){
                        subindex = i;
                        break;
                    }
                }
            }
            else { subindex = 0; }

            /// select drill size based on drill condition
            if (success4 && success5)
            {
                if (DrillType == idrillTypes[0])
                {
                    drillHole.IsTapDrill = true;
                    drillHole.BaseMaterial = FitType;
                    if (FitType == tapFit[0])
                    {
                        HoleDiameterOutput = TapDrillChart.TapDrillSizeAlum_Decimal[index][subindex];
                        DrillSizeOutput = TapDrillChart.TapDrillSizeAlum[index][subindex];
                        MinorDiameterOutput = TapDrillChart.MinorDiameters[index][subindex];

                        drillHole.IsForAlumBrassPlastic = true;
                    }
                    else if (FitType == tapFit[1])
                    {
                        HoleDiameterOutput = TapDrillChart.TapDrillSizeSteel_Decimal[index][subindex];
                        DrillSizeOutput = TapDrillChart.TapDrillSizeSteel[index][subindex];
                        MinorDiameterOutput = TapDrillChart.MinorDiameters[index][subindex];

                        drillHole.IsForSteelStainlessIron = true;
                    }
                }
                else if (DrillType == idrillTypes[1])
                {
                    drillHole.IsClearanceDrill = true;
                    drillHole.FitType = FitType;
                    if (FitType == clearFit[0])
                    {
                        HoleDiameterOutput = TapDrillChart.ClearanceDrillSizeCloseFit_Decimal[index];
                        DrillSizeOutput = TapDrillChart.ClearanceDrillSizeCloseFit[index];
                        MinorDiameterOutput = TapDrillChart.MinorDiameters[index][subindex];

                        drillHole.IsCloseFit = true;
                    }
                    else if (FitType == clearFit[1])
                    {
                        HoleDiameterOutput = TapDrillChart.ClearanceDrillSizeFreeFit_Decimal[index];
                        DrillSizeOutput = TapDrillChart.ClearanceDrillSizeFreeFit[index];
                        MinorDiameterOutput = TapDrillChart.MinorDiameters[index][subindex];

                        drillHole.IsFreeFit = true;
                    }
                }
                else if (DrillType == idrillTypes[2] || FitType == pilot[0])
                {
                    HoleDiameterOutput = 0.1000;
                    DrillSizeOutput = "Any small drill bit with diameter smaller than 0.1000 inch";
                    MinorDiameterOutput = TapDrillChart.MinorDiameters[index][subindex];

                    drillHole.IsPilotHole = true;
                }

            }
            else if (!success4) {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Must input the proper value for Drill Type");
            }
            else if (!success5) {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Must input the proper value for Fit Type");
            }

            // set the field for drillHole object
            drillHole.ScrewSize = SSizeNum;
            drillHole.ThreadPerInch = ThreadPerIn;
            drillHole.MinorDiameter = MinorDiameterOutput;
            drillHole.MajorDiameter = HoleDiameterOutput;
            drillHole.DrillSize = DrillSizeOutput;
            drillHole.DrillSizeDecimalEquiv = HoleDiameterOutput;
            drillHole.DrillType = DrillType;

            /// Output values
            DA.SetData(0, HoleDiameterOutput);
            DA.SetData(1, DrillSizeOutput);
            DA.SetData(2, MinorDiameterOutput);
            DA.SetData(4, drillHole);
        }


           // // Assign value to the value list
           // screwsizevaluelist1.CreateAttributes();
           // if (success1 == true)
           // {
           //     screwsizevaluelist1.ListItems.Clear();
           //     AddListToVL(iSizeNums, screwsizevaluelist1);
           //     screwsizevaluelist1.SelectItem(0);
           // }
           //// SSizeNum = DA.GetData(0);


           // screwsizevaluelist2.CreateAttributes();
           // if (success2 == true)
           // {
           //     screwsizevaluelist2.ListItems.Clear();
           //     AddListToVL(iSizeDecims, screwsizevaluelist2);
           //     screwsizevaluelist2.SelectItem(0);
           // }

           // tpi.CreateAttributes();
           // if (success3 == true)
           // {
           //     tpi.ListItems.Clear();
           //     AddListToVL(iThreadValue, tpi);
           //     tpi.SelectItem(0);
           // }

           // drilltype.CreateAttributes();
           // if (success4 == true)
           // {
           //     drilltype.ListItems.Clear();
           //     AddListToVL(drillTypes, drilltype);
           //     drilltype.SelectItem(0);

           // }

            //fittype.CreateAttributes();
            //if (success4 && success5 == true)
            //{
            //    if ()
            //    {
            //        tpi.ListItems.Clear();
            //        AddListToVL(fitTypes, fittype);
            //    }
            //}



        /// <summary>
        /// Input Additional Methods
        /// </summary>
        void AddListToVL(List<string> items, Grasshopper.Kernel.Special.GH_ValueList VL) 
        { 
            for (int i = 0; i< items.Count; i++)
            {
                var value = new Grasshopper.Kernel.Special.GH_ValueListItem(items[i].ToString(), items[i].ToString());
                VL.ListItems.Add(value);
            }
        }


        void AddListToVL(List<double> items, Grasshopper.Kernel.Special.GH_ValueList VL)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var value = new Grasshopper.Kernel.Special.GH_ValueListItem(items[i].ToString(), items[i].ToString());
                VL.ListItems.Add(value);
            }
        }

        void AddListToVL(List<int> items, Grasshopper.Kernel.Special.GH_ValueList VL)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var value = new Grasshopper.Kernel.Special.GH_ValueListItem(items[i].ToString(), items[i].ToString());
                VL.ListItems.Add(value);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void AssignVLValues( Grasshopper.Kernel.Special.GH_ValueList VL, string Name, string Nickname, int indexInComponent, List<string> ValueListContent)
        {
            var vallist = VL;
            vallist.CreateAttributes();
            vallist.Name = Name;
            vallist.NickName = Nickname;
            vallist.ListMode = Grasshopper.Kernel.Special.GH_ValueListMode.DropDown;

            int inputcount = this.Component.Params.Input[indexInComponent].SourceCount;
            vallist.Attributes.Pivot = new PointF((float)this.Component.Attributes.DocObject.Attributes.Bounds.Left - vallist.Attributes.Bounds.Width - 30, (float)this.Component.Params.Input[1].Attributes.Bounds.Y + inputcount * 30);

            vallist.ListItems.Clear();

            for (int i = 0; i < ValueListContent.Count; i++)
            {
                vallist.ListItems.Add(new Grasshopper.Kernel.Special.GH_ValueListItem(ValueListContent[i], i.ToString()));
            }
            vallist.Description = ValueListContent.Count.ToString();

            GrasshopperDocument.AddObject(vallist, false);

            this.Component.Params.Input[indexInComponent].AddSource(vallist);
            vallist.ExpireSolution(true);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void createValueList(string itemName, List<string> valueList, object sender, System.EventArgs e)
        {
            GH_DocumentIO docIO = new GH_DocumentIO();
            docIO.Document = new GH_Document();

            //initialize object
            GH_ValueList vl = new GH_ValueList();
            //clear default contents
            vl.ListItems.Clear();
            //add all the accent colors as both "Keys" and values
            foreach (string item in valueList)
            {
                GH_ValueListItem vi = new GH_ValueListItem(item, String.Format("\"{0}\"", item));
                vl.ListItems.Add(vi);
            }
            //set component nickname
            vl.NickName = itemName;
            //get active GH doc
            GH_Document doc = OnPingDocument();
            if (docIO.Document == null) return;
            // place the object
            docIO.Document.AddObject(vl, false, 1);
            //get the pivot of the "accent" param
            PointF currPivot = Params.Input[3].Attributes.Pivot;
            //set the pivot of the new object
            vl.Attributes.Pivot = new PointF(currPivot.X - 120, currPivot.Y - 11);

            docIO.Document.SelectAll();
            docIO.Document.ExpireSolution();
            docIO.Document.MutateAllIds();
            IEnumerable<IGH_DocumentObject> objs = docIO.Document.Objects;
            doc.DeselectAll();
            doc.UndoUtil.RecordAddObjectEvent("Create Value List", objs);
            doc.MergeDocument(docIO.Document);
            //doc.ScheduleSolution(10);
        }
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
            get { return new Guid("b5381ed7-73d1-46b6-9912-7e2a91bc756f"); }
        }
    }
}