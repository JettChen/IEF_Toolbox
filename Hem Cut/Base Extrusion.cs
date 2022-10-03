using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.DocObjects;
using Rhino.Geometry;
using IEF_Toolbox.Class;

namespace IEF_Toolbox
{
    public class BaseExtrusion : GH_Component
    {
        GH_Document GrasshopperDocument;
        IGH_Component Component;
        RhinoDoc RhinoDocument;
        /// <summary>
        /// Initializes a new instance of the Extrude_Base class.
        /// </summary>
        public BaseExtrusion()
          : base("Extrusion Base", "EBase",
              "Generate the base extrusion for hem cuts",
              "IEF Toolbox", "03_Hem Cut")
        {
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("string_Profile ID","ID", "The name of the base extrusion profile", GH_ParamAccess.item);
            pManager.AddNumberParameter("double_Cut Left", "L", "The Cut Left extrusing value", GH_ParamAccess.item);
            pManager.AddNumberParameter("double_Cut Right", "R", "The Cut Right extrusing value", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("brep_Extrusion Base", "EB", "Extrusion base for the hem cuts", GH_ParamAccess.item);
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

            string profileID = null;
            double Left = 0.0;
            double Right = 0.0;

            bool success1 = DA.GetData(0, ref profileID);
            bool success2 = DA.GetData(1, ref Left);
            bool success3 = DA.GetData(2, ref Right);

            if (!success1 || !success2 || !success3) { return; }


            ///////////////////////////////////////////////////////////////////////
            /// Reference the curves from the Profile Curve Layer
            /// 
            List<Curve> ProfileCurves = new List<Curve>();
            string profileBaseLayers = "Profile Curve";
            int layer_index = RhinoDocument.Layers.FindByFullPath(profileBaseLayers,-1);
            bool layerExist = layer_index >= 0;
            if (!layerExist)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Profile Curve layer does not exist in the document. Please reference the Profile Library");
                return;
            }
            RhinoObject[] currentCrvs = RhinoDocument.Objects.FindByLayer(profileBaseLayers);
            foreach (RhinoObject crv in currentCrvs)
            {
                GeometryBase gb = crv.Geometry;
                Curve c = gb as Curve;
                if (crv.Attributes.Name == profileID && c != null)
                {
                    ProfileCurves.Add(c);
                }
            }

            FrameProfile profile = new FrameProfile(profileID, ref ProfileCurves);

            ///////////////////////////////////////////////////////////////////////

            Brep baseExtrusion = profile.CreateBaseExtrusion(Left,Right);


            //List<Curve> crvs = profile.ProfileCrv;
            //Vector3d moveL = new Vector3d(0 - Left, 0, 0);
            //Vector3d moveR = new Vector3d(Right, 0, 0);
            //Vector3d extrude = new Vector3d(Left + Right, 0, 0);

            //Point3d ptLeft = new Point3d(0 - Left, 0, 0);
            //Point3d ptRight = new Point3d(Right, 0, 0);
            //LineCurve path = new LineCurve(ptLeft, ptRight);

            //// get inside/outside curves
            //Curve outsideCrv = profile.OutsideCrv.DuplicateCurve();
            //outsideCrv.Translate(moveL);
            //List<Curve> insideCrv = new List<Curve>();
            //foreach (Curve crv in profile.InsideCrv) {
            //    Curve cr = crv.DuplicateCurve();
            //    cr.Translate(moveL);
            //    insideCrv.Add(cr); }
            //// extrude the inside/outside curves
            //List<Brep> outsideBreps = new List<Brep>();
            //outsideBreps.Add(Extrusion.Create(outsideCrv, Left + Right, true).ToBrep());
            //List<Brep> insideBreps = new List<Brep>();
            //foreach (Curve crv in insideCrv)
            //{
            //    Brep br = Extrusion.Create(crv, Left + Right, true).ToBrep();
            //    insideBreps.Add(br);
            //}
            //double tol = RhinoDocument.ModelAbsoluteTolerance;
            //// bd the inside and outside extrusions
            //Brep[] baseGeometry = Brep.CreateBooleanDifference(outsideBreps, insideBreps, tol);


            //Surface outsideL = Surface.CreateExtrusion(outsideCrv, moveL);
            //Surface outsideR = Surface.CreateExtrusion(outsideCrv,moveR);


            DA.SetData(0, baseExtrusion);


            //List<Curve> leftCrvs = new List<Curve>();
            //List<Curve> rightCrvs = new List<Curve>();

            //foreach (Curve crv in crvs)
            //{
            //    Curve crvL = crv.DuplicateCurve();
            //    crvL.Translate(moveL);
            //    leftCrvs.Add(crvL);

            //    Curve crvR = crv.DuplicateCurve();
            //    crvR.Translate(moveR);
            //    rightCrvs.Add(crvR);
            //}



            //Brep[] baseSrf = Brep.CreatePlanarBreps(crvs);
            //BrepFace b = baseSrf[0].Faces[0];
            //PlaneSurface a = new PlaneSurface
            //Brep BaseExtrusion = 
 



            //for (int i = 0; i< crvs.Count; i++)
            //{
                
            //}

            //Curve outsideL = profile.outsideCrv.DuplicateCurve();
            //outsideL.Translate(moveL);
            //Extrusion outsideExtrusion = Extrusion.Create(outsideL, Left+Right, true);

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
            get { return new Guid("85c892a6-7fbd-4a7c-96c3-f1a6504a012c"); }
        }
    }
}