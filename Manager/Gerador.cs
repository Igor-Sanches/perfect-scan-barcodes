using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using ZXing;
using ZXing.Mobile;

namespace Perfect_Scan.Manager
{
    public class Gerador
    {
        private string codigo;
        private Image format;
        private int Width = 50, Height = 50;
        private ResourceLoader loader = new ResourceLoader();

        public Gerador(string codigo)
        {
            this.codigo = codigo;
        }

        public static Gerador With(string codigo)
        {
            return new Gerador(codigo);
        }

        public Gerador(string codigo, Image format)
        {
            this.codigo = codigo;
            this.format = format;
        }

        public static Gerador With(string codigo, Image format)
        {
            return new Gerador(codigo, format);
        }

        public void GerarCodigo(int index, Frame frame)
        {
            try
            {
                var gerador = new ZXing.BarcodeWriter();
                gerador.Format = GetBarcodeFormat(index);
                gerador.Options.Width = Width;
                gerador.Options.Height = Height;
                var result = gerador.Write(codigo);
                if(result != null)
                {
                    HistoricoManager.Gerado(codigo, gerador.Format.ToString(), frame); 
                } 
            }
            catch (Exception x)
            { 
                string message = "";
                switch (x.Message)
                {
                    case "Found empty contents":
                        {
                            message = loader.GetString("ErrorGerarCodigo1");
                        
                        }break;
                    case "Index was outside the bounds of the array.":
                        {
                            message = loader.GetString("ErrorGerarCodigo2");
                        }
                        break;
                    case "Contents do not pass checksum":
                        {
                            message = loader.GetString("ErrorGerarCodigo3");
                        }
                        break;
                    case "The length of the input should be even":
                        {
                            message = loader.GetString("ErrorGerarCodigo4");
                        }
                        break;
                }

                if (x.Message.Contains("Invalid start/end guards: Cannot encode"))
                {
                    message = x.Message.Replace("Invalid start/end guards: Cannot encode", loader.GetString("ErrorGerarCodigo5"));
                }
                else if (x.Message.Contains("Cannot encode"))
                {
                    message = x.Message.Replace("Cannot encode", loader.GetString("ErrorGerarCodigo6"));
                }
                else if (x.Message.Contains("Bad character in input"))
                {
                    message = x.Message.Replace("Bad character in input", loader.GetString("ErrorGerarCodigo7"));
                }
                else if (x.Message.Contains("Contents length should be between 1 and 80 characters, but got"))
                {
                    message = x.Message.Replace("Contents length should be between 1 and 80 characters, but got", loader.GetString("ErrorGerarCodigo8"));
                }
                else if (x.Message.Contains("Requested contents should be less than 80 digits long, but got"))
                {
                    message = x.Message.Replace("Requested contents should be less than 80 digits long, but got", loader.GetString("ErrorGerarCodigo9"));
                }
                else if (x.Message.Contains("Requested content contains a non-encodable character"))
                {
                    message = x.Message.Replace("Requested content contains a non-encodable character", loader.GetString("ErrorGerarCodigo10"));
                }
                else if (x.Message.Contains("Can't find a symbol arrangement that matches the message. Data codewords"))
                {
                    message = x.Message.Replace("Can't find a symbol arrangement that matches the message. Data codewords", loader.GetString("ErrorGerarCodigo11"));
                }
                else if (x.Message.Contains("Requested contents should be 12 (without checksum digit) or 13 digits long, but got"))
                {
                    message = x.Message.Replace("Requested contents should be 12 (without checksum digit) or 13 digits long, but got", loader.GetString("ErrorGerarCodigo12"));
                }
                else if (x.Message.Contains("Requested contents should be 7 (without checksum digit) or 8 digits long, but got"))
                {
                    message = x.Message.Replace("Requested contents should be 7 (without checksum digit) or 8 digits long, but got", loader.GetString("ErrorGerarCodigo13"));
                }
                else if (x.Message.Contains("Contents should only contain digits, but got"))
                {
                    message = x.Message.Replace("Contents should only contain digits, but got", loader.GetString("ErrorGerarCodigo14"));
                }
                else if(x.Message.Contains("Requested contents should be 8 digits long, but got"))
                {
                    message = x.Message.Replace("Requested contents should be 8 digits long, but got", loader.GetString("ErrorGerarCodigo15"));
                }

                if (message.Equals(""))
                {
                    message = loader.GetString("ErrorGerarCodigo16");
                }

                Paginas.Root.RootApp.Instance.GetToast(message, Tools.ModoColor.Error, 4);
            }
        }

