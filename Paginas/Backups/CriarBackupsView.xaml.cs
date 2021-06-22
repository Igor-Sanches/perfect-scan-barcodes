using Perfect_Scan.ViewModel;
using Perfect_Scan.ViewModel.Backups;
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

namespace Perfect_Scan.Paginas.Backups
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class CriarBackupsView : Page
    {
        private BackupsCreateViewModel vm;

        public CriarBackupsView()
        {
            this.InitializeComponent();
            vm = ViewModelDispatcher.BackupsCreateView;
            DataContext = vm;
        }
         
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Data.Data.AppViewPager = "BackupLocalView";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            vm.Shot(EditName);
        }
    }
}
