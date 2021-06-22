using Perfect_Scan.Manager;
using Perfect_Scan.Models;
using Perfect_Scan.Tools;
using Perfect_Scan.ViewModel;
using System; 
using Windows.ApplicationModel.Resources; 
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls; 
using Windows.UI.Xaml.Navigation; 

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Perfect_Scan.Paginas
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>.
    public sealed partial class GerarCodigos : Page
    {
        private GeradorViewModel vm;
        private ResourceLoader loader = new ResourceLoader();
        private static GerarCodigos instance;
        string[] protocol;
        public static GerarCodigos Instance { get { return instance; } }

        public Strings StringKey = new Strings(); 
        public GerarCodigos()
        {
            this.InitializeComponent();
            vm = ViewModelDispatcher.GeradorView;
            DataContext = vm;
            instance = this;
            Application.Current.Resuming += new EventHandler<Object>(Current_Resuming);
        }

        /// <summary>
        /// Ensures the page is refreshed on resume
        /// as content could have changed while away.
        /// </summary>
        private void Current_Resuming(object sender, object e)
        {
            int currentTabIndex = Paginas.SelectedIndex; 
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (MyPage.ActualWidth <= 600)
            {
                Toolbar.Margin = new Thickness(100, -40, 0, 0);

            }
            else
            {
                Toolbar.Margin = new Thickness(0, -50, 0, 0);
            }
            Data.Data.AppViewPager = "Gerar";
            

            GetPositionComboBox();


            protocol = e.Parameter as string[];
            if(protocol == null)
            {
                protocol = new string[] { "2", "" };
            }
            else
            {
                if(protocol[0] == "1")
                {
                    Paginas.SelectedIndex = 3;
                }
            }
            
        }


        public void GetPositionComboBox()
        {

            for (int i = 0; i < Codigos.Items.Count; i++)
            {
                ((ComboBoxItem)Codigos.Items[i]).IsEnabled = true;
            }

            switch (Paginas.SelectedIndex)
            {
                case 1:
                case 4:
                    {
                        ((ComboBoxItem)Codigos.Items[1]).IsEnabled = false;
                        ((ComboBoxItem)Codigos.Items[2]).IsEnabled = false;
                        ((ComboBoxItem)Codigos.Items[3]).IsEnabled = false;
                        ((ComboBoxItem)Codigos.Items[4]).IsEnabled = false;
                        ((ComboBoxItem)Codigos.Items[6]).IsEnabled = false;
                        ((ComboBoxItem)Codigos.Items[7]).IsEnabled = false;
                        ((ComboBoxItem)Codigos.Items[8]).IsEnabled = false;
                        ((ComboBoxItem)Codigos.Items[11]).IsEnabled = false;
                        ((ComboBoxItem)Codigos.Items[12]).IsEnabled = false;

                    }
                    break;
                case 2:
                    {
                        if (Data.Data.IsProduto)
                        {
                            ((ComboBoxItem)Codigos.Items[0]).IsEnabled = false;
                            ((ComboBoxItem)Codigos.Items[1]).IsEnabled = false;
                            ((ComboBoxItem)Codigos.Items[2]).IsEnabled = false;
                            ((ComboBoxItem)Codigos.Items[3]).IsEnabled = false;
                            ((ComboBoxItem)Codigos.Items[4]).IsEnabled = false;
                            ((ComboBoxItem)Codigos.Items[5]).IsEnabled = false;
                            ((ComboBoxItem)Codigos.Items[8]).IsEnabled = false;
                            ((ComboBoxItem)Codigos.Items[9]).IsEnabled = false;
                            ((ComboBoxItem)Codigos.Items[10]).IsEnabled = false;
                            if (Codigos.SelectedIndex != 6 || Codigos.SelectedIndex != 7 || Codigos.SelectedIndex != 11 || Codigos.SelectedIndex != 12)
                            {
                                Codigos.SelectedIndex = 6;
                            }
                        }
                        else
                        {
                            ((ComboBoxItem)Codigos.Items[1]).IsEnabled = false;
                            ((ComboBoxItem)Codigos.Items[6]).IsEnabled = false;
                            ((ComboBoxItem)Codigos.Items[7]).IsEnabled = false;
                            ((ComboBoxItem)Codigos.Items[8]).IsEnabled = false;
                            ((ComboBoxItem)Codigos.Items[11]).IsEnabled = false;
                            ((ComboBoxItem)Codigos.Items[12]).IsEnabled = false;
                            if (Codigos.SelectedIndex == 6 || Codigos.SelectedIndex == 7 || Codigos.SelectedIndex == 11 || Codigos.SelectedIndex == 12)
                            {
                                Codigos.SelectedIndex = 10;
                            }
                        }
                    }
                    break;
                case 3:
                    {
                        ((ComboBoxItem)Codigos.Items[1]).IsEnabled = false;
                        ((ComboBoxItem)Codigos.Items[6]).IsEnabled = false;
                        ((ComboBoxItem)Codigos.Items[7]).IsEnabled = false;
                        ((ComboBoxItem)Codigos.Items[8]).IsEnabled = false;
                        ((ComboBoxItem)Codigos.Items[11]).IsEnabled = false;
                        ((ComboBoxItem)Codigos.Items[12]).IsEnabled = false;

                    }
                    break;
            }
            Codigos.SelectedIndex = Data.Data.GetPaginasIndex(Paginas.SelectedIndex);
        }

        private void MyPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (MyPage.ActualWidth <= 600)
            {
                Toolbar.Margin = new Thickness(100, -40, 0, 0);

            }
            else
            {
                Toolbar.Margin = new Thickness(0, -50, 0, 0);
            }
        }

        private void Codigos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Data.Data.Codigo = Codigos.SelectedIndex;
           Data.Data.SetPaginasIndex(Paginas == null ? 0 : Paginas.SelectedIndex, Codigos.SelectedIndex);
        }

        private void Paginas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            Data.Data.PaginaViewIndex = Paginas.SelectedIndex;
            GetPositionComboBox();
        }
          
        private void Paginas_PivotItemLoading(Pivot sender, PivotItemEventArgs args)
        {
            int currentTab = Paginas.SelectedIndex;
             
            UserControl currentPainel;
            switch (currentTab)
            {
                case 0:
                    {
                        currentPainel = new Controle.GeralEditor(protocol[0] == "0" ? protocol[1] : "", Frame);
                        currentPainel.Height = Double.NaN;
                        args.Item.Content = currentPainel;
                    }
                    break;
                case 1:
                    {
                        currentPainel = new Controle.WifiControlView(Frame);
                        currentPainel.Height = Double.NaN;
                        args.Item.Content = currentPainel;
                    }
                    break;
                case 2:
                    {
                        currentPainel = new Controle.DisplayProEncView(Frame);
                        currentPainel.Height = Double.NaN;
                        args.Item.Content = currentPainel;
                    }
                    break;
                case 3:
                    {
                        currentPainel = new Controle.DisplayNumeroView(protocol[0] == "1" ? protocol[1] : "", Frame);
                        currentPainel.Height = Double.NaN;
                        args.Item.Content = currentPainel;
                    }
                    break;
                case 4:
                    {
                        currentPainel = new Controle.Visualizador.ContatosControlVisualizador();//.GerarContatos(Frame);
                        currentPainel.Height = Double.NaN;
                        args.Item.Content = currentPainel;
                    }
                    break;
            }
        }

        private void Multi_Toggled(object sender, RoutedEventArgs e)
        {
            Data.Data.IsMulti = Multi.IsOn;
        }
    }
}
