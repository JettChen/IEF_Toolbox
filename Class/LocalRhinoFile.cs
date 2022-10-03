using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino;
using Grasshopper;
using Grasshopper.Kernel;


namespace IEF_Toolbox.Class
{
    public class LocalRhinoFile
    {
        public string FileName = null;
        public string Location = null;
        public int Version = 0;
        public string ModifiedTime = null;

        public LocalRhinoFile() { }
        public LocalRhinoFile(string fileName, string location) { 
        }

    }
}
