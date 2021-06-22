using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Perfect_Scan.Data
{
    public class Data : Key
    {
        private static ApplicationDataContainer data = ApplicationData.Current.LocalSettings;

        public static bool IsAlignLeft { get { return SttTextAlignment.Equals("Left"); } }
        public static bool IsAlignCenter { get { return SttTextAlignment.Equals("Center"); } }
        public static bool IsAlignRight { get { return SttTextAlignment.Equals("Right"); } }


        public static string SttTextAlignment
        {
            get
            {
                string _string;
                if (data.Values.ContainsKey(APP_DATA_ALIGNMENT)) _string = (string)data.Values[APP_DATA_ALIGNMENT]; else _string = "Left";
                return _string;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_DATA_ALIGNMENT)) data.Values.Add(APP_DATA_ALIGNMENT, value); else data.Values[APP_DATA_ALIGNMENT] = value;
            }
        }

        public static bool SttBoldIsEnabled
        {
            get
            {
                bool _bool;
                if (data.Values.ContainsKey(APP_DATA_BOLD)) _bool = (bool)data.Values[APP_DATA_BOLD]; else _bool = false;
                return _bool;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_DATA_BOLD)) data.Values.Add(APP_DATA_BOLD, value); else data.Values[APP_DATA_BOLD] = value;
            }
        }
        public static bool SttItalicIsEnabled
        {
            get
            {
                bool _bool;
                if (data.Values.ContainsKey(APP_DATA_ITALIC)) _bool = (bool)data.Values[APP_DATA_ITALIC]; else _bool = false;
                return _bool;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_DATA_ITALIC)) data.Values.Add(APP_DATA_ITALIC, value); else data.Values[APP_DATA_ITALIC] = value;
            }
        }

        public static bool IsProduto
        {
            get
            {
                bool _bool;
                if (data.Values.ContainsKey(APP_DATA_PRODUCT)) _bool = (bool)data.Values[APP_DATA_PRODUCT]; else _bool = true;
                return _bool;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_DATA_PRODUCT)) data.Values.Add(APP_DATA_PRODUCT, value); else data.Values[APP_DATA_PRODUCT] = value;
            }
        }

        internal static int SetHistoricoPagina
        {
            get
            {
                int _int;
                if (data.Values.ContainsKey(PAGINA_HISTORICO_SELECTED)) _int = (int)data.Values[PAGINA_HISTORICO_SELECTED]; else _int = 0; ;
                return _int;
            }
            set
            {
                if (!data.Values.ContainsKey(PAGINA_HISTORICO_SELECTED)) data.Values.Add(PAGINA_HISTORICO_SELECTED, value); else data.Values[PAGINA_HISTORICO_SELECTED] = value;
            }
        }

        public static string SttFontFamily
        {
            get
            {
                string _string;
                if (data.Values.ContainsKey(APP_DATA_FONT_FAMILY)) _string = (string)data.Values[APP_DATA_FONT_FAMILY]; else _string = "Segoe UI";
                return _string;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_DATA_FONT_FAMILY)) data.Values.Add(APP_DATA_FONT_FAMILY, value); else data.Values[APP_DATA_FONT_FAMILY] = value;
            }
        }
        public static int SttFontSize
        {
            get
            {
                int _int;
                if (data.Values.ContainsKey(APP_DATA_FONT_SIZE)) _int = (int)data.Values[APP_DATA_FONT_SIZE]; else _int = 18; ;
                return _int;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_DATA_FONT_SIZE)) data.Values.Add(APP_DATA_FONT_SIZE, value); else data.Values[APP_DATA_FONT_SIZE] = value;
            }
        }

        public static int SttWifiType
        {
            get
            {
                int _int;
                if (data.Values.ContainsKey(APP_DATA_WIFI)) _int = (int)data.Values[APP_DATA_WIFI]; else _int = 0; ;
                return _int;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_DATA_WIFI)) data.Values.Add(APP_DATA_WIFI, value); else data.Values[APP_DATA_WIFI] = value;
            }
        }

        public static int SttEncTypes
        {
            get
            {
                int _int;
                if (data.Values.ContainsKey(APP_DATA_ENCOMEDA)) _int = (int)data.Values[APP_DATA_ENCOMEDA]; else _int = 0; ;
                return _int;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_DATA_ENCOMEDA)) data.Values.Add(APP_DATA_ENCOMEDA, value); else data.Values[APP_DATA_ENCOMEDA] = value;
            }
        }

        public static string SttWifiPassword
        {
            get
            {
                return SttWifiType != 0 ? "Visible" : "Collapsed";
            }
        }
         
        public static bool AppAccess
        {
            get
            {
                bool _bool;
                if (data.Values.ContainsKey(APP_DATA_ACCESS)) _bool = (bool)data.Values[APP_DATA_ACCESS]; else _bool = false;
                return _bool;
            }
            set { if (!data.Values.ContainsKey(APP_DATA_ACCESS)) data.Values.Add(APP_DATA_ACCESS, value); else data.Values[APP_DATA_ACCESS] = value; }
        }

        public static string AppViewPager
        {
            get
            {
                string _string;
                if (data.Values.ContainsKey(APP_DATA_VIEW_PAGER)) _string = (string)data.Values[APP_DATA_VIEW_PAGER]; else _string = "Inicio";
                return _string;
            }
            set { if (!data.Values.ContainsKey(APP_DATA_VIEW_PAGER)) data.Values.Add(APP_DATA_VIEW_PAGER, value); else data.Values[APP_DATA_VIEW_PAGER] = value; }
        }
         

        internal static int SortingEscaneados
        {
            get
            {
                int _int;
                if (data.Values.ContainsKey(APP_DATA_SORTING_ESCANEADOS)) _int = (int)data.Values[APP_DATA_SORTING_ESCANEADOS]; else _int = 0;
                return _int;
            }
            set { if (!data.Values.ContainsKey(APP_DATA_SORTING_ESCANEADOS)) data.Values.Add(APP_DATA_SORTING_ESCANEADOS, value); else data.Values[APP_DATA_SORTING_ESCANEADOS] = value; }
        }

        public static int SortingGerados
        {
            get
            {
                int _int;
                if (data.Values.ContainsKey(APP_DATA_SORTING_GERADOS)) _int = (int)data.Values[APP_DATA_SORTING_GERADOS]; else _int = 0;
                return _int;
            }
            set { if (!data.Values.ContainsKey(APP_DATA_SORTING_GERADOS)) data.Values.Add(APP_DATA_SORTING_GERADOS, value); else data.Values[APP_DATA_SORTING_GERADOS] = value; }
        }
 
        public static void SetPaginasIndex(int position, int value)
        {
            if (!data.Values.ContainsKey(APP_DATA_PAGINAS_INDEX + " " + position)) data.Values.Add(APP_DATA_PAGINAS_INDEX + " " + position, value); else data.Values[APP_DATA_PAGINAS_INDEX + " " + position] = value; 
        }

        public static int PaginaViewIndex
        {
            get
            {
                int _int;
                if (data.Values.ContainsKey(APP_DATA_PAGINAS_INDEX_VIEW)) _int = (int)data.Values[APP_DATA_PAGINAS_INDEX_VIEW]; else _int = 0;
                return _int;  
            }
            set { if (!data.Values.ContainsKey(APP_DATA_PAGINAS_INDEX_VIEW)) data.Values.Add(APP_DATA_PAGINAS_INDEX_VIEW, value); else data.Values[APP_DATA_PAGINAS_INDEX_VIEW] = value; }

        }

        public static int Codigo
        {
            get
            {
                int _int;
                if (data.Values.ContainsKey(APP_DATA_CODIGO)) _int = (int)data.Values[APP_DATA_CODIGO]; else _int = 9; ;
                return _int;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_DATA_CODIGO)) data.Values.Add(APP_DATA_CODIGO, value); else data.Values[APP_DATA_CODIGO] = value;
            }
        }

        public static bool IsFaulted
        {
            get
            {
                bool _bool;
                if (data.Values.ContainsKey(APP_DATA_FAULTED)) _bool = (bool)data.Values[APP_DATA_FAULTED]; else _bool = false;
                return _bool;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_DATA_FAULTED)) data.Values.Add(APP_DATA_FAULTED, value); else data.Values[APP_DATA_FAULTED] = value;
            }
        }

        public static bool IsMulti
        {
            get
            {
                bool _bool;
                if (data.Values.ContainsKey(APP_DATA_MULTI_MAKER)) _bool = (bool)data.Values[APP_DATA_MULTI_MAKER]; else _bool = false;
                return _bool;
            }
            set
            {
                if (!data.Values.ContainsKey(APP_DATA_MULTI_MAKER)) data.Values.Add(APP_DATA_MULTI_MAKER, value); else data.Values[APP_DATA_MULTI_MAKER] = value;
            }
        }

        public static int GetPaginasIndex(int position)
        {
            int _int = 9;
            if (data.Values.ContainsKey(APP_DATA_PAGINAS_INDEX + " " + position))
            {
                _int = (int)data.Values[APP_DATA_PAGINAS_INDEX + " " + position];
            }
            else
            {
                switch (position)
                {
                    case 0:
                    case 1:
                    case 3:
                    case 4:
                        _int = 10;
                        break;
                    case 2:
                        _int = 6;
                        break; 
                }
            }
            return _int;
        }
    }
}
