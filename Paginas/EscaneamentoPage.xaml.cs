using Perfect_Scan.Manager;
using Perfect_Scan.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Devices.Enumeration;
using Windows.Devices.Lights;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZXing;
using ZXing.Mobile;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Perfect_Scan.Paginas
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class EscaneamentoPage : Page
    {
        private UIElement customOverlayElement = null;
        private MobileBarcodeScanner _scanner;
        private ZXing.Net.Mobile.Forms.ZXingScannerView scanner;
        private MobileBarcodeScanningOptions options;
        private Timer myTimer;
        private ResourceLoader loader = new ResourceLoader();
        private Settings.Settings settings;
        private Assistente assistente;
        private bool isExterno = false;

        public EscaneamentoPage()
        {
            this.InitializeComponent();
            settings = new Settings.Settings();
            assistente = new Assistente(mediaPlayer);
            myTimer = new Timer(); 
            scanner = new ZXing.Net.Mobile.Forms.ZXingScannerView();
            options = new MobileBarcodeScanningOptions();
           _scanner = new MobileBarcodeScanner(this.Dispatcher);
           _scanner.Dispatcher = this.Dispatcher;
            IconeScan();
            IsFlashEnabled();
            Iniciar();

            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Data.Data.AppViewPager = "Escanear";
            if(e.Parameter != null)
            {
                isExterno = true;
                
            }
        }


        public void IconeScan()
        {
            Invert.Glyph = settings.IsScanInverso ? "" : "";

            Massa.Glyph = settings.IsScanEmMassa ? "" : "";
          
        }
        private void MyTimer_TimerTicked(object sender, TimerEventArgs e)
        { 
            Linha.Opacity = 0 + myTimer.TimeLeft.Seconds; 
        }

        private void MyTimer_TimerEnded(object sender, TimerEventArgs e)
        {
            myTimer.ResetTimer();
            myTimer.StartTimer();

        }
        private async void Iniciar()
        {
            try
            { 
                var cameraDeviceFront = await FindCameraDeviceByPanelAsync(Windows.Devices.Enumeration.Panel.Front);
                var cameraDeviceBack = await FindCameraDeviceByPanelAsync(Windows.Devices.Enumeration.Panel.Back);

                if(cameraDeviceBack != null || cameraDeviceFront != null)
                {
                    SeNHaCam.Visibility = Visibility.Collapsed;
                    SeHaCam.Visibility = Visibility.Visible;
                    if (customOverlayElement == null)
                    {
                        customOverlayElement = this.customOverlay.Children[0];
                        this.customOverlay.Children.RemoveAt(0);
                    }
                    myTimer.TimerEnded += MyTimer_TimerEnded;
                    myTimer.TimerTicked += MyTimer_TimerTicked;
                    myTimer.StartTimer();
                    //Wireup our buttons from the custom overlay
                    this.buttonCancel.Click += (s, e2) =>
                    {
                        _scanner.Cancel();
                        {
                            try
                            {
                                if (Frame.CanGoBack)
                                {
                                    Frame.GoBack();
                                }

                            }
                            catch { }
                        }

                    };
                    this.buttonFlash.Click += (s, e2) =>
                    {
                        _scanner.ToggleTorch();
                    };

                    //Set our custom overlay and enable it
                    _scanner.CustomOverlay = customOverlayElement;
                    _scanner.UseCustomOverlay = true; 
                    //<===Varreduras contínua Entre os scans===>\\
                    options.DelayBetweenContinuousScans = 2000;
                    //<===Auto rolar===>\\
                    options.AutoRotate = false;
                    //<===Tentar invertida===>\\
                    options.TryInverted = settings.IsScanInverso;
                    //<===Desativar auto focus===>\\
                    options.DisableAutofocus = !settings.IsUseAutoFocus;
                    //<===Se for celular não aceita câmera flontal===>\\ 
                    options.UseFrontCameraIfAvailable = settings.IsUseFrontalCamAvainable;
                    //<===Iniciar scaneamento===>\\ 
                    _scanner.ScanContinuously(options, result =>
                    {
                        if (result != null)
                        {
                            if (!isExterno)
                            {
                                if (settings.IsScanEmMassa)
                                {
                                    if (!settings.IsFalarResultadoAoEscanear || !settings.IsFalarResultadoAoEscanearMassa)
                                    {
                                        if (settings.IsBeepScan)
                                        {
                                            mediaPlayer.Play();
                                        }
                                    }
                                    HistoricoManager.Escaneado(result);
                                }
                                else
                                {
                                    if (settings.IsBeepScan)
                                    {
                                        mediaPlayer.Play();
                                    }
                                    GetToast(loader.GetString("salvo"));
                                    HistoricoManager.Escaneado(result);
                                }
                            }
                            else
                            {
                                string[] contents = new string[] { result.Text, result.BarcodeFormat.ToString() };
                                Frame.Navigate(typeof(EscaneamentoPage), contents);
                            }
                        }
                    });
                }
                else
                {
                    SeHaCam.Visibility = Visibility.Collapsed;
                    SeNHaCam.Visibility = Visibility.Visible;
                }


            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        public void GetToast(string msg) => Tools.Toast.MakerView(Toast, ToastText, ToastIcon, ModoColor.None, msg).Show();

        private static async Task<DeviceInformation> FindCameraDeviceByPanelAsync(Windows.Devices.Enumeration.Panel desiredPanel)
        {
            // Get available devices for capturing pictures
            var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            // Get the desired camera by panel
            DeviceInformation desiredDevice = allVideoDevices.FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == desiredPanel);

            // If there is no device mounted on the desired panel, return the first device found
            return desiredDevice ?? allVideoDevices.FirstOrDefault();
        }
        public async void IsFlashEnabled()
        {

            using (var lamp = await Lamp.GetDefaultAsync())
            {
                if (lamp == null)
                {
                    buttonFlash.Visibility = Visibility.Collapsed; 
                }
                else
                {
                    buttonFlash.Visibility = Visibility.Visible; 
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                settings.IsScanEmMassa = !settings.IsScanEmMassa;
                IconeScan();
                Root.RootApp.Instance.GetToast($"{loader.GetString("MassaModeState")} {GetONOff(settings.IsScanEmMassa)}", ModoColor.None);

            }
            catch { }
        }

        private string GetONOff(bool value)
        {
            return value ? loader.GetString("on") : loader.GetString("off");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _scanner.Cancel();
            {
                try
                {
                    settings.IsScanInverso = !settings.IsScanInverso;
                    Iniciar();
                    IconeScan();
                    Root.RootApp.Instance.GetToast($"{loader.GetString("InversoModeState")} {GetONOff(settings.IsScanInverso)}", ModoColor.None);

                }
                catch { }
            }
        }
    }
}
