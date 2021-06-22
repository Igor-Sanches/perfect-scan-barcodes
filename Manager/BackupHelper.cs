
using Perfect_Scan.Data;
using Perfect_Scan.Database.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace Perfect_Scan.Manager
{
    public class BackupHelper
    {
        public static string RegexReplace = "_..._forreplace_..._";
        public static string RegexReplace2 = "_..._forreinNextplace_..._";
        private StorageFile file;

        public BackupHelper(StorageFile file)
        {
            this.file = file;
        }

        public async Task<XmlDocument> GetDocumentAsync()
        {
            try
            {
                var xDoc = new XmlDocument();
                var doc = await XmlDocument.LoadFromFileAsync(file);
                xDoc.LoadXml(doc.GetXml());
                return xDoc;
            }
            catch { return null; }
        }

        public async Task<XmlElement> GetElementAsync(string nameTag)
        {
            try
            {
                var doc = await GetDocumentAsync();
                XmlElement element = null;
                XmlNodeList lista = doc.GetElementsByTagName(nameTag);
                IXmlNode node = lista.Item(0);
                if (node.NodeType == NodeType.ElementNode)
                {
                    element = (XmlElement)node;
                }
                return element;
            }
            catch { return null; }
        }

        public async Task<List<Escaneados>> GetListaEscaneadosAsync()
        { 
            string formato, titulo, data, tipo, codigo;
            List<Escaneados> lista_ = new List<Escaneados>();
            Escaneados escaneados = new Escaneados();
            var doc = await GetDocumentAsync();
            XmlNodeList lista = doc.GetElementsByTagName(Key.ELEMENT_KEY_DATABASE_ESCANEADOS);
            int count = lista.Count;
            for(uint i = 0; i < count; i++)
            {
                IXmlNode xmlNode = lista.Item(i);
                if(xmlNode.NodeType == NodeType.ElementNode)
                {
                    XmlElement elemento = (XmlElement)xmlNode;
                    titulo = elemento.GetAttribute(Key.ELEMENT_KEY_DATABASE_FORMAT);
                    data = elemento.GetAttribute(Key.ELEMENT_KEY_DATABASE_DATA);
                    tipo = elemento.GetAttribute(Key.ELEMENT_KEY_DATABASE_TYPE);
                    formato = elemento.GetAttribute(Key.ELEMENT_KEY_DATABASE_FORMAT);
                    codigo = elemento.InnerText.Replace(RegexReplace, "<").Replace(RegexReplace2, "&");
                    escaneados.Codigo = codigo;
                    escaneados.Data = data;
                    escaneados.DisplayName = titulo.Replace("\"", RegexReplace);
                    escaneados.Formato = formato;
                    escaneados.Tipo = tipo;
                    lista_.Add(escaneados);
                }
            }
            return lista_; 
        }
         
        public async Task<List<Gerados>> GetListaGeradosAsync()
        {
            string formato, titulo, data, tipo, codigo;
            List<Gerados> lista_ = new List<Gerados>();
            Gerados gerados = new Gerados();
            var doc = await GetDocumentAsync();
            XmlNodeList lista = doc.GetElementsByTagName(Key.ELEMENT_KEY_DATABASE_GERADOS);
            int count = lista.Count;
            for (uint i = 0; i < count; i++)
            {
                IXmlNode xmlNode = lista.Item(i);
                if (xmlNode.NodeType == NodeType.ElementNode)
                {
                    XmlElement elemento = (XmlElement)xmlNode;
                    titulo = elemento.GetAttribute(Key.ELEMENT_KEY_DATABASE_FORMAT);
                    data = elemento.GetAttribute(Key.ELEMENT_KEY_DATABASE_DATA);
                    tipo = elemento.GetAttribute(Key.ELEMENT_KEY_DATABASE_TYPE);
                    formato = elemento.GetAttribute(Key.ELEMENT_KEY_DATABASE_FORMAT);
                    codigo = elemento.InnerText.Replace(RegexReplace, "<").Replace(RegexReplace2, "&");
                    gerados.Codigo = codigo;
                    gerados.Data = data;
                    gerados.DisplayName = titulo.Replace("\"", RegexReplace);
                    gerados.Formato = formato;
                    gerados.Tipo = tipo;
                    lista_.Add(gerados);
                }
            }
            return lista_;
        }

        public async Task<bool> IsGeradosAsync()
        {
            string str = await GetStringForXmlAsync(Key.ELEMENT_KEY_GERADOS);
            //return str.Equals("true") || str.Equals("True") || str.Equals("TRUE");
            return System.Convert.ToBoolean(str);
        }

        public async Task<bool> IsEscaneadosAsync()
        {
            string str = await GetStringForXmlAsync(Key.ELEMENT_KEY_ESCANEADOS);
            //return str.Equals("true") || str.Equals("True") || str.Equals("TRUE");
            return System.Convert.ToBoolean(str); 
        }

        private async Task<string> GetStringAsync(string str)
        {
            var element = await GetElementAsync(Key.ELEMENT_KEY_INFO);
            return element.GetAttribute(str);
        }

        private async Task<string> GetStringForXmlAsync(string str)
        {
            var element = await GetElementAsync(Key.ELEMENT_KEY_DATA_BASE);
            return element.GetAttribute(str);
        }

        public string DisplayName => file.DisplayName;

        public string BackupPath => file.Path;


        public async Task<string> GetSizeBackupAsync()
        {

            var fileSize = await file.GetBasicPropertiesAsync();
            var size = fileSize.Size;
            SizeUnits units = SizeUnits.Bytes;
            if (size < 1024) units = SizeUnits.Bytes;
            else if (size > 1024 * 1024 * 1024) units = SizeUnits.GB;
            else if (size > 1024 * 1024) units = SizeUnits.MB;
            else if (size > 1024) units = SizeUnits.KB;
            return $"{(size / Math.Pow(1024, (Int64)units)).ToString($"0.0 {units.ToString()}")}";

        }

        public async Task<string> GetBackupDeviceAsync()
        {
            var str = await GetStringAsync(Key.ELEMENT_KEY_DEVICE);
            return str;
        }

        public async Task<string> GetBackupCountEscaneadosAsync()
        {
            var str = await GetStringAsync(Key.ELEMENT_KEY_COUNT_ESCANEADOS);
            return str;
        }

        public async Task<string> GetBackupCountGeradosAsync()
        {
            var str = await GetStringAsync(Key.ELEMENT_KEY_COUNT_GERADOS);
            return str;
        }

        public async Task<string> GetBackupVersionAsync()
        {
            var str = await GetStringAsync(Key.ELEMENT_KEY_SYSTEM_VERSION);
            return str;
        }

        public async Task<string> GetBackupHoraDiaAsync()
        {
            var str = await GetStringAsync(Key.ELEMENT_KEY_DATA);
            return str;
        }

        public async Task<string> GetBackupSystemAsync()
        {
            var str = await GetStringAsync(Key.ELEMENT_KEY_SYSTEM);
            return str;
        }

        public async Task<string> GetResumoCountsAsync()
        {
            List<string> counts = new List<string>();
            int countS = System.Convert.ToInt32(await GetBackupCountEscaneadosAsync());
            int countG = System.Convert.ToInt32(await GetBackupCountGeradosAsync());

            if(countS > 0)
            {
                string result = countS == 1 ? "Escaneado" : "Escaneados";

                counts.Add($"{countS} {result}");
            }
            if (countG > 0)
            {
                string result = countG == 1 ? "Gerado" : "Geradoss";
                counts.Add($"{countG} {result}");
            }

            string x = "";
            if (counts.Count == 2)
            {
                x = $"{counts[0]}, {counts[1]}";
            }
            else x = counts[0]; 

            return x;
        }
    }
}