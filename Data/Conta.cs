using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Windows.Storage;

namespace Perfect_Scan.Data
{
    class Conta
    {
        private static string APP_SYNC_LIMIT = "NuvemToLimit", APP_LOGGIN = "Logado", APP_UUID = "Uuid", APP_NOME = "Nome", APP_EMAIL = "Email", APP_NUVEM_FROM_ESCANEADOS = "NuvemFromEscaneados", APP_NUVEM_TO_ESCANEADOS = "NuvemToEscaneados", APP_NUVEM_TO_GERADOS = "NuvemToGerados", APP_NUVEM_FROM_GERADOS = "NuvemFromGerados";

        private static ApplicationDataContainer data = ApplicationData.Current.LocalSettings;

        public static void PutSettings(string tag, bool value)
        {
            if (!data.Values.ContainsKey(tag)) data.Values.Add(tag, value); else data.Values[tag] = value;
        }

        public static string Email
        {
            get  
             {
                //string _string;
                // if (data.Values.ContainsKey(APP_EMAIL)) _string = (string)data.Values[APP_EMAIL]; else _string = null;
                return "contateste@perfect.com";
            }
            set
            {
                if (!data.Values.ContainsKey(APP_EMAIL)) data.Values.Add(APP_EMAIL, value); else data.Values[APP_EMAIL] = value;
            }
        }

        public static string UUID
        {
            get
            {
                string _string;
                if (data.Values.ContainsKey(APP_UUID)) _string = (string)data.Values[APP_UUID]; else _string = null;
                return _string;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_UUID)) data.Values.Add(APP_UUID, value); else data.Values[APP_UUID] = value;
            }
        }

        public static string Nome
        {
            get
            {
                //string _string;
                //if (data.Values.ContainsKey(APP_NOME)) _string = (string)data.Values[APP_NOME]; else _string = null;
                return "Conta teste";
            }
            set
            {
                if (!data.Values.ContainsKey(APP_NOME)) data.Values.Add(APP_NOME, value); else data.Values[APP_NOME] = value;
            }
        }

        public static bool IsLogging
        {
            get
            {
                bool _bool;
                if (data.Values.ContainsKey(APP_LOGGIN)) _bool = (bool)data.Values[APP_LOGGIN]; else _bool = false;
                return _bool;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_LOGGIN)) data.Values.Add(APP_LOGGIN, value); else data.Values[APP_LOGGIN] = value;
            }
        }

        internal static void Put(UserCredential result)
        {
            UUID = result.User.Uid;
            IsLogging = result.User != null;
            Nome = result.User.Info.DisplayName;
            Email = result.User.Info.Email;
        }
         
        public static bool IsNuvemFromEscaneados
        {
            get => false;
            /*
            get
            {
                bool _bool;
                if (data.Values.ContainsKey(APP_NUVEM_FROM_ESCANEADOS)) _bool = (bool)data.Values[APP_NUVEM_FROM_ESCANEADOS]; else _bool = true;
                return _bool;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_NUVEM_FROM_ESCANEADOS)) data.Values.Add(APP_NUVEM_FROM_ESCANEADOS, value); else data.Values[APP_NUVEM_FROM_ESCANEADOS] = value;
            }
            */
        }
        public static bool IsNuvemFromGerados
        {
            get => false;
            /*
            get
            {
                bool _bool;
                if (data.Values.ContainsKey(APP_NUVEM_FROM_GERADOS)) _bool = (bool)data.Values[APP_NUVEM_FROM_GERADOS]; else _bool = true;
                return _bool;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_NUVEM_FROM_GERADOS)) data.Values.Add(APP_NUVEM_FROM_GERADOS, value); else data.Values[APP_NUVEM_FROM_GERADOS] = value;
            }
            */
        }
        public static bool IsNuvemLimit
        {
            get => true;
            /*
            get
            {
                bool _bool;
                if (data.Values.ContainsKey(APP_SYNC_LIMIT)) _bool = (bool)data.Values[APP_SYNC_LIMIT]; else _bool = false;
                return _bool;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_SYNC_LIMIT)) data.Values.Add(APP_SYNC_LIMIT, value); else data.Values[APP_SYNC_LIMIT] = value;
            }
            */
        }
        public static bool IsNuvemToEscaneados
        {
            get => false;
            /*
            get
            {
                bool _bool;
                if (data.Values.ContainsKey(APP_NUVEM_TO_ESCANEADOS)) _bool = (bool)data.Values[APP_NUVEM_TO_ESCANEADOS]; else _bool = true;
                return _bool;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_NUVEM_TO_ESCANEADOS)) data.Values.Add(APP_NUVEM_TO_ESCANEADOS, value); else data.Values[APP_NUVEM_TO_ESCANEADOS] = value;
            }
            */
        }
        public static bool IsNuvemToGerados
        {
            get => false;
            /*
            get
            {
                bool _bool;
                if (data.Values.ContainsKey(APP_NUVEM_TO_GERADOS)) _bool = (bool)data.Values[APP_NUVEM_TO_GERADOS]; else _bool = true;
                return _bool;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_NUVEM_TO_GERADOS)) data.Values.Add(APP_NUVEM_TO_GERADOS, value); else data.Values[APP_NUVEM_TO_GERADOS] = value;
            }
            */
        }

        internal static void Clear()
        {
            //IsNuvemToEscaneados = true;
            //IsNuvemFromGerados = true;
            //IsNuvemToGerados = true;
            //IsNuvemFromEscaneados = true;
            UUID = null;
            IsLogging = false;
            Nome = null;
            Email = null;
        }
    }
}
