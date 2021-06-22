using Firebase.Auth.Providers;
using Perfect_Scan.Manager;
using System; 
using System.Diagnostics; 
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core; 
using Windows.Storage;
using Windows.System; 
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls; 
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Perfect_Scan
{
    /// <summary>
    ///Fornece o comportamento específico do aplicativo para complementar a classe Application padrão.
    /// </summary>
    sealed partial class App : Application
    {
        private bool _isInBackgroundMode = false;
        private string _ApiKey = "AIzaSyDcCMeD-bOT1MzLjbb7GnKjTkhyp1tV9_Q";
        private string _AuthDomain = "perfectscan-9645f.firebaseapp.com";
        private string _database = "https://perfectscan-9645f.firebaseio.com/";
        private static App instance; 
        public static App GetInstance
        {

            get  
            {
                return instance;

            }
        }


        /// <summary>
        /// Inicializa o objeto singleton do aplicativo.  Esta é a primeira linha de código criado
        /// executado e, como tal, é o equivalente lógico de main() ou WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            instance = this;
            this.EnteredBackground += App_EnteredBackground;
            this.LeavingBackground += App_LeavingBackground;
            //App.Current.RequiresPointerMode = ApplicationRequiresPointerMode.WhenRequested;
            MemoryManager.AppMemoryUsageLimitChanging += MemoryManager_AppMemoryUsageLimitChanging;
            MemoryManager.AppMemoryUsageIncreased += MemoryManager_AppMemoryUsageIncreased;

            if (Data.Conta.IsLogging)
            {
                //new UpdateTask();
            }
        }
         
        private void MemoryManager_AppMemoryUsageIncreased(object sender, object e)
        {
            var level = MemoryManager.AppMemoryUsageLevel;

            if (level == AppMemoryUsageLevel.OverLimit || level == AppMemoryUsageLevel.High)
            {
                ReduceMemoryUsage(MemoryManager.AppMemoryUsageLimit);
            }

        }

        private void ReduceMemoryUsage(ulong newLimit)
        {
            try
            {
                if (_isInBackgroundMode && Window.Current != null && Window.Current.Content != null)
                {
                    ApplicationData.Current.LocalSettings.Values["navState"] = (Window.Current.Content as Frame).GetNavigationState();

                    var frame = Window.Current.Content as Frame;
                    if (frame != null)
                    {
                        //frame.NavigationFailed -= OnNavigationFailed;
                        //frame.Navigated -= RootFrame_Navigated;

                        //Window.Current.Content = null;
                    }

                    GC.Collect();
                }
            }
            catch { App.Current.Exit(); }
        }

        private void MemoryManager_AppMemoryUsageLimitChanging(object sender, Windows.System.AppMemoryUsageLimitChangingEventArgs e)
        {
            if (MemoryManager.AppMemoryUsage >= e.NewLimit)
            {
                ReduceMemoryUsage(e.NewLimit);
            }
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            //     throw new NotImplementedException();
        }
        private void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            Debug.WriteLine("Entered Background");
            var deferral = e.GetDeferral();
            try
            {
                _isInBackgroundMode = true;
#if DEBUG
                //If we are in debug mode free memory here because the memory limits are turned off
                //In release builds defer the actual reduction of memory to the limit changing event so we don't 
                //unnecessarily throw away the UI
                ReduceMemoryUsage(0);
#endif
            }
            finally
            {
                deferral.Complete();
            }
        }

        private void App_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            _isInBackgroundMode = false;

            // Restore view content if it was previously unloaded.
            if (Window.Current != null && Window.Current.Content == null)
            {
                CreateRootFrame(ApplicationExecutionState.Running, string.Empty, false);
            }

            Debug.WriteLine("Leaving Background");
        }

        private Firebase.Auth.FirebaseAuthConfig config
        {
            get
            {
                var config = new Firebase.Auth.FirebaseAuthConfig
                {
                    ApiKey = _ApiKey,
                    AuthDomain = _AuthDomain,
                    Providers = new FirebaseAuthProvider[]
                    {
                        new GoogleProvider().AddScopes("email"),
                        new EmailProvider()
                    } 

                };
                return config;
            }
        }

        protected override void OnActivated(IActivatedEventArgs args)
        { 
            base.OnActivated(args);
            if(args.Kind == ActivationKind.Protocol)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                if(GetClient.User != null)
                {
                    Toast("Usuário logado");
                }
                else
                {
                    Toast("Usuário nulo");
                }
                //if (Data.Conta.IsLogging)
               // {
                    if (rootFrame == null)
                    {
                        rootFrame = new Frame();
                        rootFrame.Navigate(typeof(Paginas.Root.RootApp), args);
                    }
                    else

                    {
                        rootFrame.Navigate(typeof(Paginas.Root.RootApp), args);
                    }
                    /*}
                    else
                    {
                        if (rootFrame == null)
                        {
                            rootFrame = new Frame();
                            rootFrame.Navigate(typeof(MainPage), args);
                        }
                        else
                        {
                            rootFrame.Navigate(typeof(MainPage), args);
                        }
                    }*/

                    Window.Current.Content = rootFrame;
                Window.Current.Activate();

            }
        }

        private async void Toast(string v)
        {
            await new MessageDialog(v).ShowAsync();
        }

        protected override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            base.OnShareTargetActivated(args);
            if(args != null)
            {
                if (args.Kind == ActivationKind.ShareTarget)
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                   
                    //if (Data.Conta.IsLogging)
                    //{
                        if (rootFrame == null)
                        {
                            rootFrame = new Frame();
                            rootFrame.Navigate(typeof(Paginas.Root.RootApp), args);
                        }
                        else
                        {
                            rootFrame.Navigate(typeof(Paginas.Root.RootApp), args);
                        }
                    /*}
                    else
                    { 
                    if (rootFrame == null)
                        {
                            rootFrame = new Frame();
                            rootFrame.Navigate(typeof(MainPage), args);
                        }
                        else
                        {
                            rootFrame.Navigate(typeof(MainPage), args);
                        }
                    }*/


                    Window.Current.Content = rootFrame;
                    Window.Current.Activate();

                }
            }
        }

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            try
            {
                var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = false;
                var view = ApplicationView.GetForCurrentView().TitleBar;
                view.BackgroundColor = ((SolidColorBrush)Current.Resources["Background"]).Color;
                view.ButtonBackgroundColor = ((SolidColorBrush)Current.Resources["Background"]).Color;
                view.ButtonForegroundColor = ((SolidColorBrush)Current.Resources["Texto"]).Color;
                view.ForegroundColor = ((SolidColorBrush)Current.Resources["Texto"]).Color;
                base.OnFileActivated(args);
                Frame rootFrame = Window.Current.Content as Frame;

                //if (Data.Conta.IsLogging)
                //{
                    if (rootFrame == null)
                    {
                        rootFrame = new Frame();
                        rootFrame.Navigate(typeof(Paginas.Root.RootApp), args);
                    }
                    else
                    {
                        rootFrame.Navigate(typeof(Paginas.Root.RootApp), args);
                    }
                    /*}
                    else
                    { 
                        if (rootFrame == null)
                        {
                            rootFrame = new Frame();
                            rootFrame.Navigate(typeof(MainPage), args);
                        }
                        else
                        {
                            rootFrame.Navigate(typeof(MainPage), args);
                        }
                    //}*/



                    Window.Current.Content = rootFrame;
                Window.Current.Activate();
            }
            catch { }

        }
         

        public Firebase.Database.FirebaseClient GetDatabase
        {
            get
            {
                return new Firebase.Database.FirebaseClient(_database,
                    new Firebase.Database.FirebaseOptions()
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult("SVIFbLGjwFuNfTUzR5dsrC4ZVGXKzIPajg9J4XyA")
                    });
            }
        }

        public Firebase.Auth.FirebaseAuthClient GetClient
        {
            get
            {  
                return new Firebase.Auth.FirebaseAuthClient(config);

            }
        }

        /// <summary>
        /// Chamado quando o aplicativo é iniciado normalmente pelo usuário final.  Outros pontos de entrada
        /// serão usados, por exemplo, quando o aplicativo for iniciado para abrir um arquivo específico.
        /// </summary>
        /// <param name="e">Detalhes sobre a solicitação e o processo de inicialização.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = false;
            var view = ApplicationView.GetForCurrentView().TitleBar;
            view.BackgroundColor = ((SolidColorBrush)Current.Resources["Background"]).Color;
            view.ButtonBackgroundColor = ((SolidColorBrush)Current.Resources["Background"]).Color;
            view.ButtonForegroundColor = ((SolidColorBrush)Current.Resources["Texto"]).Color;
            view.ForegroundColor = ((SolidColorBrush)Current.Resources["Texto"]).Color;
            CreateRootFrame(e.PreviousExecutionState, e.Arguments, e.PrelaunchActivated);
 }

        private void CreateRootFrame(ApplicationExecutionState previousExecutionState, string arguments, bool prelaunchActivated)
        {

            // Não repita a inicialização do aplicativo quando a Janela já tiver conteúdo,
            // apenas verifique se a janela está ativa
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                // Crie um Quadro para atuar como o contexto de navegação e navegue para a primeira página
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (previousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Carregue o estado do aplicativo suspenso anteriormente
                }

                // Coloque o quadro na Janela atual
                Window.Current.Content = rootFrame;
            }

            if (prelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Quando a pilha de navegação não for restaurada, navegar para a primeira página,
                    // configurando a nova página passando as informações necessárias como um parâmetro
                    // parâmetro
                    try
                    {
                       
                        
                            //if (Data.Conta.IsLogging)
                           // {
                                rootFrame.Navigate(typeof(Paginas.Root.RootApp), arguments); 
                           // }
                           // else
                           // {
                           //     rootFrame.Navigate(typeof(MainPage), arguments); 
                           // }
                        
                        
                }
                    catch (Exception x)
                    {
                        Message(x.Message);
                    }
                }
                // Verifique se a janela atual está ativa
                Window.Current.Activate();
            }
        }

        private async void Message(string message)
        {
            await new MessageDialog(message).ShowAsync();
        }

        /// <summary>
        /// Chamado quando ocorre uma falha na Navegação para uma determinada página
        /// </summary>
        /// <param name="sender">O Quadro com navegação com falha</param>
        /// <param name="e">Detalhes sobre a falha na navegação</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Chamado quando a execução do aplicativo está sendo suspensa.  O estado do aplicativo é salvo
        /// sem saber se o aplicativo será encerrado ou retomado com o conteúdo
        /// da memória ainda intacto.
        /// </summary>
        /// <param name="sender">A fonte da solicitação de suspensão.</param>
        /// <param name="e">Detalhes sobre a solicitação de suspensão.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Salvar o estado do aplicativo e parar qualquer atividade em segundo plano
            deferral.Complete();
        }  
    }

}
