using Perfect_Scan.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Perfect_Scan.ViewModel.Backups
{
    public class BackupInfoViewModel : ViewModelBase
    {
        private string layoutProgressTextState = "", pathFile = "", device = "", data = "", name = "", size = "", countG = "", countE = "", systemOs = "", versionOs = "", layoutProgress = "Visible", layoutView = "Collapsed";
        private string remonear="",  remonearError = "", path = "", pathFileVisible = "Collapsed", deviceVisible = "Collapsed", dataVisible = "Collapsed", nameVisible = "Collapsed", sizeVisible = "Collapsed", countGVisible = "Collapsed", countEVisible = "Collapsed", systemOsVisible = "Collapsed", versionOsVisible = "Collapsed";
        private bool isBtnEnabled = false, isCheckedG = false, isEnabledG = false, isCheckedS = false, isEnabledS = false;
        private ResourceLoader loader = new ResourceLoader();
        private ContentDialog dialog;
        private BackupManager manager = new BackupManager();
        private TextBox editName;
        private Regex regex = new Regex("[0-9-a-z-A-Z-ãçÃÕÍÓÉéõ íó-]+");
        private RelayCommand addRestaurar, addExistentesCommand, shared, showDialogEditCommand, remonearCancelarCommand, remonearSalvarCommand, deleteCommand, checkGCommand, checkSCommand, refrssCommand;

        public ICommand AddRestaurar
        {

            get
            {
                if (addRestaurar == null)
                {
                    addRestaurar = new RelayCommand(p => OnAddRestaurar());
                }
                return addRestaurar;
            }
        }
         
        public ICommand AddEsxistentes
        {
            get
            {
                if (addExistentesCommand == null)
                {
                    addExistentesCommand = new RelayCommand(p => OnAddExistentes());
                }
                return addExistentesCommand;
            }
        }

        private async void OnAddExistentes()
        {
            IsBtnEnabled = false;
            await manager.AdicionarAoExistentes(path, isCheckedG, isCheckedS).ContinueWith(async p =>
            {
                if (p.IsCompleted || p.IsCompletedSuccessfully)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        StatusBtn();
                        Paginas.Root.RootApp.Instance.GetToast(loader.GetString("addBackupOk"), Tools.ModoColor.Succes);

                    });
                }
                else if (p.IsFaulted || p.IsCanceled)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        StatusBtn();
                        Paginas.Root.RootApp.Instance.GetToast(loader.GetString("addBackupFalha"), Tools.ModoColor.Error);
                    });

                }
            });
        }


        private async void OnAddRestaurar()
        {
            IsBtnEnabled = false;
            await manager.AdicionarRestaurar(path, isCheckedG, isCheckedS).ContinueWith(async p =>
            {
                if (p.IsCompleted || p.IsCompletedSuccessfully)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        StatusBtn();
                        Paginas.Root.RootApp.Instance.GetToast(loader.GetString("addBackupOk"), Tools.ModoColor.Succes);

                    });
                }
                else if (p.IsFaulted || p.IsCanceled)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        StatusBtn();
                        Paginas.Root.RootApp.Instance.GetToast(loader.GetString("addBackupFalha"), Tools.ModoColor.Error);
                    });

                }
            });
        }

        public ICommand ShareCommand
        {
            get
            {
                if (shared == null)
                {
                    shared = new RelayCommand(p => OnShare());
                }
                return shared;
            }
        }

        private void OnShare()
        {
            DataTransferManager.ShowShareUI();
        }

        #region Editar titulo

        public ICommand ShowEdit
        {
            get
            {
                if (showDialogEditCommand == null)
                {
                    showDialogEditCommand = new RelayCommand(p => OnEditar());
                }
                return showDialogEditCommand;
            }
        }

        public async void OnEditar()
        {
            try
            {
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);
                dialog.Title = name;
                Remonear = "";
                RemonearError = "";

                await dialog.ShowAsync();

            }
            catch (Exception x)
            {
                Paginas.Root.RootApp.Instance.GetToast(x.Message, Tools.ModoColor.None);
            }
        }


        private bool IsNameValid(string name)
        {
            bool valid = false;
            if (!name.Equals(""))
            {
                if (!regex.IsMatch(name))
                {
                    RemonearError = loader.GetString("enterNameValid");
                }
                else
                {
                    valid = true;
                }
            }
            else
            {
                RemonearError = loader.GetString("enterName");
            }
            return valid;
        }

        public string RemonearError
        {
            get { return remonearError; }
            private set
            {
                if (remonearError != value)
                {
                    remonearError = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand CancelarEditarTituloCommand
        {
            get
            {
                if (remonearCancelarCommand == null)
                {
                    remonearCancelarCommand = new RelayCommand(p => OnEditarCancelar());
                }
                return remonearCancelarCommand;
            }
        }

        public ICommand SalvarEditarTituloCommand
        {
            get
            {
                if (remonearSalvarCommand == null)
                {
                    remonearSalvarCommand = new RelayCommand(p => OnEditarSalvar());
                }
                return remonearSalvarCommand;
            }
        }

        internal void Shot(ContentDialog dialog, TextBox editName)
        {
            this.dialog = dialog;
            this.editName = editName;
            editName.TextChanged += (we, d) =>
            {
                Remonear = editName.Text;
                RemonearError = "";
            };
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


        private void OnEditarCancelar()
        {
            dialog.Hide();
        }

        public string Remonear
        {
            get { return remonear; }
            private set
            {
                if (remonear != value)
                {
                    remonear = value;
                    RaisePropertyChanged();
                }
            }
        }

        private async void OnEditarSalvar()
        {
            if (IsNameValid(remonear))
            {
                try
                {

                    dialog.Hide();
                    var file = await StorageFile.GetFileFromPathAsync(path);
                    if (file != null)
                    {
                        await file.RenameAsync($"{remonear}.oic").AsTask().ContinueWith(async p =>
                        {
                            if (p.IsCompleted || p.IsCompletedSuccessfully)
                            {
                                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                {
                                    Paginas.Root.RootApp.Instance.GetToast(loader.GetString("remoneBackupOk"), Tools.ModoColor.Succes);
                                    DisplayNameShot(file.DisplayName);
                                    path = file.Path;
                                    DisplayPathShot(path);
                                });
                            }
                            else if (p.IsFaulted || p.IsCanceled)
                            {
                                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                {
                                    Paginas.Root.RootApp.Instance.GetToast(loader.GetString("remoneFalha"), Tools.ModoColor.Error);
                                });

                            }
                        });
                    }

                }
                catch { }

            }

        }

        #endregion

        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(p => DeleteFile());
                }
                return deleteCommand;
            }
        }

        private async void DeleteFile()
        {
            try
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Title = name;
                dialog.Content = loader.GetString("deletehistoricoSubtitle");
                dialog.PrimaryButtonText = loader.GetString("delete");
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                dialog.Foreground = ((Brush)App.Current.Resources["Texto"]);
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);
                dialog.PrimaryButtonStyle = ((Style)App.Current.Resources["ButtonStyle1"]);
                dialog.CloseButtonStyle = ((Style)App.Current.Resources["ButtonStyleContent"]);
                dialog.PrimaryButtonClick += async (senderBtn, ArgsBtn) =>
                {
                    var file = await StorageFile.GetFileFromPathAsync(path);
                if (file != null)
                {
                    await file.DeleteAsync().AsTask().ContinueWith(async p =>
                    {
                        if (p.IsCompleted || p.IsCompletedSuccessfully)
                        {
                            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                Paginas.Root.RootApp.Instance.GoBack(true);
                                Paginas.Root.RootApp.Instance.GetToast(loader.GetString("deleteBackupOk"), Tools.ModoColor.Succes);

                            });
                        }
                        else if (p.IsFaulted || p.IsCanceled)
                        {
                            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                Paginas.Root.RootApp.Instance.GetToast(loader.GetString("deleteFalha"), Tools.ModoColor.Error);
                            });

                        }
                    });
                }
                };
                await dialog.ShowAsync();

            }
            catch { }
        }

        public ICommand RefressCommand
        {
            get
            {
                if (refrssCommand == null)
                {
                    refrssCommand = new RelayCommand(p => UpdateFile());
                }
                return refrssCommand;
            }
        }

        public bool IsBtnEnabled
        {
            get { return isBtnEnabled; }
            private set
            {
                if (isBtnEnabled != value)
                {
                    isBtnEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string LayoutProgressTextState
        {
            get { return layoutProgressTextState; }
            private set
            {
                if (layoutProgressTextState != value)
                {
                    layoutProgressTextState = value;
                    RaisePropertyChanged();
                }
            }
        }
         
        public void ShotFileBackup(string path) 
        {
            this.path = path;
            UpdateFile();
        }

        private async void UpdateFile()
        {
            try
            {
                LayoutProgressTextState = loader.GetString("carregando");
                LayoutProgress = "Visible";
                LayoutView = "Collapsed";
                var file = await StorageFile.GetFileFromPathAsync(path);
                if (file != null)
                {
                    LayoutProgressTextState = loader.GetString("fileScanBackup");
                    BackupHelper helper = new BackupHelper(file);
                    var xDoc = await helper.GetDocumentAsync();
                    if (xDoc != null)
                    {
                        LayoutProgressTextState = loader.GetString("VerificndoDadosBackup");
                        DisplayNameShot(helper.DisplayName);
                        DisplayPathShot(helper.BackupPath);
                        DisplayCountEShot(await helper.GetBackupCountEscaneadosAsync());
                        DisplayCountGShot(await helper.GetBackupCountGeradosAsync());
                        DisplayDataShot(await helper.GetBackupHoraDiaAsync());
                        DisplayDeviceShot(await helper.GetBackupDeviceAsync());
                        DisplaySizeShot(await helper.GetSizeBackupAsync());
                        DisplaySystemShot(await helper.GetBackupSystemAsync());
                        DisplayVersionShot(await helper.GetBackupVersionAsync());
                        var isE = await helper.IsEscaneadosAsync();
                        var isG = await helper.IsGeradosAsync();
                        IsEnabledG = isG;
                        IsEnabledS = isE;
                        IsCheckedG = isG;
                        IsCheckedS = isE;
                        StatusBtn();
                        LayoutProgressTextState = "";
                        LayoutProgress = "Collapsed";
                        LayoutView = "Visible";
                    }
                    else
                    {
                        LayoutProgressTextState = loader.GetString("VerificndoDadosBackupError");
                        LayoutProgress = "Visible";
                        LayoutView = "Collapsed";
                    }
                }
                else
                {
                    LayoutProgressTextState = loader.GetString("fileScanBackupError");
                    LayoutProgress = "Visible";
                    LayoutView = "Collapsed";

                }
            }
            catch
            {
                LayoutProgressTextState = loader.GetString("fileScanBackupError");
                LayoutProgress = "Visible";
                LayoutView = "Collapsed";
            }
        }

        private void OnChckedS()
        {
            isCheckedS = !isCheckedS;
            StatusBtn();
        }

        private void OnChckedG()
        {
            isCheckedG = !isCheckedG;
            StatusBtn();
        }

        public ICommand CheckedSCommand
        {
            get
            {
                if (checkSCommand == null)
                {
                    checkSCommand = new RelayCommand(p => OnChckedS());
                }
                return checkSCommand;
            }
        }

        public ICommand CheckedGCommand
        {
            get
            {
                if (checkGCommand == null)
                {
                    checkGCommand = new RelayCommand(p => OnChckedG());
                }
                return checkGCommand;
            }
        }


        private void StatusBtn()
        {
            IsBtnEnabled = isCheckedG || isCheckedS;
        }

        #region DisplayCountE
        private void DisplayCountEShot(string input)
        {
            int count = Convert.Convert.ToInt(input);
            string retur = input;
            if(count == 0)
            {
                retur = "Nenhum";
            }
            CountE = input != null && !input.Equals("") ? retur : "";
            DisplayCountEVisible = input != null && !input.Equals("") ? "Visible" : "Collapsed";
        }

        public string DisplayCountEVisible
        {
            get { return countEVisible; }
            private set
            {
                if (countEVisible != value)
                {
                    countEVisible = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string CountE
        {
            get { return countE; }
            private set
            {
                if (countE != value)
                {
                    countE = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region DisplayCountG
        private void DisplayCountGShot(string input)
        {
            int count = Convert.Convert.ToInt(input);
            string retur = input;
            if (count == 0)
            {
                retur = "Nenhum";
            }
            CountG = input != null && !input.Equals("") ? retur : "";
            DisplayCountGVisible = input != null && !input.Equals("") ? "Visible" : "Collapsed";
        }

        public string DisplayCountGVisible
        {
            get { return countGVisible; }
            private set
            {
                if (countGVisible != value)
                {
                    countGVisible = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string CountG
        {
            get { return countG; }
            private set
            {
                if (countG != value)
                {
                    countG = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region DisplayDevice
        private void DisplayDeviceShot(string input)
        {
            Device = input != null && !input.Equals("") ? input : "";
            DisplayDeviceVisible = input != null && !input.Equals("") ? "Visible" : "Collapsed";
        }

        public string DisplayDeviceVisible
        {
            get { return deviceVisible; }
            private set
            {
                if (deviceVisible != value)
                {
                    deviceVisible = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Device
        {
            get { return device; }
            private set
            {
                if (device != value)
                {
                    device = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region DisplaySystem
        private void DisplaySystemShot(string input)
        {
            System = input != null && !input.Equals("") ? input : "";
            DisplaySystemVisible = input != null && !input.Equals("") ? "Visible" : "Collapsed";
        }

        public string DisplaySystemVisible
        {
            get { return systemOsVisible; }
            private set
            {
                if (systemOsVisible != value)
                {
                    systemOsVisible = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string System
        {
            get { return systemOs; }
            private set
            {
                if (systemOs != value)
                {
                    systemOs = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region DisplayVersion
        private void DisplayVersionShot(string input)
        {
            Version = input != null && !input.Equals("") ? input : "";
            DisplayVersionVisible = input != null && !input.Equals("") ? "Visible" : "Collapsed";
        }

        public string DisplayVersionVisible
        {
            get { return versionOsVisible; }
            private set
            {
                if (versionOsVisible != value)
                {
                    versionOsVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Version
        {
            get { return versionOs; }
            private set
            {
                if (versionOs != value)
                {
                    versionOs = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region DisplayData
        private void DisplayDataShot(string input)
        {
            Data = input != null && !input.Equals("") ? input : "";
            DisplayDataVisible = input != null && !input.Equals("") ? "Visible" : "Collapsed";
        }

        public string DisplayDataVisible
        {
            get { return dataVisible; }
            private set
            {
                if (dataVisible != value)
                {
                    dataVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Data
        {
            get { return data; }
            private set
            {
                if (data != value)
                {
                    data = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region DisplaySize
        private void DisplaySizeShot(string input)
        {
            Size = input != null && !input.Equals("") ? input : "";
            DisplaySizeVisible = input != null && !input.Equals("") ? "Visible" : "Collapsed";
        }

        public string DisplaySizeVisible
        {
            get { return sizeVisible; }
            private set
            {
                if (sizeVisible != value)
                {
                    sizeVisible = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Size
        {
            get { return size; }
            private set
            {
                if (size != value)
                {
                    size = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region DisplayName
        private void DisplayNameShot(string input)
        {
            DisplayName = input != null && !input.Equals("") ? input : "";
            DisplayNameVisible = input != null && !input.Equals("") ? "Visible" : "Collapsed";
        }

        public string DisplayNameVisible
        {
            get { return nameVisible; }
            private set
            {
                if (nameVisible != value)
                {
                    nameVisible = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string DisplayName
        {
            get { return name; }
            private set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region DisplayPath
        private void DisplayPathShot(string input)
        {
            DisplayPath = input != null && !input.Equals("") ? input : "";
            DisplayPathVisible = input != null && !input.Equals("") ? "Visible" : "Collapsed";
        }

        public string DisplayPathVisible
        {
            get { return pathFileVisible; }
            private set
            {
                if (pathFileVisible != value)
                {
                    pathFileVisible = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string DisplayPath
        {
            get { return pathFile; }
            private set
            {
                if (pathFile != value)
                {
                    pathFile = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion



        public bool IsEnabledS
        {
            get { return isEnabledS; }
            private set
            {
                if (isEnabledS != value)
                {
                    isEnabledS = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsCheckedS
        {
            get { return isCheckedS; }
            private set
            {
                if (isCheckedS != value)
                {
                    isCheckedS = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsEnabledG
        {
            get { return isEnabledG; }
            private set
            {
                if (isEnabledG != value)
                {
                    isEnabledG = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsCheckedG
        {
            get { return isCheckedG; }
            private set
            {
                if (isCheckedG != value)
                {
                    isCheckedG = value;
                    RaisePropertyChanged();
                }
            }
        }


        public string LayoutProgress
        {
            get
            {
                return layoutProgress;
            }
            private set
            {
                if (layoutProgress != value)
                {
                    layoutProgress = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string LayoutView
        {
            get
            {
                return layoutView;
            }
            private set
            {
                if (layoutView != value)
                {
                    layoutView = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
