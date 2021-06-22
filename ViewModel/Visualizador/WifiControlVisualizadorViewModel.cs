using Perfect_Scan.Database.Sql;
using Perfect_Scan.Manager;
using Perfect_Scan.Manager.ResultScanner;
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
using Windows.Security.Credentials;
using Windows.UI.Popups;

namespace Perfect_Scan.ViewModel.Visualizador
{
    public class WifiControlVisualizadorViewModel : ViewModelBase
    {
        private ResourceLoader loader = new ResourceLoader();
        private RelayCommand checkCommand, connectCommand;
        private WifiResult wifi;
        private bool isEnablled = true;
        private bool isEnablledProgress = false;
        private WiFiAdapter firstAdapter;
        private bool isProgress = true, codigoView = false;
        private string senhaFilter, senhaVisible, gridView = "Collapsed", wifiProtected = "Collapsed", displayName = "", segurity = "", senha = "";
        
        public bool IsEnabled
        {
            get { return isEnablled; }
            private set
            {
                if(isEnablled!= value)
                {
                    isEnablled = value;
                    RaisePropertyChanged();
                }
            }

        }

        public bool IsEnabledProgress
        {
            get { return isEnablledProgress; }
            private set
            {
                if (isEnablledProgress != value)
                {
                    isEnablledProgress = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void PutProgressConnect(bool b)
        {
            IsEnabled = !b;
            IsEnabledProgress = b;
        }

        internal void PutWIFI(WifiResult wifi)
        {
            try
            {
                CarregarDados();
                this.wifi = wifi;
                Displayname = wifi.SSID;
                if (wifi.IsSegurity)
                {
                    LayoutWifiProtected = "Visible";
                    Segurity = loader.GetString("Segura");
                    senhaFilter = FilterPassword(wifi.Senha); ;
                    senhaVisible = wifi.Senha;
                    Senha = senhaFilter;
                }
                else
                {
                    Segurity = loader.GetString("Aberta");
                    LayoutWifiProtected = "Collapsed";
                }
                PuProgress(true);
            }
            catch
            {

            }
        }

        public ICommand Connect
        {

            get
            {
                if(connectCommand == null)
                {
                    connectCommand = new RelayCommand(p => ConnectWIFI());
                }
                return connectCommand;
            }
        }

        public ICommand CheckSenhaView
        {
            get
            {
                if(checkCommand == null)
                {
                    checkCommand = new RelayCommand(p => OnViewSenha());
                }
                return checkCommand;
            }
        }

        private void OnViewSenha()
        {
            IsCodigoView = !IsCodigoView;
            Senha = codigoView ? senhaVisible : senhaFilter;
        }

        public bool IsCodigoView
        {
            get { return codigoView; }
            private set
            {
                if(codigoView != value)
                {
                    codigoView = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Senha
        {
            get { return senha; }
            private set
            {
                     if (senha != null)
                    {
                        senha = value;
                        RaisePropertyChanged();
                    }

             }
        }

        public string Segurity
        {
            get { return segurity; }
            private set
            {
                     if (segurity != null)
                    {
                        segurity = value;
                        RaisePropertyChanged();
                    }
                     
            }
        }

        public string LayoutWifiProtected
        {
            get { return wifiProtected; }
            private set
            {
                
                    if (wifiProtected != null)
                    {
                        wifiProtected = value;
                        RaisePropertyChanged();
                    }
                }
        }

        public string Displayname
        {
            get { return displayName; }
            private set
            {
                if (displayName != null)
                {
                    displayName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string GridViewLayoutWIFI
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

        public bool IsProgressWIFI
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
                IsProgressWIFI = false;
                GridViewLayoutWIFI = "Visible";
            }
            else
            {
                IsProgressWIFI = true;
                GridViewLayoutWIFI = "Collapsed";
            }
        }

        private string FilterPassword(string senha)
        {
            string filter = "";
            for(int i = 0; i < senha.Length; i++)
            {
                filter += "*";
            }
            return filter;
        }
         
        public ObservableCollection<WiFiNetworkDisplay> ResultCollection
        {
            get;
            private set;
        }

        private void Toast(string msg, int index)
        {
            Tools.ModoColor color = Tools.ModoColor.None;
            if(index == 0)
            {
                color = Tools.ModoColor.Succes;
            }
            else if(index == 1) { color = Tools.ModoColor.Error; }


            Paginas.Root.RootApp.Instance.GetToast(msg, color);
        }

        private async void CarregarDados()
        {
            try
            {
                ResultCollection = new ObservableCollection<WiFiNetworkDisplay>();
                 
                var access = await WiFiAdapter.RequestAccessAsync();
                if (access != WiFiAccessStatus.Allowed)
                {
                    Toast(loader.GetString("errorAccessWIFI"), 1);
                }
                else
                { 

                    var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                    if (result.Count >= 1)
                    { 
                        firstAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
                         
                    } 
                }
            }
            catch
            {

            }
        } 

        private async void ConnectWIFI()
        {
            try
            {
                PutProgressConnect(true);
                await firstAdapter.ScanAsync(); 

                DisplayNetworkReport(firstAdapter.NetworkReport);

            }
            catch { }
        }

        private async void DisplayNetworkReport(WiFiNetworkReport networkReport)
        {
            try
            {
                ResultCollection.Clear();

                foreach (var network in networkReport.AvailableNetworks)
                {
                    var networkDisplay = new WiFiNetworkDisplay(network, firstAdapter);
                    await networkDisplay.UpdateConnectivityLevel();
                    ResultCollection.Add(networkDisplay);
                }
                foreach (var red in ResultCollection)
                {
                    String ssid2 = red.Ssid;
                    if (wifi.SSID.Equals(ssid2))
                    {

                        if (red == null || firstAdapter == null)
                        {
                            Toast(loader.GetString("redeNoAvailable"), 1);
                            return;
                        }
                        WiFiReconnectionKind reconnectionKind = WiFiReconnectionKind.Manual;
                        reconnectionKind = WiFiReconnectionKind.Automatic;
                         

                        WiFiConnectionResult result;
                        if (!wifi.IsSegurity)
                        {
                            result = await firstAdapter.ConnectAsync(red.AvailableNetwork, reconnectionKind);
                        }
                        else
                        {
                            var credential = new PasswordCredential();

                            credential.Password = wifi.Senha;


                            result = await firstAdapter.ConnectAsync(red.AvailableNetwork, reconnectionKind, credential);
                        }

                        if (result.ConnectionStatus == WiFiConnectionStatus.Success)
                        {
                            PutProgressConnect(false);
                            Toast($"{loader.GetString("redeConnected")} {wifi.SSID}", 0); 
                        }
                        else
                        {
                            PutProgressConnect(false);
                            Toast($"{loader.GetString("redeFalhaConnected")} {wifi.SSID}", 1);
                        }

                    }
                    else
                    {
                        Toast(loader.GetString("redeSelecteNot"), 1);
                    }
                }
            }
            catch { }
        }

        private async void MessageBox(string message)
        {
            await new MessageDialog(message).ShowAsync();
        }
    }

}
