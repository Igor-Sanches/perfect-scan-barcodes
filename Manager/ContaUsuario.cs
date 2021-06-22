using Perfect_Scan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Perfect_Scan.Manager
{
    public class ContaUsuario
    {
        private static ApplicationDataContainer container = ApplicationData.Current.LocalSettings;

        public static string GetEmail()
        {
            string _string;
            if (container.Values.ContainsKey("EmailUser")) _string = (string)container.Values["EmailUser"]; else _string = "";
            return _string;
        }

        public static string GetKeyPass()
        {
            string _string;
            if (container.Values.ContainsKey("KeyPassUser")) _string = (string)container.Values["KeyPassUser"]; else _string = "";
            return _string;
        }

        public static string GetDisplayName()
        {
            string _string;
            if (container.Values.ContainsKey("DisplayNameUser")) _string = (string)container.Values["DisplayNameUser"]; else _string = "";
            return _string;
        }

        public static string GetDataUser()
        {
            string _string;
            if (container.Values.ContainsKey("DataUser")) _string = (string)container.Values["DataUser"]; else _string = "";
            return _string;
        }


        public static bool IsLogging()
        {
            bool _bool;
            if (container.Values.ContainsKey("IsLoggingUser")) _bool = (bool)container.Values["IsLoggingUser"]; else _bool = false;
            return _bool;
        }

        public static void PutDataUser(Usuario user)
        {
            if (!container.Values.ContainsKey("EmailUser")) container.Values.Add("EmailUser", user.Email); else container.Values["EmailUser"] = user.Email;
            if (!container.Values.ContainsKey("KeyPassUser")) container.Values.Add("KeyPassUser", user.KeyPass); else container.Values["KeyPassUser"] = user.KeyPass;
            if (!container.Values.ContainsKey("DisplayNameUser")) container.Values.Add("DisplayNameUser", user.DisplayName); else container.Values["DisplayNameUser"] = user.DisplayName;
            if (!container.Values.ContainsKey("DataUser")) container.Values.Add("DataUser", user.Data); else container.Values["DataUser"] = user.Data;
        }

        public static void PutLogging(bool Value)
        {
            if (!container.Values.ContainsKey("IsLoggingUser")) container.Values.Add("IsLoggingUser", Value); else container.Values["IsLoggingUser"] = Value;

        }

    }
}
