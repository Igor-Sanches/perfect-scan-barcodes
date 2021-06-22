using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Perfect_Scan.Tools
{
    public class Strings
    {
        private static ResourceLoader loader = new ResourceLoader();

        public string AZTEC { get { return loader.GetString("AZTEC"); } }
        public string CODABAR { get { return loader.GetString("CODABAR"); } }
        public string CODE_128 { get { return loader.GetString("CODE_128"); } }
        public string CODE_39 { get { return loader.GetString("CODE_39"); } }
        public string CODE_93 { get { return loader.GetString("CODE_93"); } }
        public string DATA_MATRIX { get { return loader.GetString("DATA_MATRIX"); } }
        public string EAN_13 { get { return loader.GetString("EAN_13"); } }
        public string EAN_8 { get { return loader.GetString("EAN_8"); } }
        public string ITF { get { return loader.GetString("ITF"); } }
        public string PDF_417 { get { return loader.GetString("PDF_417"); } }
        public string QR_CODE { get { return loader.GetString("QR_CODE"); } }
        public string UPC_A { get { return loader.GetString("UPC_A"); } }
        public string UPC_E { get { return loader.GetString("UPC_E"); } }
        public string Aberta { get { return loader.GetString("Aberta"); } }
        public string Segura { get { return loader.GetString("Segura"); } }


    }
}
