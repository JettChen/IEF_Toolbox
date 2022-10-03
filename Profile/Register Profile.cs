using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.DocObjects;
using Rhino.Geometry;
using IEF_Toolbox.Class;


namespace IEF_Toolbox.Profile
{
    public class Register_Profile : GH_Component
    {
        GH_Document GrasshopperDocument;
        IGH_Component Component;
        RhinoDoc RhinoDocument;
        IGH_Goo goo;
        /// <summary>
        /// Initializes a new instance of the Register_Profile class.
        /// </summary>
        public Register_Profile()
          : base("Register Profile", "Register",
              "If the profile curve is already aligned at (0,0,0) YZ plane and named with the Profile ID, use this component to register the profile curve and make it a Profile Object",
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
            pManager.AddCurveParameter("Profile Curves", "C", "The Profile Curves named with Profile ID", GH_ParamAccess.list);
            pManager.AddTextParameter("Profile Description", "D", "The Profile Description", GH_ParamAccess.item);
            pManager.AddTextParameter("Profile Type", "T", "The Profile Type", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Register", "R", "Set to true to bake attributes to the selected curves", GH_ParamAccess.item, false);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Profile Object", "P", "The Profile Object. Use the Deconstruct Profile component to extract information", GH_ParamAccess.item);
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
            
            //GET THE PROFILE OBJECT INPUT
            List<RhinoObject> profileCrvs = new List<RhinoObject>();
            string type = null;
            bool success1 = DA.GetDataList(0, profileCrvs);
            bool success2 = DA.GetData(1, ref type);
            if (!success1) { return; }

            string profileID = profileCrvs[0].Name;
            List<Curve> crvs = new List<Curve>();
            foreach (RhinoObject crv in profileCrvs)
            {
                if (crv.Geometry is Curve) { crvs.Add(crv.Geometry as Curve); }
            }

            // construct the profile
            FrameProfile Profile = new FrameProfile(profileID, ref crvs);
            Profile.ProfileType = type;

            DA.SetData(0, Profile);
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
            get { return new Guid("3f3c945b-b6eb-4b88-a281-dd56ce58d9f9"); }
        }
    }
}