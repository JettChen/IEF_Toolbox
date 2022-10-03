using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Excel = Microsoft.Office.Interop.Excel;

namespace IEF_Toolbox.Class
{
    class Library
    {
        public string FilePath = null;
        public string WorkSheet = null;
        public List<string> Title = new List<string>();
        public List<List<string>> Values = new List<List<string>>();
        public Dictionary<string,List<string>> keyValuePairs = new System.Collections.Generic.Dictionary<string,List<string>>();

        public Library(string filePath, string worksheet)
        {
            //Excel.Application xlApp = new Excel.Application();
            //Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filePath);
            //Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            //Excel.Range xlRange = xlWorksheet.UsedRange;


        }
    }
}
