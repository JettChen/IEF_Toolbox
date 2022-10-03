using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEF_Toolbox.Class
{
    class Bake
    {
        public int BakeVersion;
        public string bakeTime
        {
            get { return DateTime.UtcNow.AddHours(-4).ToLongTimeString(); }
        }


        public Bake() { BakeVersion += 1; }

        public void bake() { BakeVersion += 1; }
    }
}
