using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using Rhino.DocObjects;
using Grasshopper;
using Grasshopper.Kernel;

namespace IEF_Toolbox.Class
{
    public class ProfileSkeleton
    {

        public string ProfileID = string.Empty;
        public string ProfileType = string.Empty;

        public Rectangle3d BoundingRectangle = new Rectangle3d();
        public double Depth = 0.0000;
        public double WidthOrHeight = 0.0000;

        public List<Circle> ScrewChaseHole = new List<Circle>();
        public List<Point3d> ScrewChaseLocation = new List<Point3d>();
        public List<double> ScrewChaseDiameter = new List<double>();

        public Point3d Anchor = new Point3d();
        public Plane BasePlane = new Plane();
        public Plane TopPlane;
        public Plane BottomPlane;
        public double LeftWidth;
        public double RightWidth;


        public int VersionNumber;
        public string uniqueID;

        public double Area;
        


        public ProfileSkeleton() { }
        public ProfileSkeleton(FrameProfile profile) {
            ProfileID = profile.ProfileID;
            ProfileType = profile.ProfileType;
            VersionNumber = profile.VersionNumber;
            uniqueID = "SKT_" + profile.uniqueID;
            Anchor = profile.Anchor;
            BasePlane = profile.BasePlane;
            TopPlane = profile.TopPlane;
            BottomPlane = profile.BottomPlane;
            // Assign the boundary curve
            DefineBoundingBox(profile.OutsideCrv, BasePlane);
            // Get screw chase holes
            List<Curve> crvWithScrewChase = new List<Curve>();
            string errorMessage = null;
            ScrewChaseHolePrep(ProfileType, profile.OutsideCrv,profile.InsideCrv, out crvWithScrewChase, out errorMessage);
            if (crvWithScrewChase.Count != 0)
            {
                AnalyzeScrewChaseHole(crvWithScrewChase);
            } else { return; }
            // compute area
            CalcArea(profile);
            // calc Top Bottom Plane
            CalcTopBottomPlane(profile);
        }

        void DefineBoundingBox(Curve outsideBoundary, Plane basePlane)
        {
            var bbx = outsideBoundary.GetBoundingBox(true);
            Point3d[] bbxPts = bbx.GetCorners();
            Vector3d dir = basePlane.Normal;
            //List<Point3d> basePlanePts = new List<Point3d>();
            //foreach (Point3d pt in bbxPts) {
            //    Point3d ptPlane = new Point3d();
            //    basePlane.RemapToPlaneSpace(pt, out ptPlane);
            //    basePlanePts.Add(ptPlane);
            //}
            //basePlanePts.Sort();
            List<double> allPtY = bbxPts.Select(pt => pt.Y).ToList();
            List<double> allPtZ = bbxPts.Select(pt => pt.Z).ToList();
            Rectangle3d bCrv = new Rectangle3d(basePlane, new Point3d(0,allPtY.Min(),allPtZ.Min()), new Point3d(0,allPtY.Max(),allPtZ.Max()));
            double d = Math.Abs(allPtZ.Max()-allPtZ.Min());
            double w = Math.Abs(allPtY.Max() - allPtY.Min());

            BoundingRectangle = bCrv;
            Depth = d;
            WidthOrHeight = w;
        }

        void AnalyzeScrewChaseHole(List<Curve> selProfileCrvs)
        {
            List<Circle> cir = new List<Circle>();
            List<Point3d> centers = new List<Point3d>();
            List<double> diameters = new List<double>();
            foreach (Curve crv in selProfileCrvs)
            {
                List<Curve> segmList = new List<Curve>();
                MakeCurveSegments(ref segmList, crv, true);
                List<Curve> arcList = new List<Curve>();
                arcList = segmList.Where(c => !c.IsLinear()).ToList();

                //foreach (Curve c in segmList)
                //{
                //    ArcCurve a = c as ArcCurve;
                //    if (a != null) { arcList.Add(a); }
                //}

                foreach (Curve a in arcList)
                {
                    double r = double.NaN;
                    double angle = double.NaN;
                    // Note the bool here is to "not linear" rather than "is arc"
                    if (!a.IsLinear())
                    {
                        double d = new Line(a.PointAtStart, a.PointAtEnd).Length;
                        double l = new Line((a.PointAtStart+a.PointAtEnd)/2,a.PointAtLength(a.GetLength()/2)).Length;
                        r = (l * l + d * d / 4) / (2 * l);
                        angle = a.GetLength() / (2 * Math.PI * r) * 360;
                        // Screw chase must have angle degree > 240
                        if (angle >= 240)
                        {
                            Point3d ps = a.PointAtStart;
                            Point3d pe = a.PointAtEnd;
                            Vector3d ts = a.TangentAtStart;
                            Vector3d te = a.TangentAtEnd;
                            ts.Rotate(Math.PI / 2, BasePlane.Normal);
                            te.Rotate(Math.PI / 2, BasePlane.Normal);

                            var line1 = new Line(ps, ts*100);
                            var line2 = new Line(pe, te*100);

                            double iS;
                            double iE;
                            var intersectPt = Rhino.Geometry.Intersect.Intersection.LineLine(line1, line2, out iS, out iE);
                            Point3d center = line1.PointAt(iS);

                            centers.Add(center);
                            diameters.Add(r * 2);
                        }
                    }
                }
                // generate the circle based on the center/radius pair
                for (int i = 0; i < centers.Count; i++) {
                    Circle circle = new Circle(BasePlane, centers[i], diameters[i] / 2);
                    cir.Add(circle);
                }
                // output fields
                ScrewChaseHole = cir;
                ScrewChaseLocation = centers;
                ScrewChaseDiameter = diameters;
            }
        }

