using Perfect_Scan.Database.Sql;
using Perfect_Scan.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Perfect_Scan.Paginas
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class HistoricosPage : Page
    {
        private ResourceLoader loader = new ResourceLoader();
        private Regex regex = new Regex("[0-9-a-z-A-Z-ãçÃÕÍÓÉéõ íó-]+");
        private HistoricosViewModel vm;

        public HistoricosPage()
        {
            this.InitializeComponent();
            Application.Current.Resuming += new EventHandler<Object>(Current_Resuming);
            vm = ViewModelDispatcher.HistoricosViewModel;
            DataContext = vm;
        }

        /// <summary>
        /// Ensures the page is refreshed on resume
        /// as content could have changed while away.
        /// </summary>
        private void Current_Resuming(object sender, object e)
        {
            int currentTabIndex = Paginas.SelectedIndex; 
        }

        private void Pivot_PivotItemLoading(Pivot sender, PivotItemEventArgs args)
        {
            int currentTab = Paginas.SelectedIndex;
             

            UserControl currentPainel;
            switch (currentTab)
            {
                case 0:
                    {
                        currentPainel = new Controle.Historicos.EscaneadosView(Sorting, Delete, Paginas);
                        currentPainel.Height = Double.NaN;
                        args.Item.Content = currentPainel; 
                    }
                    break;
                case 1:
                    {
                        currentPainel = new Controle.Historicos.GeradosView(Sorting, Delete, Paginas, Frame);
                        currentPainel.Height = Double.NaN;
                        args.Item.Content = currentPainel;
                    }
                    break;
            }
             
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e); 
            Data.Data.AppViewPager = "Historicos";
            Paginas.SelectedIndex = Data.Data.SetHistoricoPagina;
            vm.OnNavigation(true); 
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            vm.OnNavigation(false);
        }
         
        private void Paginas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Data.Data.SetHistoricoPagina = Paginas.SelectedIndex;
        }
    }
}
