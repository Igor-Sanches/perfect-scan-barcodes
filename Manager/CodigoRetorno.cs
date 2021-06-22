using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using ZXing;

namespace Perfect_Scan.Manager
{
    public class CodigoRetorno
    {
        private static ResourceLoader loader = new ResourceLoader();
        public static string GetResultado(string codigo, string format)
        {
            CodigoFind codeType = CodigoFind.Texto;
            if (CodigoVerificador.From(codigo).IsEMAIL())
            {
                codeType = CodigoFind.Email;
            }
            else if (CodigoVerificador.From(codigo).IsWhatsApp())
            {
                codeType = CodigoFind.Whatsapp;
            }
            else if (CodigoVerificador.From(codigo).IsContato())
            {
                codeType = CodigoFind.Contato;
            }
            else if (CodigoVerificador.From(codigo).IsNumeroTelefone())
            {
                codeType = CodigoFind.Numero;
            }
            else if (CodigoVerificador.From(codigo).IsAppAndroid())
            {
                codeType = CodigoFind.AppAndroid;
            }
            else if (CodigoVerificador.From(codigo).IsAppWindows())
            {
                codeType = CodigoFind.AppWindows;
            }
            else if (CodigoVerificador.From(codigo).IsWIFI())
            {
                codeType = CodigoFind.WIFI;
            }
            else if (CodigoVerificador.From(codigo, format).IsProduto())
            {
                codeType = CodigoFind.Produto;
            }
            else if (CodigoVerificador.From(codigo).IsSMS())
            {
                codeType = CodigoFind.SMS;
            }
            else if (CodigoVerificador.From(codigo).IsUri())
            {
                codeType = CodigoFind.URI;
            }
            else
            {
                codeType = CodigoFind.Texto;
            }
            return codeType.ToString();
        }

        public static string GetName(string formato)
        {
            string name = loader.GetString("QR_CODE");
            switch (formato)
            {
                case "AZTEC": name = loader.GetString("AZTEC"); break;
                case "CODABAR": name = loader.GetString("CODABAR"); break;
                case "CODE_128": name = loader.GetString("CODE_128"); break;
                case "CODE_39": name = loader.GetString("CODE_39"); break;
                case "CODE_93": name = loader.GetString("CODE_93"); break; 
                case "DATA_MATRIX": name = loader.GetString("DATA_MATRIX"); break; 
                case "EAN_13": name = loader.GetString("EAN_13"); break;
                case "EAN_8": name = loader.GetString("EAN_8"); break;
                case "ITF": name = loader.GetString("ITF"); break;
                case "PDF_417": name = loader.GetString("PDF_417"); break;
                case "QR_CODE": name = loader.GetString("QR_CODE"); break;
                case "UPC_A": name = loader.GetString("UPC_A"); break;
                case "UPC_E": name = loader.GetString("UPC_E"); break;
            }
            return name;
        }

        public static string GetIcone(String tipo)
        {
            string res = loader.GetString("ic_action_back");
            switch (tipo)
            {
                case "AppAndroid": res = ""; break;
                case "AppWindows": res = ""; break;
                case "Whatsapp": res = ""; break;
                case "WIFI": res = ""; break;
                case "Contato": res = ""; break;
                case "Numero": res = ""; break;
                case "Texto": res = ""; break;
                case "Produto": res = ""; break;
                case "Email": res = ""; break;
                case "SMS": res = ""; break;
                case "URI": res = ""; break;
            }
            return res;
        }

        public static string GetTipeName(String tipo)
        {
            string res = loader.GetString("ic_action_back");
            switch (tipo)
            {
                case "AppAndroid": res = loader.GetString("ic_action_android_app"); break;
                case "AppWindows": res = loader.GetString("ic_action_windows_app"); break;
                case "Whatsapp": res = loader.GetString("ic_action_whatsapp"); break;
                case "WIFI": res = loader.GetString("ic_action_wifi; break"); break;
                case "Contato": res = loader.GetString("ic_action_contato"); break;
                case "Numero": res = loader.GetString("ic_action_numero"); break;
                case "Texto": res = loader.GetString("ic_action_texto"); break;
                case "Produto": res = loader.GetString("ic_action_produto"); break;
                case "Email": res = loader.GetString("ic_action_back"); break;
                case "SMS": res = loader.GetString("ic_action_back"); break;
                case "URI": res = loader.GetString("ic_action_back"); break;
            }
            return res;
        }
    }
}
