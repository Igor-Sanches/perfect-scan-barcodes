using Perfect_Scan.Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Perfect_Scan.ViewModel
{
    public class WifiControlViewModel : ViewModelBase
    {
        private ResourceLoader loader = new ResourceLoader();
        private RelayCommand btnGerar, wifiScanCommand; 
        private WiFiAdapter firstAdapter;
        private Frame frame;
        private bool btnGerarEnabled = false;
        private string wifiNameDigit = "", wifiKeyDigit = "", wifiPassWord = "", placeholderTextWifiKey = "", placeholderTextWifiName = "", wifiDisponivelPrglayout = "True", wifiDisponivellayout = "Collapsed";

        public ICommand GerarCommand
        {
            get
            {
                if(btnGerar == null)
                {
                    btnGerar = new RelayCommand(p => this.OnGerar());
                }
                return btnGerar;
            }
        }

        private void OnGerar()
        {
            string WIFI = "WIFI:T:";
            string FimWifi = WIFI;
            if (Data.Data.SttWifiType == 1)
            {
                FimWifi += "WPA;S:" + wifiNameDigit + ";P:" + wifiKeyDigit + ";;";
            }
            else
            {
                FimWifi += "nopass;S:" + wifiNameDigit + ";;;";
            }
            Gerador.With(FimWifi).GerarCodigo(Data.Data.Codigo, frame);
        }

        private void EnabledBtn()
        {
            if(Data.Data.SttWifiType == 0)
            {
                ButtonEnabled = !wifiNameDigit.Equals("");
            }
            else
            {
                ButtonEnabled = !wifiNameDigit.Equals("") && !wifiKeyDigit.Equals("");

            }
        }

        #region Gets

        public bool ButtonEnabled
        {
            get { return btnGerarEnabled; }
            private set
            {
                if(btnGerarEnabled != value)
                {
                    btnGerarEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string WifiKeyDigit
        {
            get { return wifiKeyDigit; }
            private set
            {
                if(wifiKeyDigit != value)
                {
                    wifiKeyDigit = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string WifiNameDigit
        {
            get { return wifiNameDigit; }
            private set
            {
                if(wifiNameDigit != value)
                {
                    wifiNameDigit = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string WifiPassWord
        {
            get { return Data.Data.SttWifiPassword; }
            private set
            {
                if (wifiPassWord != value)
                {
                    wifiPassWord = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string PlaceholderTextWifiKey
        {
            get { return placeholderTextWifiKey; }
            private set
            {
                if (placeholderTextWifiKey != value)
                {
                    placeholderTextWifiKey = value;
                    RaisePropertyChanged();
                }
            }
        }

        internal void ItemClick(WiFiNetworkDisplay wifi, ComboBox WifiSelector, TextBox WifiName, PasswordBox WifiKey)
        { 
            if (wifi.GetNetworkAuthenticationType == NetworkAuthenticationType.Open80211)
            {
                WifiSelector.SelectedIndex = 0;
                WifiNameDigit = wifi.Ssid; 
                WifiKeyDigit = "";
            }
            else
            {
                WifiSelector.SelectedIndex = 1;
                WifiNameDigit = wifi.Ssid;
                WifiKeyDigit = "";
                WifiKey.Focus(FocusState.Keyboard);
            }
        }

        public string PlaceholderTextWifiName
        {
            get { return placeholderTextWifiName; }
            private set
            {
                if (placeholderTextWifiName != value)
                {
                    placeholderTextWifiName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string WifiDisponivellayout
        {
            get { return wifiDisponivellayout; }
            private set
            {
                if(wifiDisponivellayout != null)
                {
                    wifiDisponivellayout = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<WiFiNetworkDisplay> ResultsListView
        {
            get
            {
                return ResultCollection;
            }
            private set
            {
                if(ResultCollection != value)
                {
                    ResultCollection = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand WifiScanCommand
        {
            get
            {
                if(wifiScanCommand == null)
                {
                    wifiScanCommand = new RelayCommand(p => ScanWifi());
                }
                return wifiScanCommand;
            }

        }

        public int WifiType { get { return Data.Data.SttWifiType; } }

        public string WifiDisponivelPrglayout
        {
            get { return wifiDisponivelPrglayout; }
            private set
            {
                if(wifiDisponivelPrglayout != value)
                {
                    wifiDisponivelPrglayout = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        public ObservableCollection<WiFiNetworkDisplay> ResultCollection
        {
            get;
            private set;
        }

        public WifiControlViewModel()
        {
            WifiDisponivelPrglayout = "False";
            UpdateWifiDados();
        }

        private async void UpdateWifiDados()
        {
            ResultCollection = new ObservableCollection<WiFiNetworkDisplay>();
            var access = await WiFiAdapter.RequestAccessAsync();
            if (access != WiFiAccessStatus.Allowed)
            {
                NotifyUser(loader.GetString("NoWifiNoAccess"));
            }
            else
            {

                var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                if (result.Count >= 1)
                {
                    firstAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);

                }
                else
                {
                    NotifyUser(loader.GetString("NoWifiAvailable"));
                }
            }
        }

        private async void DisplayNetworkReport(WiFiNetworkReport report)
        {

            ResultCollection.Clear();

            foreach (var network in report.AvailableNetworks)
            {
                var networkDisplay = new WiFiNetworkDisplay(network, firstAdapter);
                await networkDisplay.UpdateConnectivityLevel();
                ResultCollection.Add(networkDisplay);

            }
            WifiDisponivelPrglayout = "False";
            if (ResultCollection.Count >= 1)
            {
                WifiDisponivellayout = "Visible";
                ResultsListView = ResultCollection;
            }
            else
            {
                NotifyUser(loader.GetString("NoWifiAvailable"));
            }

        }

        private async void ScanWifi()
        {
            try
            {
                WifiDisponivellayout = "Collapsed";
                WifiDisponivelPrglayout = "True";
                await firstAdapter.ScanAsync();
                DisplayNetworkReport(firstAdapter.NetworkReport);
            }
            catch
            {
                WifiDisponivelPrglayout = "False";
                NotifyUser(loader.GetString("NoWifiAvailableNotFunction"));

            }

        }

        internal void SetCompomentes(ComboBox wifiSelector, TextBox wifiName, PasswordBox wifiKey, Frame frame)
        {
            this.frame = frame;
             wifiSelector.SelectionChanged += (sender, args) =>
            {
                Data.Data.SttWifiType = wifiSelector.SelectedIndex;
                WifiPassWord = Data.Data.SttWifiPassword;
                EnabledBtn();
            };

            wifiName.TextChanged += (sender, args) =>
            {
                wifiNameDigit = wifiName.Text;
                if (!wifiName.Text.Equals(""))
                {
                    PlaceholderTextWifiName = wifiName.PlaceholderText;
                }
                else
                {
                    PlaceholderTextWifiName = "";
                }
                EnabledBtn();
            };

            wifiKey.PasswordChanged += (sender, args) =>
            {
                wifiKeyDigit = wifiKey.Password;
                if (!wifiKey.Password.Equals(""))
                { 
                    PlaceholderTextWifiKey = wifiKey.PlaceholderText;
                }
                else
                {
                    PlaceholderTextWifiKey = "";
                }
                EnabledBtn();
            };
            EnabledBtn();
        }

        private void NotifyUser(string v)
        {
            Paginas.Root.RootApp.Instance.GetToast(v);
        }
    }
}