        internal void Format(string formato)
        {
            try
            {
                var gerador = new ZXing.BarcodeWriter(); 
                gerador.Format = GetBarcodeFormat(formato); 
                SetDimensionCode(gerador);
                var result = gerador.Write(codigo);
                if (result != null)
                {
                    format.Source = result;
                }
            }
            catch (Exception x)
            {
                string message = "";
                switch (x.Message)
                {
                    case "Found empty contents":
                        {
                            message = loader.GetString("ErrorGerarCodigo1");

                        }
                        break;
                    case "Index was outside the bounds of the array.":
                        {
                            message = loader.GetString("ErrorGerarCodigo2");
                        }
                        break;
                    case "Contents do not pass checksum":
                        {
                            message = loader.GetString("ErrorGerarCodigo3");
                        }
                        break;
                    case "The length of the input should be even":
                        {
                            message = loader.GetString("ErrorGerarCodigo4");
                        }
                        break;
                }

                if (x.Message.Contains("Invalid start/end guards: Cannot encode"))
                {
                    message = x.Message.Replace("Invalid start/end guards: Cannot encode", loader.GetString("ErrorGerarCodigo5"));
                }
                else if (x.Message.Contains("Cannot encode"))
                {
                    message = x.Message.Replace("Cannot encode", loader.GetString("ErrorGerarCodigo6"));
                }
                else if (x.Message.Contains("Bad character in input"))
                {
                    message = x.Message.Replace("Bad character in input", loader.GetString("ErrorGerarCodigo7"));
                }
                else if (x.Message.Contains("Contents length should be between 1 and 80 characters, but got"))
                {
                    message = x.Message.Replace("Contents length should be between 1 and 80 characters, but got", loader.GetString("ErrorGerarCodigo8"));
                }
                else if (x.Message.Contains("Requested contents should be less than 80 digits long, but got"))
                {
                    message = x.Message.Replace("Requested contents should be less than 80 digits long, but got", loader.GetString("ErrorGerarCodigo9"));
                }
                else if (x.Message.Contains("Requested content contains a non-encodable character"))
                {
                    message = x.Message.Replace("Requested content contains a non-encodable character", loader.GetString("ErrorGerarCodigo10"));
                }
                else if (x.Message.Contains("Can't find a symbol arrangement that matches the message. Data codewords"))
                {
                    message = x.Message.Replace("Can't find a symbol arrangement that matches the message. Data codewords", loader.GetString("ErrorGerarCodigo11"));
                }
                else if (x.Message.Contains("Requested contents should be 12 (without checksum digit) or 13 digits long, but got"))
                {
                    message = x.Message.Replace("Requested contents should be 12 (without checksum digit) or 13 digits long, but got", loader.GetString("ErrorGerarCodigo12"));
                }
                else if (x.Message.Contains("Requested contents should be 7 (without checksum digit) or 8 digits long, but got"))
                {
                    message = x.Message.Replace("Requested contents should be 7 (without checksum digit) or 8 digits long, but got", loader.GetString("ErrorGerarCodigo13"));
                }
                else if (x.Message.Contains("Contents should only contain digits, but got"))
                {
                    message = x.Message.Replace("Contents should only contain digits, but got", loader.GetString("ErrorGerarCodigo14"));
                }
                else if (x.Message.Contains("Requested contents should be 8 digits long, but got"))
                {
                    message = x.Message.Replace("Requested contents should be 8 digits long, but got", loader.GetString("ErrorGerarCodigo15"));
                }

                if (message.Equals(""))
                {
                    message = loader.GetString("ErrorGerarCodigo16");
                }

                Paginas.Root.RootApp.Instance.GetToast(message, Tools.ModoColor.Error, 4);
            }
        }

