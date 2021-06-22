using Perfect_Scan.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class ContaView : Page
    {
        private ContaViewModel vm;

        public ContaView()
        {
            this.InitializeComponent();
            vm = ViewModelDispatcher.ContaView;
            DataContext = vm;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Data.Data.AppViewPager = "Account";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void NuvemConected_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggle = (ToggleSwitch)sender as ToggleSwitch;
            Data.Conta.PutSettings(toggle.Tag.ToString(), toggle.IsOn);
        }
         
    }
}
