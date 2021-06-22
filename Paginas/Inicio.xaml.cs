using Perfect_Scan.Manager;
using Perfect_Scan.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.Management.Deployment;
using Windows.System;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Perfect_Scan.Paginas
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Inicio : Page
    {
        private ResourceLoader loader = new ResourceLoader();

        private RootViewModel vm;
        public Inicio()
        {
            this.InitializeComponent();
            vm = ViewModelDispatcher.RootView;
            DataContext = vm; 
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Adicionar e lebrar que esta pagina esta em uso e não poderar ser selecionada novamente
            Data.Data.AppViewPager = "Inicio";
         } 

        private void SheetDialog_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Desativar o painel flutuante
            vm.SheetPanel(false);
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            //Ativar o painel flutuante
            vm.SheetPanel(true);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Carregar o papel de parede
            vm.loadBackground();
        }
         
    }
         
}
