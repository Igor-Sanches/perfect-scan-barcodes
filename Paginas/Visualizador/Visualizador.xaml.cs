using Perfect_Scan.Database.Sql;
using System; 
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls; 
using Windows.UI.Xaml.Navigation; 
using Windows.UI.Popups;
using Firebase.Database.Query;
using System.Net.NetworkInformation;
using Perfect_Scan.Database;
using Perfect_Scan.ViewModel;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Perfect_Scan.Paginas.Visualizador
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Visualizador : Page
    {
        private VisualizadorViewModel viewModel;

        public Visualizador()
        {
            this.InitializeComponent();
            viewModel = ViewModelDispatcher.VisualizadorView;
            DataContext = viewModel;
          
        }
        private async void Toast(string v)
        {
            await new MessageDialog(v).ShowAsync();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

           

            }
            catch (Exception x)
            {
                Toast(x.Message);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter != null)
            {
                if(e.Parameter == e.Parameter as Gerados)
                {
                    Gerados gerado = e.Parameter as Gerados;
                    var control = viewModel.PutHistorico(gerado);
                    switch (control)
                    {
                        case 1:
                            {
                                ContoleUser.Content = new Controle.Visualizador.WifiControlVisualizador();
                            }
                            break;
                        case 2:
                            {
                                ContoleUser.Content = new Controle.Visualizador.NumeroControlVisualizador();
                            }
                            break;
                        case 3:
                            {
                                ContoleUser.Content = new Controle.Visualizador.ContatosControlVisualizador();
                            }
                            break;

                    }
                }
            }
        }

        private void Scroll_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            
        }
    }
}
