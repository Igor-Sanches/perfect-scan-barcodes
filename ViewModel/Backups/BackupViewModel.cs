using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Perfect_Scan.Tools;
using Perfect_Scan.Models;
using Perfect_Scan.Manager;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.DataTransfer;

namespace Perfect_Scan.ViewModel.Backups
{
    public class BackupViewModel : ViewModelBase
    {
        private List<BackupInfo> files = new List<BackupInfo>();
       private ResourceLoader loader = new ResourceLoader();
        private string remonearError = "", remonear = "", backupList = "Collapsed", statusLayout = "Visible";
        private string stausPrincipal = "", stausSubPrincipal = "";
        private RelayCommand commandDelete, avancadoCommand;
        private RelayCommand remonearCancelarCommand, remonearSalvarCommand;
        private ContentDialog dialog;
        private TextBox editName;
        private string isBtnRefressVisible = "Visible", isBtnDeleteVisible = "Collapsed";
        private BackupInfo info;
        private Regex regex = new Regex("[0-9-a-z-A-Z-ãçÃÕÍÓÉéõ íó-]+");

        internal void Shot(ContentDialog dialog, TextBox editName)
        {
            this.dialog = dialog;
            this.editName = editName;
            editName.TextChanged += (we, d) =>
            {
                Remonear = editName.Text;
                RemonearError = "";
            };
        }
         