        void ScrewChaseHolePrep(string profileType, Curve outsideCrv, List<Curve> insideCrv, out List<Curve> crvWithScrewChase, out string ifErrorMessage) {
            string[] onlyOutside = { "SHEAR BLOCK" , "CORNER MULLION SHEAR BLOCK"} ;
            string[] onlyInside = { "MULLION","CORNER MULLION", "CHICKEN HEAD" } ;
            string[] both = { "HORIZONTAL RECEPTOR"};

            int sel = int.MinValue;

            if (onlyOutside.Contains(profileType)) { sel = 1; }
            else if (onlyInside.Contains(profileType)) { sel = 2; }
            else if (both.Contains(profileType)) { sel = 3; }
            else { sel = 0; }

            List<Curve> output = new List<Curve>();
            string message = null;
            int prep = sel;

            switch (prep)
            {
                case 0:
                    message = "Profile Type doesn't exist in the code space. Please check typo or contact Mingjia Chen for help";
                    break;
                case 1:
                    output.Add(outsideCrv);
                    break;
                case 2:
                    foreach (Curve c in insideCrv) { output.Add(c); }
                    break;
                case 3:
                    output.Add(outsideCrv);
                    foreach (Curve c in insideCrv) { output.Add(c); }
                    break;
            }

            crvWithScrewChase = output;
            ifErrorMessage = message;
        }
        public static bool MakeCurveSegments(ref List<Curve> cList, Curve crv, bool recursive)
        {


            PolyCurve polycurve = crv as PolyCurve;
            if (polycurve != null)
            {
                if (recursive) polycurve.RemoveNesting();
                Curve[] segments = polycurve.Explode();
                if (segments == null || segments.Length == 0) return false;
                if (recursive) foreach (Curve segment in segments) MakeCurveSegments(ref cList, segment, recursive);
                else foreach (Curve segment in segments) cList.Add(segment.DuplicateShallow() as Curve);
                return true;
            }

            PolylineCurve polyline = crv as PolylineCurve;
            if (polyline != null)
            {
                if (recursive)
                {
                    for (int i = 0; i < (polyline.PointCount - 1); i++) cList.Add(new LineCurve(polyline.Point(i), polyline.Point(i + 1)));
                }
                else cList.Add(polyline.DuplicateCurve());
                return true;
            }

            Polyline p;
            if (crv.TryGetPolyline(out p))
            {
                if (recursive)
                {
                    for (int i = 0; i < (p.Count - 1); i++) cList.Add(new LineCurve(p[i], p[i + 1]));
                }
                else cList.Add(new PolylineCurve(p));
                return true;
            }

            LineCurve line = crv as LineCurve;
            if (line != null) { cList.Add(line.DuplicateCurve()); return true; }

            ArcCurve arc = crv as ArcCurve;
            if (arc != null) { cList.Add(arc.DuplicateCurve()); return true; }

            NurbsCurve nurbs = crv.ToNurbsCurve();
            if (nurbs == null) return false;

            double t0 = nurbs.Domain.Min; double t1 = nurbs.Domain.Max; double t;
            int cListCount = cList.Count;

            do
            {
                if (!nurbs.GetNextDiscontinuity(Continuity.C1_locus_continuous, t0, t1, out t)) break;

                Interval trim = new Interval(t0, t);
                if (trim.Length < 1e-10) { t0 = t; continue; }

                Curve nDC = nurbs.DuplicateCurve();
                nDC = nDC.Trim(trim);
                if (nDC.IsValid) cList.Add(nDC);
                t0 = t;
            }
            while (true);

            if (cList.Count == cListCount) cList.Add(nurbs);
            return true;
        }

        public void CalcArea(FrameProfile profile) {
            double outsideArea = AreaMassProperties.Compute(profile.OutsideCrv).Area;
            double insideArea = AreaMassProperties.Compute(profile.InsideCrv).Area;
            Area = outsideArea - insideArea;
        }
        public void CalcTopBottomPlane(FrameProfile profile) {
            var bbx = profile.OutsideCrv.GetBoundingBox(true);
            Point3d[] bbxPts = bbx.GetCorners();
            double[] PtsY = bbxPts.Select(pt => pt.Y).ToArray();
            double YMax = PtsY.Max();
            double YMin = PtsY.Min();
            LeftWidth = YMin;
            RightWidth = YMax;

            TopPlane = new Plane(new Point3d(0, YMax, 0), new Vector3d(1, 0, 0), new Vector3d(0, 0, 1));
            BottomPlane = new Plane(new Point3d(0, YMin, 0), new Vector3d(1, 0, 0), new Vector3d(0, 0, 1));
        }
    }
}
