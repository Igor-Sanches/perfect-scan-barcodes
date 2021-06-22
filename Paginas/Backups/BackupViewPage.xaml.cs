using Perfect_Scan.Models;
using Perfect_Scan.ViewModel.Backups;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Perfect_Scan.Paginas.Backups
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class BackupViewPage : Page
    {
        private string path = "", name = "";
        private BackupViewModel vm;

        public BackupViewPage()
        {
            this.InitializeComponent();
            vm = ViewModel.ViewModelDispatcher.BackupView;
            DataContext = vm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Data.Data.AppViewPager = "BackupLocalViews";
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        private IReadOnlyList<StorageFile> storageItems;
        private async void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            List<StorageFile> files = new List<StorageFile>();
            files.Add(await StorageFile.GetFileFromPathAsync(path));
            storageItems = files;
            DataPackage requestData = args.Request.Data;
            requestData.Properties.Title = name;
            requestData.Properties.Description = DateTime.Now.ToString();
            requestData.SetStorageItems(storageItems);
        }



        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ContentDialog dialog = DialogEdit;
            vm.Shot(dialog, EditName);
            vm.LoadData();
        } 

        private void MenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MenuFlyoutItem item = (MenuFlyoutItem)sender as MenuFlyoutItem;
            BackupInfo info = (BackupInfo)item.DataContext as BackupInfo;
            if (item.Tag.ToString().Equals("0"))
            {
                
                    name = info.BackupName;

                path = info.BackupPath;
                    DataTransferManager.ShowShareUI();
               

            }
            else
            {
                vm.ItemClicked(info, item.Tag.ToString());
            }
         }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Button item = (Button)sender as Button;
            BackupInfo info = (BackupInfo)item.DataContext as BackupInfo;
            Frame.Navigate(typeof(BackupViewInfo), info.BackupPath);
        }
    }
}
