using Microsoft.Toolkit.Extensions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Globalization.PhoneNumberFormatting;

namespace Perfect_Scan.Manager
{
    public class CodigoVerificador// : Interfaces.VerificadorInterface
    {
        private string codigo, formato;
        private Regex DIGITO = new Regex("\\d+");
        private Regex URI_W = new Regex("[a-zA-Z][a-zAZ0-9+->]+:");
        private Regex URI_WH = new Regex("([a-zA-Z0-9-]+\\.){1,6}[a-zA-Z]{2,}(:\\d{1,5})?(/|\\?|$)");
        private Regex ALLOWE = new Regex("[-._~:/?\\[\\]@!$&'()*+,;=Aa-z0-9]+");
        private Regex HOST = new Regex(":/*([^/@]+)@[^/]+");

        public CodigoVerificador(string codigo)
        {
            this.codigo = codigo;
        }

        public CodigoVerificador(string codigo, string formato)
        {
            this.codigo = codigo;
            this.formato = formato;
        }

        public static CodigoVerificador From(string codigo)
        {
            return new CodigoVerificador(codigo);
        }

        public static CodigoVerificador From(string codigo, string formato)
        {
            return new CodigoVerificador(codigo, formato);
        }

        public bool IsContato()
        {
            if (codigo.StartsWith("MECARD:") || codigo.StartsWith("BEGIN:VCARD"))
            {
                return codigo.Contains("N") || codigo.Contains("TEL");
            }
            else { return false; }
        }

        public bool IsNumeroTelefone()
        {
            if (codigo.StartsWith("tel:") || codigo.StartsWith("TEL:"))
            {

                PhoneNumberFormatter phone = new PhoneNumberFormatter();
                string formatado = phone.FormatString(codigo).Replace("tel:", "").Replace("TEL:", "").Replace("+", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                int numero = formatado.Length;
                if (numero < 8) return false;
                else if (numero > 16) return false;
                else
                {
                    if (!new Regex(@"[^\d]").IsMatch(System.Convert.ToString(numero)))
                    {
                        return true;
                    }
                    else return false;
                }


            }
            return false;
        }

        public bool IsWhatsApp()
        {
            if (codigo.Contains("api.whatsapp.com") || codigo.Contains("wa.me/") || codigo.Contains("v.whatsapp.com") || codigo.Contains("chat.whatsapp.com"))
            {
                return true;
            }
            else return false;
        }

        public bool IsAppAndroid()
        {
            if (codigo.Contains("play.google.com") || codigo.Contains("market://") || codigo.Contains("market.android.com"))
            {
                return true;
            }
            else return false;

        }

        public bool IsAppWindows()
        {  
            if (codigo.Contains("https://www.microsoft.com/store/productId")
                || codigo.Contains("ms-windows-store://pdp/?ProductId=")
                || codigo.Contains("ms-windows-store://pdp/?PFM=")
                || codigo.Contains("ms-windows-store://pdp/?PhoneAppId=")
                || codigo.Contains("ms-windows-store://pdp/?AppId=")
                || codigo.Contains("https://www.microsoft.com/store/"))
            {
                return true;
            }
            else return false;
        }

        public bool IsWIFI()
        {
            if (codigo.StartsWith("WIFI:") || codigo.StartsWith("wifi:"))
            {
                if (codigo.Contains(";"))
                {
                    return true;

                }
                else return false;
            }
            else return false;

        }
        
        private string ClearProduto(string produto)
        {
            string x = "";
            foreach(char c in produto)
            {
                x += c;
            }
            return x;
        }

        public bool IsProduto()
        {
            bool pass = false;
            Regex regex = new Regex(@"[^\d]");
            string produto = codigo;
            Regex singaLetra = new Regex("[A-Z]+");
            if (formato.Equals("UPC_A") || formato.Equals("UPC_E") || formato.Equals("EAN_8") || formato.Equals("EAN_13"))
            {
               if (!regex.IsMatch(ClearProduto(produto)))
               {
                    pass = true;
                }  
            }
            if(produto.StartsWith("package:") || produto.Length == 13 || produto.Length == 16 || produto.Length == 18)
            {
                string formatado = "";
                if (produto.StartsWith("package:")) formatado = produto.Replace("package:", ""); else formatado = produto;


                int count = formatado.Length; 
                if (count == 13)
                {
                   if (formatado.StartsWith("SYBAA") || formatado.StartsWith("SYBAB"))
                    {
                        string num = formatado.Substring(5, 8);
                        if (!regex.IsMatch(num))
                        {
                            pass = true;
                        }
                    }
                    else if (formatado.StartsWith("SY"))
                    {
                        string num = formatado.Substring(2, 11);
                        if (!regex.IsMatch(num))
                        {
                            pass = true;
                        }
                        else
                        {
                            string fim = formatado.Substring(11, 2);
                            string fim1 = formatado.Substring(0, 1);
                            string fim2 = formatado.Substring(1, 1);
                            if (singaLetra.IsMatch(fim1) && singaLetra.IsMatch(fim2))
                            {
                                string center = formatado.Substring(2, 9);
                                if (!regex.IsMatch(center))
                                {
                                    pass = true;
                                }
                            }
                        }

                    }
                    else
                    {
                        string a1 = formatado.Substring(0, 1);
                        string a2 = formatado.Substring(1, 1);
                        if (singaLetra.IsMatch(a1) && singaLetra.IsMatch(a2))
                        {
                            string b = formatado.Substring(2, 9);
                            if (!regex.IsMatch(b))
                            {
                                string c = formatado.Substring(11, 2);
                                string c1 = c.Substring(0, 1);
                                string c2 = c.Substring(1, 1);
                                if (singaLetra.IsMatch(c1) && singaLetra.IsMatch(c2))
                                {
                                    pass = true;
                                }
                            }
                        }
                    }
                }
                else if (count == 18)
                {
                    if (formatado.StartsWith("YT"))
                    {
                        string num = formatado.Substring(2, 16);
                        if (!regex.IsMatch(num))
                        {
                            pass = true;
                        }
                    }
                }
                else if(count == 16)
                {
                    if (formatado.StartsWith("LP"))
                    {
                        string num = formatado.Substring(2, 14);
                        if (!regex.IsMatch(num))
                        {
                            pass = true;
                        }
                    }
                }

            }
            return pass;
            
        }
         
        public bool IsEMAIL()
        {
            return codigo.IsEmail() || Email.Validor(codigo);
        }

        public bool IsSMS()
        {
            if (codigo.StartsWith("smsto:") || codigo.StartsWith("SMSTO:"))
            {
                return true;
            }
            else return false;
        }
         
        public bool IsUri()
        {
            if (codigo.StartsWith("http://") || codigo.StartsWith("https://"))
            {
                if (codigo.Contains(" "))
                    return false;
                else
                {
                    if (!codigo.EndsWith("."))
                    {
                        return true;
                    }
                    else return false;
                }
            }
            else if (codigo.StartsWith("URL:") || codigo.StartsWith("URI:"))
            {
                return true;
            }
            if (!IsBasicValueURI() || IsPossibleURI())
            {
                return false;
            }
            else return false;

        }

        private bool IsPossibleURI()
        {
            return ALLOWE.IsMatch(codigo);
        }

        private bool IsBasicValueURI()
        {
            if (codigo.Contains(" "))
            {
                return false;
            } 

            return true;
        }

    }
}
