using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Geometry;
using Rhino.FileIO;

namespace IEF_Toolbox.Class
{
    public class TapClearanceDrillHole
    {
        /// <summary>
        /// Fields
        /// </summary>
        public string ScrewSize = string.Empty;
        public int ThreadPerInch = 0;
        public double MinorDiameter = 0.0000;
        public double MajorDiameter = 0.0000;

        public string DrillType = string.Empty;
        public string BaseMaterial = string.Empty;
        public string FitType = string.Empty;

        public bool IsTapDrill = false;
        public bool IsForAlumBrassPlastic = false;
        public bool IsForSteelStainlessIron = false;

        public bool IsClearanceDrill = false;
        public bool IsCloseFit = false;
        public bool IsFreeFit = false;

        public bool IsPilotHole = false;

        public static double PilotHoleDiameter = 0.1000;

        public string DrillSize = string.Empty;
        public double DrillSizeDecimalEquiv = 0.0000;

        public Point3d Location = new Point3d();

        /// <summary>
        /// special fields in the tap&clearance drill chart
        /// </summary>




        /// <summary>
        /// Constructors
        /// </summary>
        public TapClearanceDrillHole() { }
        public TapClearanceDrillHole(string screwSize, int threadPerInch)
        {

        }



        ///Methods
        ///
        ///the goal is to store the chart sizing information in a list/tuple
        ///and then access the drilling size based on input like screwSize and Drilltype
        ///test tap/clearance/pilot
        ///

        public double GetHoleSize(string sizeNum, double sizeDecimal, int threadPerIn, string drillType, string fitType) {

            double holeSize = double.NaN;

            TapClearanceDrillChart dChart = new TapClearanceDrillChart();
            ;
            
            







            
            
            
            return holeSize;
        }


        //private bool IsScrewSizeValid(string sizeNum, double sizeDecimal) { 
        
        
        
        
       // }


    }

}
