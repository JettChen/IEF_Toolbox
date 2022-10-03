using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Rhino;
using Rhino.Geometry;


namespace IEF_Toolbox.Class
{
    class HemCut
    {
        /// <summary>
        /// Field
        /// </summary>
        public string HemCutID = "";
        public static string ProfileID = "";
        public double CutLeft = 0.0;
        public double CutRight = 0.0;

        /// <summary>
        /// Constructor
        /// </summary>
        public HemCut() { }
        public HemCut(string id, double left, double right)
        {
            HemCutID = id;
            CutLeft = left;
            CutRight = right;
        }

        /// <summary>
        /// Function
        /// </summary>

        public bool IsIDOccupied(string hemcutID)
        {
            bool idOccupied = new bool();

            //compare the id input with an imported id list from the hem cut list

            return idOccupied;
        }
        public string GetProfileID(string hemcutID)
        {
            
            string profileID = "";
            string[] idSegments = hemcutID.Split('.');

            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            Match result = re.Match(idSegments[1]);
            string alphaPart = result.Groups[1].Value;
            string numberPart = result.Groups[2].Value;
            
            profileID = alphaPart + "-" +numberPart;
            return profileID;
        }
        
        public void AssignID(string hemcutID)
        {
            //

        }
    }
}
