using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_Scan.Manager.ResultScanner
{
    public class NumeroResult
    {
        private string codigo;
        private bool isValid = true;
        private string numero;

        public NumeroResult(string codigo)
        {
            this.codigo = codigo;
        }

        public void DecodificarNumero()
        {
            isValid = true;
            String telURI;
            String rawText = codigo;
            if (!rawText.StartsWith("tel:") && !rawText.StartsWith("TEL:"))
            {
                isValid = false;
            }
            if (rawText.StartsWith("TEL:"))
            {
                telURI = "tel:" + rawText.Substring(4);
            }
            else
            {
                telURI = rawText;
            }

            string number = filterNumber(telURI);

            if(number == null)
            {
                numero = telURI.Substring(4, telURI.Length - 4);
            }
            else
            {
                numero = number;
            }

        }

        private string filterNumber(string telURI)
        {
            string filter = "";
            foreach(char i in telURI)
            {
                if ("*#+0123456789".Contains(i))
                {
                    filter += i;
                }
            }
            return filter;
        }

        public string Numero { get { return numero; } }

        public bool IsValidate { get { return isValid; } }
    }
}
