using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEF_Toolbox.Class

{
    class ChickenHead : FrameProfile
    {
        


        /// <summary>
        /// Constructors
        /// </summary>
        public ChickenHead() {




            ResetProfileType();
        }

        /// <summary>
        /// Methods
        /// </summary>
        public override void ResetProfileType() {
            ProfileType = "Chicken Head";
        }
    }
}
