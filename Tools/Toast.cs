using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Perfect_Scan.Tools
{
    public enum ModoColor
    {
        Error, Succes, None
    }

    public class Toast
    {
    
        private Timer timer;
        private Grid root;
        private TextBlock msgView;
        private FontIcon iconView;
        private ModoColor mode;
        private string msg;

        public Toast(Grid grid, TextBlock msgView, ModoColor color, string msg)
        {
            timer = new Timer(new TimeSpan(0, 0, 0, 2));
            mode = color;
            root = grid;
            this.msgView = msgView;
            this.msg = msg;
        }

        public Toast(Grid grid, TextBlock msgView, FontIcon icon, ModoColor color, string msg)
        {
            timer = new Timer(new TimeSpan(0, 0, 0, 2));
            iconView = icon;
            mode = color;
            root = grid;
            this.msgView = msgView;
            this.msg = msg;
        }

        public Toast(Grid grid, TextBlock msgView, FontIcon icon, ModoColor color, string msg, int _timer)
        {
            timer = new Timer(new TimeSpan(0, 0, 0, _timer));
            iconView = icon;
            mode = color;
            root = grid;
            this.msgView = msgView;
            this.msg = msg;
        }

        public static Toast MakerView(Grid grid, TextBlock msgView, ModoColor color, string msg)
        {
            return new Toast(grid, msgView, color, msg);
        }
        public static Toast MakerView(Grid grid, TextBlock msgView, FontIcon icon, ModoColor color, string msg)
        {
            return new Toast(grid, msgView, icon, color, msg);
        }

        public static Toast MakerView(Grid grid, TextBlock msgView, FontIcon icon, ModoColor color, string msg, int _timer)
        {
            return new Toast(grid, msgView, icon, color, msg, _timer);
        }

        public void Show()
        {
            switch (mode)
            {
                case ModoColor.None:
                    {
                        root.Background = ((SolidColorBrush)App.Current.Resources["ToastBackground"]);
                    }
                    break;
                case ModoColor.Error:
                    {
                        root.Background = new SolidColorBrush(Colors.DarkRed);
                        iconView.Glyph = "";
                    }
                    break;
                case ModoColor.Succes:
                    {
                        root.Background = new SolidColorBrush(Colors.DarkGreen);
                        iconView.Glyph = "";
                    }
                    break;
            }
            iconView.Visibility = mode == ModoColor.None ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;


            TextBlock text = msgView;
            text.Text = msg != null ? msg : "Null";
            root.Opacity = 10;
            timer.StartTimer();
            root.Visibility = Windows.UI.Xaml.Visibility.Visible; 
            timer.TimerEnded += (s, a) => 
            {
                root.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            }; 

        }

    }
}