        private void SetDimensionCode(ZXing.BarcodeWriter gerador)
        {
            int width = 0;
            int height = 0;

            switch (gerador.Format)
            {
                case BarcodeFormat.DATA_MATRIX:
                case BarcodeFormat.QR_CODE:
                case BarcodeFormat.AZTEC:
                    width = gerador.Options.Width * 3;
                    height = gerador.Options.Height * 3;
                    break;
                case BarcodeFormat.CODABAR:
                case BarcodeFormat.CODE_128:
                case BarcodeFormat.CODE_39:
                case BarcodeFormat.CODE_93:
                case BarcodeFormat.EAN_13:
                case BarcodeFormat.EAN_8:
                case BarcodeFormat.ITF:
                case BarcodeFormat.UPC_A:
                case BarcodeFormat.UPC_E:
                    width = gerador.Options.Width * 4;
                    height = gerador.Options.Height * 2;
                    //height = 270;
                    break;
                case BarcodeFormat.PDF_417:
                    width = gerador.Options.Width * 4;
                    height = 150;
                    break;
            }

            gerador.Options.Width = width;
            gerador.Options.Height = height; 

        }

        private BarcodeFormat GetBarcodeFormat(string formato)
        {
            BarcodeFormat format = BarcodeFormat.QR_CODE;
            switch (formato)
            {
                case "AZTEC":
                    format = BarcodeFormat.AZTEC;
                    break;
                case "CODABAR":
                    format = BarcodeFormat.CODABAR;
                    break;
                case "CODE_128":
                    format = BarcodeFormat.CODE_128;
                    break;
                case "CODE_39":
                    format = BarcodeFormat.CODE_39;
                    break;
                case "CODE_93":
                    format = BarcodeFormat.CODE_93;
                    break;
                case "DATA_MATRIX":
                    format = BarcodeFormat.DATA_MATRIX;
                    break;
                case "EAN_13":
                    format = BarcodeFormat.EAN_13;
                    break;
                case "EAN_8":
                    format = BarcodeFormat.EAN_8;
                    break;
                case "ITF":
                    format = BarcodeFormat.ITF;
                    break;
                case "PDF_417":
                    format = BarcodeFormat.PDF_417;
                    break;
                case "QR_CODE":
                    format = BarcodeFormat.QR_CODE;
                    break;
                case "UPC_A":
                    format = BarcodeFormat.UPC_A;
                    break;
                case "UPC_E":
                    format = BarcodeFormat.UPC_E;
                    break;
            }
            return format;

        }


        private BarcodeFormat GetBarcodeFormat(int position)
        {
            BarcodeFormat format = BarcodeFormat.QR_CODE;
            switch (position)
            {
                case 0:
                    format = BarcodeFormat.AZTEC;
                    break;
                case 1:
                    format = BarcodeFormat.CODABAR;
                    break;
                case 2:
                    format = BarcodeFormat.CODE_128;
                    break;
                case 3:
                    format = BarcodeFormat.CODE_39;
                    break;
                case 4:
                    format = BarcodeFormat.CODE_93;
                    break;
                case 5:
                    format = BarcodeFormat.DATA_MATRIX;
                    break;
                case 6:
                    format = BarcodeFormat.EAN_13;
                    break;
                case 7:
                    format = BarcodeFormat.EAN_8;
                    break;
                case 8:
                    format = BarcodeFormat.ITF;
                    break;
                case 9:
                    format = BarcodeFormat.PDF_417;
                    break;
                case 10:
                    format = BarcodeFormat.QR_CODE;
                    break;
                case 11:
                    format = BarcodeFormat.UPC_A;
                    break;
                case 12:
                    format = BarcodeFormat.UPC_E;
                    break;
            }
            return format;

        }
    }
}
