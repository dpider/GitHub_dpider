using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pde.net.console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0) return;
            string funcName = args[0].ToUpper();
            switch (funcName)
            {
                case "CHANGEFILENOBOMUTF8":
                    if (args.Length == 2)
                    {
                        changeFileNoBomUTF8(args[1]);
                    }
                    else if (args.Length == 3)
                    {
                        changeFileNoBomUTF8(args[1], args[2]);
                    }
                    else return;
                    break;

                default:
                    return;
            } 
        }

        private static void changeFileNoBomUTF8(string inFile)
        {
            string strContent = File.ReadAllText(@inFile);
            string outFile = inFile; 
            UTF8Encoding utf8 = new UTF8Encoding(false);
            File.WriteAllText(outFile, strContent, utf8); 
        }

        private static void changeFileNoBomUTF8(string inFile, string willReplace)
        {
            string strContent = File.ReadAllText(@inFile);
            string outFile = inFile;
            if (!willReplace.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                outFile = Path.ChangeExtension(inFile, "noBom" + Path.GetExtension(inFile)); // + Path.GetFileNameWithoutExtension(inFile) + "_noBom" + Path.GetExtension(inFile);
            }
            UTF8Encoding utf8 = new UTF8Encoding(false);
            File.WriteAllText(outFile, strContent, utf8);
        }
    }
}
