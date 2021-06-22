using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_Scan.Manager.ResultScanner
{
    public class WifiResult : ResultParse
    {
        private string codigo;
        private string ssid, senha, tipo;
        private bool isVaid; 

        public WifiResult(String codigo)
        {
            this.codigo = codigo;
        }
         
        public void CodificarWifi()
        {
            try
            {
                isVaid = true;
                String rawText = codigo;// getMassagedText(result);
                if (!rawText.StartsWith("WIFI:"))
                {
                    isVaid = false;
                }
                String rawText2 = rawText.Substring(5);
                ssid = MatchSinglePrefixedField("S:", rawText2, ';', false);
                if (ssid == null || ssid.Equals(""))
                {
                    isVaid = false;
                }
                tipo = MatchSinglePrefixedField("T:", rawText2, ';', false);
                if (!tipo.Equals("nopass"))
                {
                    senha = MatchSinglePrefixedField("P:", rawText2, ';', false);
                }

            }
            catch (Exception x)
            {
         
   }
        }

        public bool IsSegurity { get { return !tipo.Equals("nopass"); } }

        public bool IsValidate { get { return isVaid; } }

        public string SSID { get { return ssid; } }

        public string Senha { get { return senha; } }
    }
}
