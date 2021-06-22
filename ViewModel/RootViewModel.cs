using Perfect_Scan.Manager;
using Perfect_Scan.Paginas;
using Perfect_Scan.Paginas.Backups;
using Perfect_Scan.Paginas.Visualizador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Perfect_Scan.ViewModel
{
    public class RootViewModel : ViewModelBase
    {
        private Frame _frame;
        private ResourceLoader loader = new ResourceLoader();
        private RelayCommand btnTiles, addCommand, restaurarCommand;
        private ImageSource backgrouund;
        private bool iniciar = true;
        private string sheetPanelVisible = "Collapsed";

        public string Name
        {
            get { return Data.Conta.Nome; }
        }

        public string Email
        {
            get { return Data.Conta.Email; }
        }

        public void Shot(Frame frame)
        {
            _frame = frame;

        }

        public ICommand GetCommand
        {
            get
            {
                if (btnTiles == null)
                {
                    btnTiles = new RelayCommand(this.Navigation);
                }
                return btnTiles;
            }
        }

        private void Navigation(object CommandParam)
        {
            string tag = CommandParam as string;
            NavigationFrame(tag);
        }

        public void NavigationFrame(string tag)
        {
            try
            {

                switch (tag)
                {

                    case "Account":
                        {
                            if (!tag.Equals(Data.Data.AppViewPager))
                            {
                                _frame.Navigate(typeof(ContaView));
                            }
                        }
                        break;
                    case "Sobre":
                        {
                            if (!tag.Equals(Data.Data.AppViewPager))
                            {
                                _frame.Navigate(typeof(Sobre));
                            }
                        }
                        break;
                    case "VersionMobile":
                        {
                            if (!"VersionMobile".Equals(Data.Data.AppViewPager))
                            {
                                _frame.Navigate(typeof(InfoAppsPlataformas));
                            }
                        }
                        break;
                    case "Support":
                        {
                            ApplicationID.OnSuporte();
                        }
                        break;
                    case "RateApp":
                        {
                            ApplicationID.GoLaunchRateOurReview();
                        }
                        break;
                    case "Inicio":
                        {
                            if (!tag.Equals(Data.Data.AppViewPager))
                            {
                                _frame.Navigate(typeof(Inicio));
                            }
                        }
                        break;
                    case "Escanear":
                        {
                            if (!tag.Equals(Data.Data.AppViewPager))
                            {
                                _frame.Navigate(typeof(EscaneamentoPage));
                            }
                        }
                        break;
                    case "EscanearImagem":
                        {
                            if (!tag.Equals(Data.Data.AppViewPager))
                            {
                                _frame.Navigate(typeof(EscanearImagem));
                            }
                        }
                        break;
                    case "Gerar":
                        {
                            if (!tag.Equals(Data.Data.AppViewPager))
                            {
                                _frame.Navigate(typeof(GerarCodigos));
                            }
                        }
                        break;
                    case "Historicos":
                        {
                            if (!tag.Equals(Data.Data.AppViewPager))
                            {
                                _frame.Navigate(typeof(HistoricosPage));
                            }
                        }
                        break;
                    case "Configuracoes":
                        {
                            if (!tag.Equals(Data.Data.AppViewPager))
                            {
                                _frame.Navigate(typeof(Configuracoes));
                            }
                        }
                        break;
                    case "BackupLocal":
                        {
                            ShowBackupOptions();
                        }
                        break;
                }
            }
            catch (Exception x)
            {

                Paginas.Root.RootApp.Instance.GetToast(x.Message, Tools.ModoColor.Error);
            }
        }


        private async void ShowBackupOptions()
        {
            try
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Title = loader.GetString("select");
                ScrollViewer view = new ScrollViewer();
                StackPanel painel = new StackPanel();
                string[] options = new string[] { loader.GetString("selectFazerBackup"), loader.GetString("selectRestaurarBackup") };
                int count = options.Count();
                for (int i = 0; i < count; i++)
                {
                    NavigationViewItem item = new NavigationViewItem();
                    item.Content = options[i];
                    item.Foreground = ((Brush)App.Current.Resources["Texto"]);
                    item.Tag = i;
                    item.Tapped += (sender, args) =>
                    {
                        if ((int)((NavigationViewItem)sender).Tag == 0)
                        {
                            dialog.Hide();
                            if (!"BackupLocalView".Equals(Data.Data.AppViewPager))
                            {
                                _frame.Navigate(typeof(CriarBackupsView));
                            }
                        }
                        else
                        {
                            dialog.Hide();
                            if (!"BackupLocalViews".Equals(Data.Data.AppViewPager))
                            {
                                _frame.Navigate(typeof(BackupViewPage));
                            }

                        }
                    };
                    painel.Children.Add(item);
                }
                view.Content = painel;
                dialog.Content = view;

                dialog.Foreground = ((Brush)App.Current.Resources["Texto"]);
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);
                dialog.CloseButtonStyle = ((Style)App.Current.Resources["ButtonStyleContent"]);
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                await dialog.ShowAsync();

            }
            catch { }
        }


        public string SheetPanelVisible
        {
            get { return sheetPanelVisible; }
            private set
            {
                if (sheetPanelVisible != value)
                {
                    sheetPanelVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand RestaurarCommand
        {
            get
            {
                if (restaurarCommand == null)
                {
                    restaurarCommand = new RelayCommand(p => RestaurarBackground());
                }
                return restaurarCommand;
            }
        }

        public ICommand AddCommand
        {
            get
            {
                if (addCommand == null)
                {
                    addCommand = new RelayCommand(p => SelectNewBackground());
                }
                return addCommand;
            }
        }

        internal async void OnNavigationTo(NavigationEventArgs e)
        {
            try
            {
                if(e.Parameter == e.Parameter as IActivatedEventArgs)
                {
                    var args = e.Parameter as IActivatedEventArgs;
                    if (args != null)
                    {
                        if (args.Kind == ActivationKind.File)
                        {
                            try
                            {
                                var fileargs = args as Windows.ApplicationModel.Activation.FileActivatedEventArgs;
                                string sxt = fileargs.Files[0].Path;
                                var file = (StorageFile)fileargs.Files[0];
                                if (file != null)
                                {
                                    //Verificar se é um Backup ou não 
                                    if (file.DisplayType == "Perfect Scan Backup")
                                    {
                                        iniciar = false;
                                        _frame.Navigate(typeof(Paginas.Backups.BackupViewInfo), file.Path);
                                    }
                                    else
                                    {
                                        iniciar = false;
                                        _frame.Navigate(typeof(EscanearImagem), args);
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        else if (args.Kind == ActivationKind.Protocol)
                        {
                            iniciar = false;
                            ApplicationID.Protocol(_frame, args);
                        }
                    }
                }
                else if(e.Parameter == e.Parameter as ShareOperation)
                {
                    var args = e.Parameter as ShareOperation;
                    if(args.Data.Contains(StandardDataFormats.Text))
                    {
                        string text = await args.Data.GetTextAsync();
                        string url = await args.Data.GetTextAsync();
                        string codigo = args.Data.Contains(StandardDataFormats.Text) ? text : url; 
                        _frame.Navigate(typeof(Paginas.GerarCodigos), codigo);
                    }else if (args.Data.Contains(StandardDataFormats.Bitmap))
                    {
                        var stream = await args.Data.GetBitmapAsync(); 
                        _frame.Navigate(typeof(EscanearImagem), stream);
                    }
                }
            }
            catch { }
        }
         
        internal void NavigationFrameFromHome()
        {
             if (iniciar)
             {
             _frame.Navigate(typeof(Inicio));
             }

        }

        internal void SheetPanel(bool v)
        {
            if (v)
            {
                if (sheetPanelVisible.Equals("Collapsed"))
                {
                    SheetPanelVisible = "Visible";
                }
            }
            else
            {
                if (sheetPanelVisible.Equals("Visible"))
                {
                    SheetPanelVisible = "Collapsed";
                }
            }

        }

        private async void RestaurarBackground()
        {
            try
            {
                SheetPanel(false);
                var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Background");
                if(folder != null)
                {
                    await folder.DeleteAsync().AsTask().ContinueWith(async p =>
                    {
                        if (p.IsCompletedSuccessfully || p.IsCompleted)
                        {
                            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            { 
                                Background = null;
                                loadBackground();
                            });
                        }
                        else
                        {
                            //Paginas.Root.RootApp.Instance.GetToast(loader.GetString("LoadPapelError"), Tools.ModoColor.Error);
                        }
                    });
                }
            }
            catch
            {
                Paginas.Root.RootApp.Instance.GetToast(loader.GetString("LoadPapelError"), Tools.ModoColor.Error);
            }
        }

        private async void SelectNewBackground()
        {
            try
            {
                SheetPanel(false);
                FileOpenPicker fileOpen = new FileOpenPicker();
                fileOpen.FileTypeFilter.Add(".jpg");
                fileOpen.FileTypeFilter.Add(".bmp");
                fileOpen.FileTypeFilter.Add(".jpeg");
                fileOpen.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                fileOpen.ViewMode = PickerViewMode.List;
                var file = await fileOpen.PickSingleFileAsync();
                if (file != null)
                {
                    AddNewBackground(file);
                }

            }
            catch { }
        }

        private async void AddNewBackground(StorageFile file)
        {
            try
            {

                Paginas.Root.RootApp.Instance.GetToast(loader.GetString("LoadPapel"), Tools.ModoColor.None);
                var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Background", CreationCollisionOption.OpenIfExists);
                if(folder != null)
                {
                    await file.CopyAsync(folder, "Background.jpg", NameCollisionOption.ReplaceExisting).AsTask().ContinueWith(async p =>
                    {
                        if(p.IsCompletedSuccessfully || p.IsCompleted)
                        {
                            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                var stream = await file.OpenAsync(FileAccessMode.Read);
                                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                                SoftwareBitmap software = await decoder.GetSoftwareBitmapAsync();
                                SoftwareBitmap bi = SoftwareBitmap.Convert(software, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

                                WriteableBitmap wb = new WriteableBitmap(bi.PixelWidth, bi.PixelHeight);

                                await wb.SetSourceAsync(stream);
                                Background = wb;
                            });
                        }
                        else
                        {
                            Paginas.Root.RootApp.Instance.GetToast(loader.GetString("LoadPapelError"), Tools.ModoColor.Error);
                        }
                    });
                }

            }
            catch { }
        }

        public ImageSource Background
        {
            get
            {
                return backgrouund;
            }
            private set
            {
                if (backgrouund != value)
                {
                    backgrouund = value;
                    RaisePropertyChanged();
                }
            }
        }

        public async void loadBackground()
        {

            if (backgrouund == null)
            {
                try
                {
                    StorageFile file = null;
                    try
                    {
                        var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Background");
                        if (folder != null)
                        {
                            var file2 = await folder.GetFileAsync("Background.jpg");
                            if (file2 != null)
                            {
                                file = file2;
                            }
                        }
                    }
                    catch { }
                    if (file == null)
                    {
                        file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/background.jpg"));

                    }
                    var stream = await file.OpenAsync(FileAccessMode.Read);
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    SoftwareBitmap software = await decoder.GetSoftwareBitmapAsync();
                    SoftwareBitmap bi = SoftwareBitmap.Convert(software, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

                    WriteableBitmap wb = new WriteableBitmap(bi.PixelWidth, bi.PixelHeight);

                    await wb.SetSourceAsync(stream);
                    Background = wb;
                }
                catch (Exception x) { Paginas.Root.RootApp.Instance.GetToast(x.Message); ; }
            }

        }
         
    }
}
