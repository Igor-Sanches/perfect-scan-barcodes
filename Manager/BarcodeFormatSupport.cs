using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;

namespace Perfect_Scan.Manager
{

    public class BarcodeFormatSupport
    {
        public static bool IsFormate(BarcodeFormat format)
        {
            bool suporte = false;
            switch (format)
            {
                case BarcodeFormat.AZTEC:
                case BarcodeFormat.CODABAR:
                case BarcodeFormat.CODE_93:
                case BarcodeFormat.CODE_128:
                case BarcodeFormat.DATA_MATRIX:
                case BarcodeFormat.EAN_8:
                case BarcodeFormat.CODE_39:
                case BarcodeFormat.EAN_13:
                case BarcodeFormat.ITF:
                case BarcodeFormat.PDF_417:
                case BarcodeFormat.QR_CODE:
                case BarcodeFormat.UPC_A:
                case BarcodeFormat.UPC_E:
                    suporte = true;
                    break;
            }
            return suporte;
        }
    }
}
