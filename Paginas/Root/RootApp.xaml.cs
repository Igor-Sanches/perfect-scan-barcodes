using Perfect_Scan.Helpers;
using Perfect_Scan.Tools;
using Perfect_Scan.ViewModel;
using Perfect_Scan_Mobile.Tools;
using System;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources; 
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls; 
using Windows.UI.Xaml.Input; 
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Perfect_Scan.Paginas.Root
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class RootApp : Page
    { 
        public TitleBarHelper TitleHelper => TitleBarHelper.Instance;
        private ResourceLoader loader = new ResourceLoader(); 
        private static RootApp Instance_;
        private long current = 0;
        public static RootApp Instance => Instance_; 

        private RootViewModel vm;
        public RootApp()
        {
            this.InitializeComponent();
            vm = ViewModelDispatcher.RootView;
            DataContext = vm;
            //Atirar o frame a ViewModel
            vm.Shot(_frame);
            Instance_ = this;

            //Acionar o butão voltar
            navview.BackRequested += (sender, args) => { GoBack(); };
            
        }

        #region Butão volta
        public void GoBack()
        {
            try
            {
                if (_frame.CanGoBack)
                {
                    _frame.GoBack();
                }
                else
                {
                    if (TimerUtils.CurrentTimeMillis() - current > 2000)
                    {
                        current = TimerUtils.CurrentTimeMillis();
                        GetToast(loader.GetString("clickExit"), ModoColor.None);
                    }
                    else
                    {
                        App.Current.Exit();
                    }
                }
            }
            catch { }
        }

        public void GoBack(bool value)
        {
            try
            {
                if (_frame.CanGoBack)
                {
                    _frame.GoBack();
                }
                else
                {
                    _frame.Navigate(typeof(Inicio));
                }
            }
            catch { _frame.Navigate(typeof(Inicio)); }
        }
        #endregion

        #region Notificações Toast
        public void GetToast(string msg, ModoColor color) => Tools.Toast.MakerView(Toast, ToastText, ToastIcon, color, msg).Show();

        public void GetToast(string msg, ModoColor color, int _timer) => Tools.Toast.MakerView(Toast, ToastText, ToastIcon, color, msg, _timer).Show();

        public void GetToast(string msg) => Tools.Toast.MakerView(Toast, ToastText, ToastIcon, ModoColor.None, msg).Show();
        #endregion

        #region Navegalçao
        //Verificar quando a frame termina na navegação
        private void _frame_Navigated(object sender, NavigationEventArgs e)
        {
            try
            {
                if (PageIniciar.Width <= 1008)
                {
                    navview.IsPaneOpen = false;
                }
                switch (e.SourcePageType)
                {
                    case Type c when e.SourcePageType == typeof(Visualizador.Visualizador): 
                        navview.Header = "";// MyInicio.Content;
                        break;
                    case Type c when e.SourcePageType == typeof(ContaView):
                        ((NavigationViewItem)navview.MenuItems[0]).IsSelected = true;
                        navview.Header = "";// MyInicio.Content;
                        break;
                    case Type c when e.SourcePageType == typeof(Inicio):
                        ((NavigationViewItem)navview.MenuItems[1]).IsSelected = true;
                        navview.Header = MyInicio.Content;
                        break;
                    case Type c when e.SourcePageType == typeof(EscaneamentoPage):
                        ((NavigationViewItem)navview.MenuItems[2]).IsSelected = true;
                        navview.Header = MyEscanear.Content;
                        break;
                    case Type c when e.SourcePageType == typeof(EscanearImagem):
                        ((NavigationViewItem)navview.MenuItems[3]).IsSelected = true;
                        navview.Header = MyEscanearImagem.Content;
                        break;
                    case Type c when e.SourcePageType == typeof(GerarCodigos):
                        ((NavigationViewItem)navview.MenuItems[4]).IsSelected = true;
                        navview.Header = "";
                        break;
                    case Type c when e.SourcePageType == typeof(HistoricosPage):
                        ((NavigationViewItem)navview.MenuItems[5]).IsSelected = true;
                        navview.Header = MyHistoricos.Content;
                        break;
                    case Type c when e.SourcePageType == typeof(Configuracoes):
                        navview.Header = loader.GetString("settingsFor");
                        ((NavigationViewItem)navview.MenuItems[0]).IsSelected = true;
                        break;
                    case Type c when e.SourcePageType == typeof(Backups.BackupViewPage):
                        navview.Header = loader.GetString("Backups");
                        ((NavigationViewItem)navview.MenuItems[0]).IsSelected = true;
                        break; 
                    case Type c when e.SourcePageType == typeof(Backups.CriarBackupsView):
                        navview.Header = loader.GetString("Backups");
                        ((NavigationViewItem)navview.MenuItems[0]).IsSelected = true;
                        break;
                    case Type c when e.SourcePageType == typeof(Backups.BackupViewInfo):
                        navview.Header = loader.GetString("Backups");
                        ((NavigationViewItem)navview.MenuItems[0]).IsSelected = true;
                        break;
                    case Type c when e.SourcePageType == typeof(InfoAppsPlataformas):
                        navview.Header = "";
                        ((NavigationViewItem)navview.MenuItems[0]).IsSelected = true;
                        break;
                    case Type c when e.SourcePageType == typeof(Sobre):
                        navview.Header = loader.GetString("SobreApp");
                        ((NavigationViewItem)navview.MenuItems[0]).IsSelected = true;
                        break;
                }

                //Verificar o estado do butão volta depois de navegar para a frame
                BtnBackState();
            }
            catch { }
        }

        private void BtnBackState()
        {
            //Verificar se há frames para trás, se houver o butão voltar ficara disponivel para click
            navview.IsBackEnabled = _frame.CanGoBack || Frame.CanGoBack; ;
        }
         
        private void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NavigationViewItem item = (NavigationViewItem)sender as NavigationViewItem;
            string tag = item.Tag.ToString();
           //Navegar para outras frames
            vm.NavigationFrame(tag); 
    } 

        private void PageIniciar_Loaded(object sender, RoutedEventArgs e)
        {
            vm.NavigationFrameFromHome();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Verificar os arquivos de suporte
            base.OnNavigatedTo(e);
            vm.OnNavigationTo(e);
             
        }
        #endregion

    }
}
