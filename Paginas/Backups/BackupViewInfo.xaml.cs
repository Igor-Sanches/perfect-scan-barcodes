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
    public sealed partial class BackupViewInfo : Page
    {
        private BackupInfoViewModel vm;
        public BackupViewInfo()
        {
            this.InitializeComponent();
            vm = ViewModel.ViewModelDispatcher.BackupInfoView;
            DataContext = vm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Data.Data.AppViewPager = "BackupLocalCriar";
            base.OnNavigatedTo(e);
            try
            {
                var args = e.Parameter as string;
                vm.ShotFileBackup(args);
                 
            }
            catch { }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = DialogEdit;
            vm.Shot(dialog, EditName);

        }
    }
}
