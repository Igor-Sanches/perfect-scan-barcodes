using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_Scan.Database
{
    public class HistoricoInfo
    {
        private string DisplayName, Codigo, Tipo, Formato, Data;
        private long Id;

        public long GetId
        {
            get { return Id; }
            set { Id = value; }
        }

        public string GetDisplayName
        {
            get { return DisplayName; }
            set { DisplayName = value; }
        }

        public string GetCodigo
        {
            get { return Codigo; }
            set { Codigo = value; }
        }

        public string GetTipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public string GetFormato
        {
            get { return Formato; }
            set { Formato = value; }
        }

        public string GetData
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
