using Perfect_Scan.Paginas;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;

namespace Perfect_Scan.Manager
{
    public class WiFiNetworkDisplay : INotifyPropertyChanged
    {
        private WiFiAdapter adapter;
        private ResourceLoader loader = new ResourceLoader();
        public WiFiNetworkDisplay(WiFiAvailableNetwork availableNetwork, WiFiAdapter adapter)
        {
            AvailableNetwork = availableNetwork;
            this.adapter = adapter; 
        }

        public WiFiNetworkDisplay()
        {
        }
         

        public async Task UpdateConnectivityLevel()
        {
            string connectivityLevel = "Not Connected";
            string connectedSsid = null;

            var connectedProfile = await adapter.NetworkAdapter.GetConnectedProfileAsync();
            if (connectedProfile != null &&
                connectedProfile.IsWlanConnectionProfile &&
                connectedProfile.WlanConnectionProfileDetails != null)
            {
                connectedSsid = connectedProfile.WlanConnectionProfileDetails.GetConnectedSsid();
            }

            if (!string.IsNullOrEmpty(connectedSsid))
            {
                if (connectedSsid.Equals(AvailableNetwork.Ssid))
                {
                    connectivityLevel = connectedProfile.GetNetworkConnectivityLevel().ToString();
                }
            }

            ConnectivityLevel = connectivityLevel;

            OnPropertyChanged("ConnectivityLevel");
        }

        public String Ssid
        {
            get
            {
                return availableNetwork.Ssid;
            }
        }
         
          
        public String ConnectivityLevel
        {
            get;
            private set;
        }

        public NetworkAuthenticationType GetNetworkAuthenticationType
        {
            get
            {
                return AvailableNetwork.SecuritySettings.NetworkAuthenticationType;
            }
        }

        public String SecuritySettings
        {
            get
            {
                if (AvailableNetwork.SecuritySettings.NetworkAuthenticationType == NetworkAuthenticationType.Open80211)
                {
                    return loader.GetString("Aberta");
                }
                else { return loader.GetString("Segura"); }
            }
        }


        private WiFiAvailableNetwork availableNetwork;
        public WiFiAvailableNetwork AvailableNetwork
        {
            get
            {
                return availableNetwork;
            }

            private set
            {
                availableNetwork = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected async void OnPropertyChanged(string name)
        {
            Paginas.Root.RootApp gerar = Paginas.Root.RootApp.Instance;
            await gerar.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            });
        }
    }
}