using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_Scan.Manager
{
    public class PersisText : Windows.UI.Xaml.Controls.TextBox
    {
        protected override void OnLostFocus(Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                base.OnLostFocus(e);
            }

            //base.OnLostFocus(e);
        }

        public void OnLostFocusForce(Windows.UI.Xaml.RoutedEventArgs e)
        {
            base.OnLostFocus(e);
        }
    }
}