        public ICommand DeleteCommand
        {
            get
            {
                if (commandDelete == null)
                {
                    commandDelete = new RelayCommand(p => OnDeleteAll());
                }
                return commandDelete;
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

        private async void OnEditarSalvar()
        {
            if (IsNameValid(remonear))
            {
                try
                {

                    dialog.Hide();
                    var file = await StorageFile.GetFileFromPathAsync(info.BackupPath);
                    if (file != null)
                    {
                        await file.RenameAsync($"{remonear}.oic").AsTask().ContinueWith(async p =>
                        {
                            if (p.IsCompleted || p.IsCompletedSuccessfully)
                            {
                                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                {
                                    LoadData();
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

        private async void OnDeleteAll()
        {
            try
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Title = loader.GetString("deleteAllTitle");
                dialog.Content = loader.GetString("deleteAllSubtitleBackups");
                dialog.PrimaryButtonText = loader.GetString("delete");
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                dialog.Foreground = ((Brush)App.Current.Resources["Texto"]);
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);
                dialog.PrimaryButtonStyle = ((Style)App.Current.Resources["ButtonStyle1"]);
                dialog.CloseButtonStyle = ((Style)App.Current.Resources["ButtonStyleContent"]);
                dialog.PrimaryButtonClick += async (s, a) =>
                {
                    try
                    {
                        dialog.Hide();
                        LayoutListBackup = "Collapsed";
                        LayoutStatusBackup = "Visible";
                        IsButtonRefressVisible = "Collapsed";
                        IsButtonDeleteVisible = "Collapsed";
                        BackupTextStatus = loader.GetString("apagando");
                        BackupSubTextStatus = "";
                        var lista = files;
                        if (lista.Count > 0)
                        {
                            foreach (var item in lista)
                            {
                                var file = await StorageFile.GetFileFromPathAsync(item.BackupPath);
                                BackupSubTextStatus = $"{loader.GetString("apagando")} {file.DisplayName}\n";
                                await file.DeleteAsync();
                            }
                            LoadData();
                        }
                    }
                    catch (Exception x)
                    {
                        Paginas.Root.RootApp.Instance.GetToast(x.Message, Tools.ModoColor.None);
                    }

                };

                await dialog.ShowAsync();
            }
            catch { }
        }

        internal void ItemClicked(BackupInfo info, string tag)
        {
            switch (tag)
            {
                case "1":
                    OnEditar(info);
                    break;
                case "2":
                    OnDelete(info);
                    break;
            }
        }
         
        public async void OnEditar(BackupInfo info)
        {
            try
            {
                this.info = info;
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);
                dialog.Title = info.BackupName;
                Remonear = "";
                RemonearError = "";

                await dialog.ShowAsync();

            }
            catch (Exception x)
            {
                Paginas.Root.RootApp.Instance.GetToast(x.Message, Tools.ModoColor.None);
            }
        }


        private async void OnDelete(BackupInfo info)
        {
            try
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Title = info.BackupName;
                dialog.Content = loader.GetString("deletehistoricoSubtitle");
                dialog.PrimaryButtonText = loader.GetString("delete");
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                dialog.Foreground = ((Brush)App.Current.Resources["Texto"]);
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);
                dialog.PrimaryButtonStyle = ((Style)App.Current.Resources["ButtonStyle1"]);
                dialog.CloseButtonStyle = ((Style)App.Current.Resources["ButtonStyleContent"]);
                dialog.PrimaryButtonClick += async (senderBtn, ArgsBtn) =>
                {
                    try
                    {

                        var file = await StorageFile.GetFileFromPathAsync(info.BackupPath);
                        if (file != null)
                        {
                            await file.DeleteAsync().AsTask().ContinueWith(async p =>
                            {
                                if (p.IsCompleted || p.IsCompletedSuccessfully)
                                {
                                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                    {

                                        files.Remove(info); ;
                                        LoadData();

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

                    }
                    catch (Exception x)
                    {
                        Paginas.Root.RootApp.Instance.GetToast(x.Message, Tools.ModoColor.None);
                    }
                };
                await dialog.ShowAsync();

            }
            catch (Exception x)
            {
                Paginas.Root.RootApp.Instance.GetToast(x.Message, Tools.ModoColor.None);
            }

        }

        public string IsButtonDeleteVisible
        {
            get
            {
                return isBtnDeleteVisible;
            }
            private set
            {
                if (isBtnDeleteVisible != value)
                {
                    isBtnDeleteVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string IsButtonRefressVisible
        {
            get
            {
                return isBtnRefressVisible;
            }
            private set
            {
                if (isBtnRefressVisible != value)
                {
                    isBtnRefressVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand CommandLoadAvancado
        {
            get
            {
                if (avancadoCommand == null)
                {
                    avancadoCommand = new RelayCommand(p => OnRefress());
                }
                return avancadoCommand;
            }
        }

        private void OnRefress()
        {
            LoadData();
        }

        public string BackupSubTextStatus
        {
            get { return stausSubPrincipal; }
            private set
            {
                if (stausSubPrincipal != value)
                {
                    stausSubPrincipal = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string BackupTextStatus
        {
            get { return stausPrincipal; }
            private set
            {
                if (stausPrincipal != value)
                {
                    stausPrincipal = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string LayoutStatusBackup
        {
            get { return statusLayout; }
            private set
            {
                if (statusLayout != value)
                {
                    statusLayout = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string LayoutListBackup
        {
            get { return backupList; }
            private set
            {
                if (backupList != value)
                {
                    backupList = value;
                    RaisePropertyChanged();
                }
            }
        }

    
        public List<BackupInfo> Lista
        {
            get { return files; }
            private set
            {
                if (files != value)
                {
                    files = value;
                    RaisePropertyChanged();
                }
            }
        }

        public void LoadData()
        {
            LayoutListBackup = "Collapsed";
            LayoutStatusBackup = "Visible";
            IsButtonRefressVisible = "Collapsed";
            IsButtonDeleteVisible = "Collapsed";
            BackupTextStatus = loader.GetString("buscandoBackups");
            BackupSubTextStatus = "";
            TaskLoad();
        }

        private async void TaskLoad()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
               CoreDispatcherPriority.Normal,
               async () =>
               {
                   await UpdateListBackupAsync();
               });
        }

        private async Task UpdateListBackupAsync()
        {
            try
            {
                List<BackupInfo> ListaFiles = new List<BackupInfo>();
              List<StorageFile> lista = null;


                lista = await AppStorage.GetFilesBackupsAsync(); 
                if (lista.Count > 0)
                {
                    foreach(var file in lista)
                    { 
                        BackupHelper helper = new BackupHelper(file);
                        var xDoc = await helper.GetDocumentAsync();
                        if (xDoc != null)
                        {
                            BackupSubTextStatus = file.DisplayName;
                            BackupInfo info = new BackupInfo();
                            info.BackupName = helper.DisplayName;
                            info.BackupSize = await helper.GetSizeBackupAsync();
                            info.BackupData = await helper.GetBackupHoraDiaAsync();
                            info.BackupPath = helper.BackupPath;
                            info.BackupSize = await helper.GetSizeBackupAsync();
                            info.BackupSize = await helper.GetSizeBackupAsync();
                            info.BackupResume = await helper.GetResumoCountsAsync();
                            ListaFiles.Add(info);
                        } 
                    }
                    BackupSubTextStatus = "";

                }

                Lista.OrderByDescending(p => p.BackupData);
                Lista = ListaFiles;
                int count = files.Count; 
                IsButtonRefressVisible = "Visible";
                IsButtonDeleteVisible = count > 0 ? "Visible" : "Collapsed";
                LayoutListBackup = count > 0 ? "Visible" : "Collapsed";
                LayoutStatusBackup = count == 0 ? "Visible" : "Collapsed";
                BackupTextStatus = loader.GetString("NoBackups");
                BackupSubTextStatus = loader.GetString("NoDescBackups");

            }
            catch { }
        }
    }
}
