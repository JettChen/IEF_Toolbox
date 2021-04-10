using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Geometry;
using Rhino.DocObjects;
using Grasshopper;
using Grasshopper.Kernel;


namespace IEF_Toolbox.Class

{
    public class FrameProfile
        ///subclass: ChickenHead, HorizontalReceptor, VerticalBlade, VerticalReceiver, BoxMullion, CornerMullion, FlatPerimeter, ShearBlock
    {
        /// <summary>
        /// field
        /// </summary>
        public string ProfileID = string.Empty;
        public string ProfileType = string.Empty;
        public List<Curve> ProfileCrv = new List<Curve>();
        public Point3d Anchor = new Point3d();
        public Plane BasePlane = new Plane();

        public Curve OutsideCrv = null;
        public List<Curve> InsideCrv = new List<Curve>();

        public int VersionNumber;
        public string uniqueID;


        ///constructor
        ///

        public FrameProfile() { }
        public FrameProfile(string id, ref List<Curve> profileCrv, Point3d anchor, Plane bplane) {
            ProfileID = id;
            ProfileCrv = profileCrv;
            Anchor = anchor;
            BasePlane = bplane;
            SetProfileType();
            SortInsideOutside();
        }

        public FrameProfile(string id, ref List<Curve> profileCrv, Point3d anchor)
        {
            Point3d orig = Point3d.Origin;
            Vector3d planeX = new Vector3d(0.0, 0.0, -1.0);
            Vector3d planeY = new Vector3d(0.0, 1.0, 0.0);
            Plane defPlane = new Plane(orig, planeX, planeY);

            ProfileID = id;
            ProfileCrv = profileCrv;
            Anchor = anchor;
            BasePlane = defPlane;
            SetProfileType();
            SortInsideOutside();
        }

        public FrameProfile(string id, ref List<Curve> profileCrv)
        {
            Point3d orig = Point3d.Origin;
            Vector3d planeX = new Vector3d(0.0, 0.0, -1.0);
            Vector3d planeY = new Vector3d(0.0, 1.0, 0.0);
            Plane defPlane = new Plane(orig, planeX, planeY);

            ProfileID = id;
            ProfileCrv = profileCrv;
            Anchor = orig;
            BasePlane = defPlane;
            SetProfileType();
            SortInsideOutside();
        }

        public FrameProfile(string id)
        {
            Point3d orig = Point3d.Origin;
            Vector3d planeX = new Vector3d(0.0, 0.0, -1.0);
            Vector3d planeY = new Vector3d(0.0, 1.0, 0.0);
            Plane defPlane = new Plane(orig, planeX, planeY);

            //need a import module to grab the latest version of profileCrv from the profile library

            ProfileID = id;
            ProfileCrv = GetProfileCurves(id,Rhino.RhinoDoc.ActiveDoc);
            Anchor = orig;
            BasePlane = defPlane;
            SetProfileType();
            SortInsideOutside();
        }


        /// <summary>
        /// Methods
        /// </summary>

        public void SortInsideOutside() {
            List<Curve> SortedCrvs = ProfileCrv.OrderBy(i => i.GetBoundingBox(false).Area).ToList();
            SortedCrvs.Reverse();
            if (SortedCrvs.Count > 0)
            {
                OutsideCrv = SortedCrvs[0];
                if (SortedCrvs.Count >1)
                for (int i = 1; i < SortedCrvs.Count; i++)
                {
                    InsideCrv.Add(SortedCrvs[i]);
                }
            }
            else { return; }
        }

        public virtual void SetProfileType() {
            ProfileType = string.Empty;
        }

        public void UpdateChanges() { 
        
        }

        public Brep CreateBaseExtrusion(double CutLeft, double CutRight) {

            Vector3d moveL = new Vector3d(-CutLeft, 0, 0);

            List<Curve> moveLCrv = new List<Curve>(ProfileCrv);
            foreach (Curve crv in moveLCrv) { crv.Translate(moveL); }

            Point3d Left = new Point3d(-CutLeft, 0, 0);
            Point3d Right = new Point3d(CutRight, 0, 0);

            Curve extrusionPath = new LineCurve(Left,Right);
            Brep[] baseSrf = Brep.CreatePlanarBreps(moveLCrv, 0.001);
            BrepFace brepFace = baseSrf[0].Faces[0];

            Brep baseExtrusion = brepFace.CreateExtrusion(extrusionPath, true);

            return baseExtrusion;
        }

        public List<Curve> GetProfileCurves(string ProfileID, RhinoDoc RhinoDocument)
        {
            List<Curve> ProfileCurves = new List<Curve>();
            string profileBaseLayers = "Profile Curve";
            int layer_index = RhinoDocument.Layers.FindByFullPath(profileBaseLayers, -1);
            bool layerExist = layer_index >= 0;
            if (!layerExist)
            {
                return null;
            }
            RhinoObject[] currentCrvs = RhinoDocument.Objects.FindByLayer(profileBaseLayers);
            foreach (RhinoObject crv in currentCrvs)
            {
                GeometryBase gb = crv.Geometry;
                Curve c = gb as Curve;
                if (crv.Attributes.Name == ProfileID && c != null)
                {
                    ProfileCurves.Add(c);
                }
            }
            return ProfileCurves;
        }
    }
}
