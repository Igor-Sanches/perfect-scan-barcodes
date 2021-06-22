using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;

namespace Perfect_Scan.Convert
{
    public class Convert
    {
        public static Visibility ToVisibility(bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }

        internal async static Task<Visibility> ToVisibilityByClipboardAsync(DataPackageView view)
        {
            Visibility visibility = Visibility.Collapsed;

            if (view.Contains(StandardDataFormats.Text))
            {
                string ctt = await view.GetTextAsync();
                string text = clearText(ctt);
                visibility = text.Equals("") ? Visibility.Collapsed : Visibility.Visible;
            }

            return visibility;
        }

        private static string clearText(string ctt)
        {
            string text = "";
            foreach(char c in ctt)
            {
                if (",;+#*0123456789".Contains(c))
                {
                    text += c;
                }
            }
            return text;
        }

        internal static int ToInt(string input)
        {
            return System.Convert.ToInt32(input);
        }
    }
}
