using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Perfect_Scan.Database.Sql;
using Perfect_Scan.Manager;
using Perfect_Scan.Manager.ResultScanner;
using Windows.ApplicationModel.Calls;
using Windows.ApplicationModel.Resources;
using Windows.Globalization.PhoneNumberFormatting;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Perfect_Scan.ViewModel.Visualizador
{
    public class NumeroControlVisualizadorViewModel : ViewModelBase
    {
        private string numberToDial = "", gridView = "Collapsed";
        private bool isProgress = true;
        private ResourceLoader loader = new ResourceLoader();
        private RelayCommand callCommand;
        private RelayCommand dialCommand;
        private RelayCommand saveCommand;
        private PhoneNumberFormatter phone = new PhoneNumberFormatter();

        public ICommand SaveLauncher
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(p => OnSave());
                }
                return saveCommand;
            }
        }

        private async void OnSave()
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri($"ms-people:savetocontact?PhoneNumber={numberToDial}"));
            }
            catch
            {
                var dialog = new ContentDialog();
                dialog.Content = loader.GetString("fallsave");
                dialog.Background = ((SolidColorBrush)App.Current.Resources["Background"]);
                dialog.Foreground = ((SolidColorBrush)App.Current.Resources["Texto"]);
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                await dialog.ShowAsync();

            }
        }

        public ICommand DialLauncher
        {
            get
            {
                if (dialCommand == null)
                {
                    dialCommand = new RelayCommand(p => OnDial());
                }
                return dialCommand;
            }
        }
        private async void OnDial()
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri($"tel:{numberToDial}"));
            }
            catch
            {
                var dialog = new ContentDialog();
                dialog.Content = loader.GetString("fallDial");
                dialog.Background = ((SolidColorBrush)App.Current.Resources["Background"]);
                dialog.Foreground = ((SolidColorBrush)App.Current.Resources["Texto"]);
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                await dialog.ShowAsync();

            }
        }

        public ICommand Call
        {
            get
            {
                if (callCommand == null)
                {
                    callCommand = new RelayCommand(p => OnCall());
                }
                return callCommand;
            }
        }

        private void OnCall()
        {
            try
            { 
                CallingInfo.CallingInfoInstance.DialOnCurrentLineAsync(numberToDial, numberToDial);
            }
            catch (Exception x)
            {
                Toast(x.Message, 1);
            }
        }

        public string GridViewLayoutNumero
        {
            get { return gridView; }
            private set
            {
                if (gridView != value)
                {
                    gridView = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsProgressNumero  
        {
            get { return isProgress; }
            private set
            {
                if (isProgress != value)
                {
                    isProgress = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void PuProgress(bool v)
        {
            if (v)
            {
                IsProgressNumero = false;
                GridViewLayoutNumero = "Visible";
            }
            else
            {
                IsProgressNumero = true;
                GridViewLayoutNumero = "Collapsed";
            }
        }

        private void Toast(string msg, int index)
        {
            Tools.ModoColor color = Tools.ModoColor.None;
            if (index == 0)
            {
                color = Tools.ModoColor.Succes;
            }
            else if (index == 1) { color = Tools.ModoColor.Error; }


            Paginas.Root.RootApp.Instance.GetToast(msg, color);
        }

        internal void PutNumero(NumeroResult numero)
        {  
            Digit = numero.Numero;
             

            PuProgress(true);
        }

        public string Digit
        {
            set
            {
                ClearDialerNumberHeap();
                cleanInput(value);
            }
        }

        public void ClearDialerNumberHeap()
        {
            if (numberToDial.Length > 0)
            {
                this.NumberToDial = ""; 
            }
        }

        private void cleanInput(string content)
        {
            foreach (char c in content)
            {
                if (",;+#*0123456789".Contains(c.ToString()))
                {
                    this.NumberToDial += c; 
                }
            }
        }

        public void ReplaceOnHoldingDigit(string newDigit)
        {
            if (numberToDial.Length > 0)
            {
                if ((newDigit == "+") && (numberToDial.Length > 1))
                {
                    return;
                }
                this.NumberToDial = this.NumberToDial.Remove(numberToDial.Length - 1);
                this.NumberToDial += newDigit;
            }
        }


        public string NumberToDial
        {
            get
            {
                return phone.FormatString(numberToDial);
            }
            private set
            {
                if (numberToDial != value)
                {
                    numberToDial = phone.FormatString(value);
                    RaisePropertyChanged();
                }
            }
        }
    }
}
