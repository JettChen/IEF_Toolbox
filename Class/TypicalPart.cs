using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Geometry;


namespace IEF_Toolbox.Class

{
    class TypicalPart
        ///sub-class: SleeveBlock, SplicePlate, AlignmentBlade, Angle, 
    {
        public string PartNumber = string.Empty;
        public string ProfileID = string.Empty;
        public List<Curve> PartProfile = new List<Curve>();
        public double MaterialLength = 0.0;

        /// <summary>
        /// constructors
        /// </summary>
        public TypicalPart() { }






    }
}
