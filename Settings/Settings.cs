using Perfect_Scan.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Perfect_Scan.Settings
{
    public class Settings : Key
    {
        private ResourceLoader loader = new ResourceLoader(); 
        private ApplicationDataContainer container = ApplicationData.Current.LocalSettings;

        public int GetCodigoIndex
        {
            get { return Data.Data.GetPaginasIndex(Data.Data.PaginaViewIndex); }
        }

        public bool IsUseAutoFocus
        {
            get
            {
                bool _bool;
                if (container.Values.ContainsKey(APP_SETTINGS_AUTO_FOCUS)) _bool = (bool)container.Values[APP_SETTINGS_AUTO_FOCUS]; else _bool = true;
                return _bool;
            }
        }
        public bool IsUseFrontalCamAvainable
        {
            get
            {
                bool _bool;
                if (container.Values.ContainsKey(APP_SETTINGS_FRONTAL_CAMERA)) _bool = (bool)container.Values[APP_SETTINGS_FRONTAL_CAMERA]; else _bool = true;
                return _bool;
            }
        }
        public bool IsScanInverso
        {
            get
            {
                bool _bool;
                if (container.Values.ContainsKey(APP_SETTINGS_INVERTER_SCAN)) _bool = (bool)container.Values[APP_SETTINGS_INVERTER_SCAN]; else _bool = false;
                return _bool;
            }
            set
            {
                if (!container.Values.ContainsKey(APP_SETTINGS_INVERTER_SCAN)) container.Values.Add(APP_SETTINGS_INVERTER_SCAN, value); else container.Values[APP_SETTINGS_INVERTER_SCAN] = value;
            }
        }
        public bool IsScanEmMassa
        {
            get
            {
                bool _bool;
                if (container.Values.ContainsKey(APP_SETTINGS_SCAN_MASSA)) _bool = (bool)container.Values[APP_SETTINGS_SCAN_MASSA]; else _bool = false;
                return _bool;
            }
            set
            {
                if (!container.Values.ContainsKey(APP_SETTINGS_SCAN_MASSA)) container.Values.Add(APP_SETTINGS_SCAN_MASSA, value); else container.Values[APP_SETTINGS_SCAN_MASSA] = value;
            }
        }
        public bool IsBeepScan
        {
            get
            {
                bool _bool;
                if (container.Values.ContainsKey(APP_SETTINGS_APTAR)) _bool = (bool)container.Values[APP_SETTINGS_APTAR]; else _bool = true;
                return _bool;
            }
        }
        public bool IsVibrateScan
        {
            get
            {
                bool _bool;
                if (container.Values.ContainsKey(APP_SETTINGS_VIBRAR)) _bool = (bool)container.Values[APP_SETTINGS_VIBRAR]; else _bool = false;
                return _bool;
            }
        }
        public string GetLaserColor
        {
            get
            {
                string _string = Get("laser_default");
                string[] strings = new string[] { "laser_default", "laser_blue_bright", "laser_blue_light", "laser_blue_dark", "laser_green_light", "laser_green_dark", "laser_orange_light", "laser_orange_dark", "laser_purple", "laser_red_light", "laser_red_dark" };
                switch (GetLaserColorValue)
                {
                    case 0: _string = Get(strings[0]); break;
                    case 1: _string = Get(strings[1]); break;
                    case 2: _string = Get(strings[2]); break;
                    case 3: _string = Get(strings[3]); break;
                    case 4: _string = Get(strings[4]); break;
                    case 5: _string = Get(strings[5]); break;
                    case 6: _string = Get(strings[6]); break;
                    case 7: _string = Get(strings[7]); break;
                    case 8: _string = Get(strings[8]); break;
                    case 9: _string = Get(strings[9]); break;
                    case 10: _string = Get(strings[10]); break;
                }
                return $"{Get("laserColor")}: {_string}";
            }
        } 
        private string Get(string v)
        { 
            
            return loader.GetString(v);
        }
        public int GetLaserColorValue
        {
            get
            {
                int _int;
                if (container.Values.ContainsKey(APP_SETTINGS_LASER_COR)) _int = (int)container.Values[APP_SETTINGS_LASER_COR]; else _int = 0;
                return _int;
            }
        }
        public bool IsSalvarEscaneados
        {
            get
            {
                bool _bool;
                if (container.Values.ContainsKey(APP_SETTINGS_SALVAR_ESCANEADOS)) _bool = (bool)container.Values[APP_SETTINGS_SALVAR_ESCANEADOS]; else _bool = true;
                return _bool;
            }
        }
        public bool IsSalvarGerados
        {
            get
            {
                bool _bool;
                if (container.Values.ContainsKey(APP_SETTINGS_SALVAR_GERADOS)) _bool = (bool)container.Values[APP_SETTINGS_SALVAR_GERADOS]; else _bool = true;
                return _bool;
            }
        }
        public bool IsFalarResultadoAoEscanear
        {
            get
            {
                bool _bool;
                if (container.Values.ContainsKey(APP_SETTINGS_NARRAR_RESULTADO)) _bool = (bool)container.Values[APP_SETTINGS_NARRAR_RESULTADO]; else _bool = false;
                return _bool;
            }
        }
        public bool IsFalarResultadoAoEscanearMassa
        {
            get
            {
                bool _bool;
                if (container.Values.ContainsKey(APP_SETTINGS_NARRAR_RESULTADO_MASSA)) _bool = (bool)container.Values[APP_SETTINGS_NARRAR_RESULTADO_MASSA]; else _bool = false;
                return _bool;
            }
        }
        public string GetTipoCodigo
        {
            get
            {
                string _string = loader.GetString("tipoCodigo");
                switch (GetTipoCodigoValue)
                {
                    case 0: _string = loader.GetString("tipoCodigo"); break;
                    case 1: _string = loader.GetString("formatoCodigo"); break;
                    case 2: _string = loader.GetString("resultado"); break;
                }
                return _string;
            }
        }
        public int GetTipoCodigoValue
        {
            get
            {
                int _int;
                if (container.Values.ContainsKey(APP_SETTINGS_TIPO_NARRAR)) _int = (int)container.Values[APP_SETTINGS_TIPO_NARRAR]; else _int = 0;
                return _int;
            }
        }

        public bool IsDarkMode
        {
            get
            {
                bool _bool;
                if (container.Values.ContainsKey(APP_SETTINGS_DARK_MODE)) _bool = (bool)container.Values[APP_SETTINGS_DARK_MODE]; else _bool = false;
                return _bool;
            }
            set
            {
                if (!container.Values.ContainsKey(APP_SETTINGS_DARK_MODE)) container.Values.Add(APP_SETTINGS_DARK_MODE, value); else container.Values[APP_SETTINGS_DARK_MODE] = value;
            }
        }

        public bool IsWifiLaunchConnect
        {
            get
            {
                bool _bool;
                if (container.Values.ContainsKey(APP_SETTINGS_WIFI_LAUNCH_CONNECT)) _bool = (bool)container.Values[APP_SETTINGS_WIFI_LAUNCH_CONNECT]; else _bool = false;
                return _bool;
            }
            set
            {
                if (!container.Values.ContainsKey(APP_SETTINGS_WIFI_LAUNCH_CONNECT)) container.Values.Add(APP_SETTINGS_WIFI_LAUNCH_CONNECT, value); else container.Values[APP_SETTINGS_WIFI_LAUNCH_CONNECT] = value;
            }
        }
         
        public bool IsLinkLaunchRedirect
        {
            get
            {
                bool _bool;
                if (container.Values.ContainsKey(APP_SETTINGS_LINK_LAUNCH_REDIRECT)) _bool = (bool)container.Values[APP_SETTINGS_LINK_LAUNCH_REDIRECT]; else _bool = false;
                return _bool;
            }
            set
            {
                if (!container.Values.ContainsKey(APP_SETTINGS_LINK_LAUNCH_REDIRECT)) container.Values.Add(APP_SETTINGS_WIFI_LAUNCH_CONNECT, value); else container.Values[APP_SETTINGS_LINK_LAUNCH_REDIRECT] = value;
            }
        }
         
        public void Put(string key, bool v)
        {
            if (!container.Values.ContainsKey(key)) container.Values.Add(key, v); else container.Values[key] = v;
        }
        public void Put(string key, int v)
        {
            if (!container.Values.ContainsKey(key)) container.Values.Add(key, v); else container.Values[key] = v;
        }

    }
}
