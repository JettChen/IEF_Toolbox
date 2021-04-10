using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects;
using IEF_Toolbox.Class;


namespace IEF_Toolbox.Profile
{
    public class Acquire_Profile : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Acquire_Die_Profile class.
        /// </summary>
        /// 
        GH_Document GrasshopperDocument;
        IGH_Component Component;
        RhinoDoc RhinoDocument;
        public Acquire_Profile()
          : base("Acquire Profile", "Acquire",
              "Use the Profile ID to acquire the latest die profile in the Profile Curve Layer",
              "IEF Toolbox", "02_Profile")
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
            pManager.AddTextParameter("Profile ID","P","The Profile ID",GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Profile Object", "P", "The profile object package, Use Deconstruct Profile Object to extract information", GH_ParamAccess.item);
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

            string iProfileID = null;
            bool success1 = DA.GetData(0, ref iProfileID);
            if(!success1) { return; }

            // get curve geometries in the Profile Curve Layer
            List<Curve> ProfileCurves = new List<Curve>();
            string profileBaseLayers = "Profile Curve";
            int layer_index = RhinoDocument.Layers.FindByFullPath(profileBaseLayers, -1);
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
                if (crv.Attributes.Name == iProfileID && c != null)
                {
                    ProfileCurves.Add(c);
                }
            }

            //Construct the profile Object
            FrameProfile profile = new FrameProfile(iProfileID, ref ProfileCurves);

            DA.SetData(0, profile);
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
            get { return new Guid("b4e46819-63bf-4336-afe7-eefeafd06b84"); }
        }
    }
}