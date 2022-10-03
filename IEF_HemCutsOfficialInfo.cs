using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace IEF_Toolbox
{
    public class IEF_HemCutsOfficialInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "IEFHemCutsOfficial";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("e37c5d99-deb7-4a8e-bf2e-e03559aa48c6");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
