using System;
using System.Text.RegularExpressions;

namespace Perfect_Scan.Manager
{
    internal class Email 
    {
        private static Regex EMAIL_VALIDO = new Regex("[a-zA-Z0-9@.!#$&%'*+\\-/?^_`{|}~]+"); 

        internal static bool Validor(string id)
        {
            string codigo = id;
            if (codigo.StartsWith("mailto:") || codigo.StartsWith("MAILTO:") && IsEmailValido(codigo))
            {
                if (codigo.Contains("@") && !codigo.Contains("/") || codigo.Contains("@") && !codigo.Contains(" "))
                {
                    String[] aar = codigo.Split("@");
                    String number = aar[0];
                    if (codigo.Substring(number.Length + 1).Contains("@") || codigo.Substring(number.Length + 1).Contains(","))
                    {
                        return false;
                    }

                    if (aar[1].Contains("."))
                    {
                        if (aar[1].EndsWith(".") || aar[1].StartsWith(".") || aar[1].Contains(",") || aar[1].Contains(" "))
                            return false;
                        else return true;
                    }
                    else return false;
                }
                return false;
            }
            else if (codigo.Contains("@") && !codigo.Contains(":") && IsEmailValido(codigo))
            {
                String[] aar = codigo.Split("@");
                String number = aar[0];
                if (codigo.Substring(number.Length + 1).Contains("@") || codigo.Substring(number.Length + 1).Contains(",") || codigo.Substring(number.Length + 1).Contains(" "))
                {
                    return false;
                }

                if (aar[1].Contains("."))
                {
                    if (aar[1].EndsWith(".") || aar[1].StartsWith(".") || aar[1].Contains(","))
                        return false;
                    else return true;
                }
                else return false;
            }
            else return false;
        }

        private static bool IsEmailValido(string codigo)
        {
            return EMAIL_VALIDO.IsMatch(codigo);
        }
    }
}