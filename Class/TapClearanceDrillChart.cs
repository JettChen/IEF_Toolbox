using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEF_Toolbox.Class

{
    public class TapClearanceDrillChart : TapClearanceDrillHole
    {
        public Tuple<string, double>[] Chart_ScrewSize = { new Tuple<string, double> ("0",0.0) };
        public List<List<int>> ThreadPerIn = new List<List<int>>();
        public List<List<double>> MinorDiameters = new List<List<double>>();
        public List<List<string>> TapDrillSizeAlum = new List<List<string>>();
        public List<List<double>> TapDrillSizeAlum_Decimal = new List<List<double>>();
        public List<List<string>> TapDrillSizeSteel = new List<List<string>>();
        public List<List<double>> TapDrillSizeSteel_Decimal = new List<List<double>>();
        public List<string> ClearanceDrillSizeCloseFit = new List<string>();
        public List<double> ClearanceDrillSizeCloseFit_Decimal = new List<double>();
        public List<string> ClearanceDrillSizeFreeFit = new List<string>();
        public List<double> ClearanceDrillSizeFreeFit_Decimal = new List<double>();


        public TapClearanceDrillChart()
        {
            /// Setup string, double import
            Tuple<string, double>[] chart_ScrewSize = {
            new Tuple<string, double> ("0", 0.0600),
            new Tuple<string, double> ("1", 0.0730),
            new Tuple<string, double> ("2", 0.0860),
            new Tuple<string, double> ("3", 0.0990),
            new Tuple<string, double> ("4", 0.1120),
            new Tuple<string, double> ("5", 0.125),
            new Tuple<string, double> ("6", 0.138),
            new Tuple<string, double> ("8", 0.1640),
            new Tuple<string, double> ("10", 0.1900),
            new Tuple<string, double> ("12", 0.2160),
            new Tuple<string, double> ("1/4", 0.2500),
            new Tuple<string, double> ("5/16", 0.3125),
            new Tuple<string, double> ("3/8", 0.3750),
            new Tuple<string, double> ("7/16", 0.4375),
            new Tuple<string, double> ("1/2", 0.5000),
            new Tuple<string, double> ("9/16", 0.5625),
            new Tuple<string, double> ("5/8", 0.6250),
            new Tuple<string, double> ("11/16", 0.6875),
            new Tuple<string, double> ("3/4", 0.7500),
            new Tuple<string, double> ("13/16", 0.8125),
            new Tuple<string, double> ("7/8", 0.8750),
            new Tuple<string, double> ("15/16", 0.9375),
            new Tuple<string, double> ("1", 1.0000)
        };


            ///
            /// Setup the Thread per Inch
            ///
            List<List<int>> threadPerIn = new List<List<int>>();
           
            threadPerIn.Add(new List<int> { 80 });
            threadPerIn.Add(new List<int> { 64, 72 });
            threadPerIn.Add(new List<int> { 56, 64 });
            threadPerIn.Add(new List<int> { 48, 56 });
            threadPerIn.Add(new List<int> { 40, 48 });
            threadPerIn.Add(new List<int> { 40, 44 });
            threadPerIn.Add(new List<int> { 32, 40 });
            threadPerIn.Add(new List<int> { 32, 36 });
            threadPerIn.Add(new List<int> { 24, 32 });
            threadPerIn.Add(new List<int> { 24, 28, 32 });
            threadPerIn.Add(new List<int> { 20, 28, 32 });
            threadPerIn.Add(new List<int> { 18, 24, 32 });
            threadPerIn.Add(new List<int> { 16, 24, 32 });
            threadPerIn.Add(new List<int> { 14, 20, 28 });
            threadPerIn.Add(new List<int> { 13, 20, 28 });
            threadPerIn.Add(new List<int> { 12, 18, 24 });
            threadPerIn.Add(new List<int> { 11, 18, 24 });
            threadPerIn.Add(new List<int> { 24 });
            threadPerIn.Add(new List<int> { 10, 16, 20 });
            threadPerIn.Add(new List<int> { 20 });
            threadPerIn.Add(new List<int> { 9, 14, 20 });
            threadPerIn.Add(new List<int> { 20 });
            threadPerIn.Add(new List<int> { 8, 12,20 });

            ///
            /// Setup the minorDiameter
            ///

            List<List<double>> minorDiameter = new List<List<double>>();

            minorDiameter.Add(new List<double> { 0.0447 });
            minorDiameter.Add(new List<double> { 0.0538, 0.0560 });
            minorDiameter.Add(new List<double> { 0.0641, 0.0668 });
            minorDiameter.Add(new List<double> { 0.0734, 0.0771 });
            minorDiameter.Add(new List<double> { 0.0813, 0.0864 });
            minorDiameter.Add(new List<double> { 0.0943, 0.0971 });
            minorDiameter.Add(new List<double> { 0.0997, 0.1073 });
            minorDiameter.Add(new List<double> { 0.1257, 0.1299 });
            minorDiameter.Add(new List<double> { 0.1389, 0.1517 });
            minorDiameter.Add(new List<double> { 0.1649, 0.1722, 0.1777 });
            minorDiameter.Add(new List<double> { 0.1887, 0.2062, 0.2117 });
            minorDiameter.Add(new List<double> { 0.2443, 0.2614, 0.2742 });
            minorDiameter.Add(new List<double> { 0.2983, 0.3239, 0.3367 });
            minorDiameter.Add(new List<double> { 0.3499, 0.3762, 0.3937 });
            minorDiameter.Add(new List<double> { 0.4056, 0.4387, 0.4562 });
            minorDiameter.Add(new List<double> { 0.4603, 0.4943, 0.5114 });
            minorDiameter.Add(new List<double> { 0.5135, 0.5568, 0.5739 });
            minorDiameter.Add(new List<double> { 0.6364 });
            minorDiameter.Add(new List<double> { 0.6273, 0.6733, 0.6887 });
            minorDiameter.Add(new List<double> { 0.7512 });
            minorDiameter.Add(new List<double> { 0.7387, 0.7874, 0.8137 });
            minorDiameter.Add(new List<double> { 0.8762 });
            minorDiameter.Add(new List<double> { 0.8466, 0.8978, 0.9387 });

            
            /// Setup the tapDrillSizeAlum
            ///

            List<List<string>> tapDrillSizeAlum = new List<List<string>>();
            
            tapDrillSizeAlum.Add(new List<string> { "3/64" });
            tapDrillSizeAlum.Add(new List<string> { "53", "53" });
            tapDrillSizeAlum.Add(new List<string> { "50", "50" });
            tapDrillSizeAlum.Add(new List<string> { "47", "45" });
            tapDrillSizeAlum.Add(new List<string> { "43", "42" });
            tapDrillSizeAlum.Add(new List<string> { "38", "37" });
            tapDrillSizeAlum.Add(new List<string> { "36", "33" });
            tapDrillSizeAlum.Add(new List<string> { "29", "29" });
            tapDrillSizeAlum.Add(new List<string> { "25", "21" });
            tapDrillSizeAlum.Add(new List<string> { "16", "14", "13" });
            tapDrillSizeAlum.Add(new List<string> { "7", "3", "7/32" });
            tapDrillSizeAlum.Add(new List<string> { "F", "I", "9/32" });
            tapDrillSizeAlum.Add(new List<string> { "5/16", "Q", "11/32" });
            tapDrillSizeAlum.Add(new List<string> { "U", "25/64", "Y" });
            tapDrillSizeAlum.Add(new List<string> { "27/64", "29/64", "15/32" });
            tapDrillSizeAlum.Add(new List<string> { "31/64", "33/64", "33/64" });
            tapDrillSizeAlum.Add(new List<string> { "17/32", "37/64", "37/64" });
            tapDrillSizeAlum.Add(new List<string> { "41/64" });
            tapDrillSizeAlum.Add(new List<string> { "21/32", "11/16", "45/64" });
            tapDrillSizeAlum.Add(new List<string> { "49/64"});
            tapDrillSizeAlum.Add(new List<string> { "49/64", "13/16", "53/64" });
            tapDrillSizeAlum.Add(new List<string> { "57/64" });
            tapDrillSizeAlum.Add(new List<string> { "7/8", "15/16", "61/64" });


            List<List<double>> tapDrillSizeAlum_Decimal = new List<List<double>>();

            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.0469 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.0595, 0.0595 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.0700, 0.0700 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.0785, 0.0820 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.0890, 0.0935 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.1015, 0.1040 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.1065, 0.1130 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.1360, 0.1360 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.1495, 0.1590 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.1770, 0.1820, 0.1850 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.2010, 0.2130, 0.2188 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.2570, 0.2720, 0.2812 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.3125, 0.3320, 0.3438 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.3680, 0.3906, 0.4040 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.4219, 0.4531, 0.4688 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.4844, 0.5156, 0.5156 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.5312, 0.5781, 0.5781 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.6406 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.6562, 0.6875, 0.7031 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.7656 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.7656, 0.8125, 0.8281 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.8906 });
            tapDrillSizeAlum_Decimal.Add(new List<double> { 0.8750, 0.9375, 0.9531 });



            /// Setup the tapDrillSizeSteel
            ///

            List<List<string>> tapDrillSizeSteel = new List<List<string>>();

            tapDrillSizeSteel.Add(new List<string> { "55" });
            tapDrillSizeSteel.Add(new List<string> { "1/16", "52" });
            tapDrillSizeSteel.Add(new List<string> { "49", "48" });
            tapDrillSizeSteel.Add(new List<string> { "44", "43" });
            tapDrillSizeSteel.Add(new List<string> { "41", "40" });
            tapDrillSizeSteel.Add(new List<string> { "7/64", "35" });
            tapDrillSizeSteel.Add(new List<string> { "32", "31" });
            tapDrillSizeSteel.Add(new List<string> { "27", "26" });
            tapDrillSizeSteel.Add(new List<string> { "20", "18" });
            tapDrillSizeSteel.Add(new List<string> { "12", "10", "9" });
            tapDrillSizeSteel.Add(new List<string> { "7/32", "1", "1" });
            tapDrillSizeSteel.Add(new List<string> { "J", "9/32", "L" });
            tapDrillSizeSteel.Add(new List<string> { "Q", "S", "T" });
            tapDrillSizeSteel.Add(new List<string> { "25/64", "13/32", "Z" });
            tapDrillSizeSteel.Add(new List<string> { "29/64", "15/32", "15/32" });
            tapDrillSizeSteel.Add(new List<string> { "33/64", "17/32", "17/32" });
            tapDrillSizeSteel.Add(new List<string> { "9/16", "19/32", "19/32" });
            tapDrillSizeSteel.Add(new List<string> { "21/32" });
            tapDrillSizeSteel.Add(new List<string> { "11/16", "45/64", "23/32" });
            tapDrillSizeSteel.Add(new List<string> { "25/32" });
            tapDrillSizeSteel.Add(new List<string> { "51/64", "53/64", "27/32" });
            tapDrillSizeSteel.Add(new List<string> { "29/32" });
            tapDrillSizeSteel.Add(new List<string> { "59/64", "61/64", "31/32" });


            List<List<double>> tapDrillSizeSteel_Decimal = new List<List<double>>();

            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.0520 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.0625, 0.0635 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.0730, 0.0760 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.0860, 0.0890 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.0960, 0.0980 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.1094, 0.1100 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.1160, 0.1200 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.1440, 0.1470 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.1610, 0.1695 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.1890, 0.1935, 0.1960 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.2188, 0.2280, 0.2280 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.2770, 0.2812, 0.2900 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.3320, 0.3480, 0.3580 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.3906, 0.4062, 0.4130 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.4531, 0.4688, 0.4688 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.5156, 0.5312, 0.5312 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.5625, 0.5938, 0.5938 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.6562 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.6875, 0.7031, 0.7188 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.7812 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.7969, 0.8281, 0.8438 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.9062 });
            tapDrillSizeSteel_Decimal.Add(new List<double> { 0.9219, 0.9531, 0.9688 });


            ///Set up the clearanceDrillSizeCloseFit
            ///

            List<string> clearanceDrillSizeCloseFit = new List<string>();

            clearanceDrillSizeCloseFit.Add("52");
            clearanceDrillSizeCloseFit.Add("48");
            clearanceDrillSizeCloseFit.Add("43");
            clearanceDrillSizeCloseFit.Add("37");
            clearanceDrillSizeCloseFit.Add("32");
            clearanceDrillSizeCloseFit.Add("30");
            clearanceDrillSizeCloseFit.Add("27");
            clearanceDrillSizeCloseFit.Add("18");
            clearanceDrillSizeCloseFit.Add("9");
            clearanceDrillSizeCloseFit.Add("2");
            clearanceDrillSizeCloseFit.Add("F");
            clearanceDrillSizeCloseFit.Add("P");
            clearanceDrillSizeCloseFit.Add("W");
            clearanceDrillSizeCloseFit.Add("29/64");
            clearanceDrillSizeCloseFit.Add("33/64");
            clearanceDrillSizeCloseFit.Add("37/64");
            clearanceDrillSizeCloseFit.Add("41/64");
            clearanceDrillSizeCloseFit.Add("45/64");
            clearanceDrillSizeCloseFit.Add("49/64");
            clearanceDrillSizeCloseFit.Add("53/64");
            clearanceDrillSizeCloseFit.Add("57/64");
            clearanceDrillSizeCloseFit.Add("61/64");
            clearanceDrillSizeCloseFit.Add("1-1/64");


            List<double> clearanceDrillSizeCloseFit_Decimal = new List<double>();

            clearanceDrillSizeCloseFit_Decimal.Add(0.0635);
            clearanceDrillSizeCloseFit_Decimal.Add(0.0760);
            clearanceDrillSizeCloseFit_Decimal.Add(0.0890);
            clearanceDrillSizeCloseFit_Decimal.Add(0.1040);
            clearanceDrillSizeCloseFit_Decimal.Add(0.1160);
            clearanceDrillSizeCloseFit_Decimal.Add(0.1285);
            clearanceDrillSizeCloseFit_Decimal.Add(0.1440);
            clearanceDrillSizeCloseFit_Decimal.Add(0.1695);
            clearanceDrillSizeCloseFit_Decimal.Add(0.1960);
            clearanceDrillSizeCloseFit_Decimal.Add(0.2210);
            clearanceDrillSizeCloseFit_Decimal.Add(0.2570);
            clearanceDrillSizeCloseFit_Decimal.Add(0.3230);
            clearanceDrillSizeCloseFit_Decimal.Add(0.3860);
            clearanceDrillSizeCloseFit_Decimal.Add(0.4531);
            clearanceDrillSizeCloseFit_Decimal.Add(0.5156);
            clearanceDrillSizeCloseFit_Decimal.Add(0.5781);
            clearanceDrillSizeCloseFit_Decimal.Add(0.6406);
            clearanceDrillSizeCloseFit_Decimal.Add(0.7031);
            clearanceDrillSizeCloseFit_Decimal.Add(0.7656);
            clearanceDrillSizeCloseFit_Decimal.Add(0.8281);
            clearanceDrillSizeCloseFit_Decimal.Add(0.8906);
            clearanceDrillSizeCloseFit_Decimal.Add(0.9531);
            clearanceDrillSizeCloseFit_Decimal.Add(1.0156);


            ///Set up clearanceDrillSizeFreeFit
            ///

            List<string> clearanceDrillSizeFreeFit = new List<string>();

            clearanceDrillSizeFreeFit.Add("50");
            clearanceDrillSizeFreeFit.Add("46");
            clearanceDrillSizeFreeFit.Add("41");
            clearanceDrillSizeFreeFit.Add("35");
            clearanceDrillSizeFreeFit.Add("30");
            clearanceDrillSizeFreeFit.Add("29");
            clearanceDrillSizeFreeFit.Add("25");
            clearanceDrillSizeFreeFit.Add("16");
            clearanceDrillSizeFreeFit.Add("7");
            clearanceDrillSizeFreeFit.Add("1");
            clearanceDrillSizeFreeFit.Add("H");
            clearanceDrillSizeFreeFit.Add("Q");
            clearanceDrillSizeFreeFit.Add("X");
            clearanceDrillSizeFreeFit.Add("15/32");
            clearanceDrillSizeFreeFit.Add("17/32");
            clearanceDrillSizeFreeFit.Add("19/32");
            clearanceDrillSizeFreeFit.Add("21/32");
            clearanceDrillSizeFreeFit.Add("23/32");
            clearanceDrillSizeFreeFit.Add("25/32");
            clearanceDrillSizeFreeFit.Add("27/32");
            clearanceDrillSizeFreeFit.Add("29/32");
            clearanceDrillSizeFreeFit.Add("31/32");
            clearanceDrillSizeFreeFit.Add("1-1/32");


            List<double> clearanceDrillSizeFreeFit_Decimal = new List<double>();

            clearanceDrillSizeFreeFit_Decimal.Add(0.0700);
            clearanceDrillSizeFreeFit_Decimal.Add(0.0810);
            clearanceDrillSizeFreeFit_Decimal.Add(0.0960);
            clearanceDrillSizeFreeFit_Decimal.Add(0.1100);
            clearanceDrillSizeFreeFit_Decimal.Add(0.1285);
            clearanceDrillSizeFreeFit_Decimal.Add(0.1360);
            clearanceDrillSizeFreeFit_Decimal.Add(0.1495);
            clearanceDrillSizeFreeFit_Decimal.Add(0.1770);
            clearanceDrillSizeFreeFit_Decimal.Add(0.2010);
            clearanceDrillSizeFreeFit_Decimal.Add(0.2280);
            clearanceDrillSizeFreeFit_Decimal.Add(0.2660);
            clearanceDrillSizeFreeFit_Decimal.Add(0.3320);
            clearanceDrillSizeFreeFit_Decimal.Add(0.3970);
            clearanceDrillSizeFreeFit_Decimal.Add(0.4687);
            clearanceDrillSizeFreeFit_Decimal.Add(0.5312);
            clearanceDrillSizeFreeFit_Decimal.Add(0.5938);
            clearanceDrillSizeFreeFit_Decimal.Add(0.6562);
            clearanceDrillSizeFreeFit_Decimal.Add(0.7188);
            clearanceDrillSizeFreeFit_Decimal.Add(0.7812);
            clearanceDrillSizeFreeFit_Decimal.Add(0.8438);
            clearanceDrillSizeFreeFit_Decimal.Add(0.9062);
            clearanceDrillSizeFreeFit_Decimal.Add(0.9688);
            clearanceDrillSizeFreeFit_Decimal.Add(1.0313);



            ///Assign values to the field;
            ///

            Chart_ScrewSize = chart_ScrewSize;
            ThreadPerIn = threadPerIn;
            MinorDiameters = minorDiameter;
            TapDrillSizeAlum = tapDrillSizeAlum;
            TapDrillSizeAlum_Decimal = tapDrillSizeAlum_Decimal;
            TapDrillSizeSteel = tapDrillSizeSteel;
            TapDrillSizeSteel_Decimal = tapDrillSizeSteel_Decimal;
            ClearanceDrillSizeCloseFit = clearanceDrillSizeCloseFit;
            ClearanceDrillSizeCloseFit_Decimal = clearanceDrillSizeCloseFit_Decimal;
            ClearanceDrillSizeFreeFit = clearanceDrillSizeFreeFit;
            ClearanceDrillSizeFreeFit_Decimal = clearanceDrillSizeFreeFit_Decimal;
        }





    }
}
