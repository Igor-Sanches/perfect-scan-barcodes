using Firebase.Database.Query;
using Perfect_Scan.Database;
using Perfect_Scan.Database.Sql;
using Perfect_Scan.Manager;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Perfect_Scan.ViewModel
{
    public class EscaneadosViewModel : ViewModelBase
    {
        private List<Escaneados> GetEscaneados = new List<Escaneados>();
        private bool progressLayout = true;
        private ResourceLoader loader = new ResourceLoader(); 
        private ControleEscaneados escaneados = new ControleEscaneados();
        private string remonearError = "", remonear = "", historicoVisible = "Collapsed", listaVazia = "Collapsed";
        private RelayCommand remonearCancelarCommand, remonearSalvarCommand;
        private ContentDialog dialog;
        private Regex regex = new Regex("[0-9-a-z-A-Z-ãçÃÕÍÓÉéõ íó-]+");
        private Escaneados escaneado;
        private TextBox EditName;


        internal async void Sorting(int selectedIndex)
        {
            try
            {
                if (selectedIndex == 0)
                {
                    ScrollViewer view = new ScrollViewer();
                    StackPanel panel = new StackPanel();
                    ContentDialog dialog = new ContentDialog();
                    dialog.Foreground = ((Brush)App.Current.Resources["Titulo"]);
                    dialog.Title = loader.GetString("select");
                    dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);

                    string[] strings = new string[] { "sortingName", "sortingNameZA", "sortingData", "sortingDataZA", "sortingCodigoTipo", "sortingFormato" };
                    for (int i = 0; i < strings.Length; i++)
                    {
                        RadioButton box = new RadioButton();
                        box.Style = ((Style)App.Current.Resources["RadioButtonStyle1"]);
                        box.Content = loader.GetString(strings[i]);
                        box.Tag = i;
                        box.IsChecked = Data.Data.SortingEscaneados == i; 
                        box.Click += (s, a) =>
                        {

                            dialog.Hide();
                            RadioButton btn = (RadioButton)s as RadioButton;
                            Data.Data.SortingEscaneados = (int)btn.Tag;
                            loadLista();
                            CallingInfoInstance_CellInfoUpdateCompletedAsync();

                        };
                        panel.Children.Add(box);
                    }
                    view.Content = panel;
                    dialog.Content = view;
                    dialog.CloseButtonStyle = ((Style)App.Current.Resources["ButtonStyleContent"]);
                    dialog.CloseButtonText = loader.GetString("ButtonClose");
                    await dialog.ShowAsync();
                } 
            }
            catch
            {

            }
        }

        internal async void DeleteAll(int selectedIndex)
        {
            try
            {
                if (selectedIndex == 0)
                {
                    ContentDialog dialog = new ContentDialog();
                    dialog.Title = loader.GetString("deleteAllTitle");
                    dialog.Content = loader.GetString("deleteAllSubtitleEscaneados");
                    dialog.PrimaryButtonText = loader.GetString("ButtonDeleteArmazemaneto");
                    dialog.SecondaryButtonText = loader.GetString("ButtonDeleteNuvem");
                    dialog.CloseButtonText = loader.GetString("ButtonClose");
                    dialog.Foreground = ((Brush)App.Current.Resources["Texto"]);
                    dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);
                    dialog.PrimaryButtonStyle = ((Style)App.Current.Resources["ButtonStyle1"]);
                    dialog.SecondaryButtonStyle = ((Style)App.Current.Resources["ButtonStyle1"]);
                    dialog.CloseButtonStyle = ((Style)App.Current.Resources["ButtonStyleContent"]);
                    dialog.PrimaryButtonClick += (s, a) =>
                    {
                        
                        dialog.Hide();
                        escaneados.Limpa(); 
                        CallingInfoInstance_CellInfoUpdateCompletedAsync();

                    };
                     dialog.SecondaryButtonClick += async (s, a) =>
                    {
                        try
                        {
                            dialog.Hide();
                            await App.GetInstance.GetDatabase.Child($"{Data.Conta.UUID}/Escaneados").DeleteAsync();
                            loadLista();
                            CallingInfoInstance_CellInfoUpdateCompletedAsync();
                            Paginas.Root.RootApp.Instance.GetToast(loader.GetString("nuvemDeleted"), Tools.ModoColor.Succes);
                        }
                        catch (Exception x)
                        {
                            Paginas.Root.RootApp.Instance.GetToast(x.Message, Tools.ModoColor.None);
                        }

                    };
                    await dialog.ShowAsync();
                } 
            }
            catch
            {

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

        private void OnEditarSalvar()
        { 
            if (IsNameValid(remonear))
            {
                escaneado.DisplayName = remonear;
                escaneados.Atualizar(escaneado);
                OnNuvemEditor(escaneado);
                dialog.Hide();
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

        internal void Shot(ContentDialog dialog, TextBox EditName)
        {
            this.dialog = dialog;
            this.EditName = EditName;
            EditName.TextChanged += (we, d) =>
            {
                Remonear = EditName.Text;
                RemonearError = "";
            };
        }

        public async void OnDelete(Escaneados escaneado)
        {
            try
            {
                dialog = new ContentDialog();
                dialog.Title = escaneado.DisplayName;
                dialog.Content = loader.GetString("deletehistoricoSubtitle");
                dialog.PrimaryButtonText = loader.GetString("delete");
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                dialog.Foreground = ((Brush)App.Current.Resources["Texto"]);
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);
                dialog.PrimaryButtonStyle = ((Style)App.Current.Resources["ButtonStyle1"]);
                dialog.CloseButtonStyle = ((Style)App.Current.Resources["ButtonStyleContent"]);
                dialog.PrimaryButtonClick += (senderBtn, ArgsBtn) =>
                {
                    try
                    {
                        OnDeleteNuvem(escaneado);
                        escaneados.Excluir(escaneado);
                        GetEscaneados.Remove(escaneado);
                        dialog.Hide();
                        CallingInfoInstance_CellInfoUpdateCompletedAsync();

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

        private async void OnDeleteNuvem(Escaneados escaneado)
        {
            try
            {
                if (escaneado.IsNuvem)
                {
                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        var db = App.GetInstance.GetDatabase;
                        await db.Child($"{Data.Conta.UUID}/Escaneados/{escaneado.UUID}").DeleteAsync();
                    }
                }
            }
            catch { }
        }

        public async void OnEditar(Escaneados escaneado)
        {
            try
            {
                this.escaneado = escaneado;
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);
                dialog.Title = escaneado.DisplayName;
                Remonear = "";
                RemonearError = "";
                
                await dialog.ShowAsync();

            }
            catch (Exception x)
            {
                Paginas.Root.RootApp.Instance.GetToast(x.Message, Tools.ModoColor.None);
            }
        }

        private async void OnNuvemEditor(Escaneados escaneado)
        {
            try
            {
                if (escaneado.IsNuvem)
                {
                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        HistoricoNuvem nuvem = new HistoricoNuvem();
                        nuvem.UUID = escaneado.UUID;
                        nuvem.Titulo = escaneado.DisplayName;
                        nuvem.Codigo = escaneado.Codigo;
                        nuvem.Formato = escaneado.Formato;
                        nuvem.Data = escaneado.Data;
                        nuvem.Tipo = escaneado.Tipo;
                        nuvem.DbType = "Escaneado";
                        var db = App.GetInstance.GetDatabase;
                        await db.Child($"{Data.Conta.UUID}/Escaneados/{nuvem.UUID}").PutAsync(nuvem).ContinueWith(p => { if (!p.IsCompleted) { Data.Data.IsFaulted = true; } });
                    }
                }
            }
            catch { }
        }

        public EscaneadosViewModel()
        {
            loadLista();
            AtualizarLista();
        }

        public void AtualizarLista()
        {
            CallingInfoInstance_CellInfoUpdateCompletedAsync();
        }

        /// <summary>
        /// Ensures the UI is updated upon initilization of the cellular details singleton.
        /// Even if the contacts module is loaded before the singleton is done intializaing
        /// This is fired from the calling info singleton
        /// </summary>
        private async void CallingInfoInstance_CellInfoUpdateCompletedAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    UpdateHistoricosListAsync();
                });
        }
        private void UpdateHistoricosListAsync()
        {
             LoadHistoricosFromStoreAsync();
        }

        private void LoadHistoricosFromStoreAsync()
        {
            GetEscaneados =  escaneados.ListaEscaneados();

            int count = GetEscaneados.Count; 
                Lista = GetEscaneados; 
            LayoutListavazia = count == 0 ? "Visible" : "Collapsed";
            LayoutHistoricos = count > 0 ? "Visible" : "Collapsed";
            ProgressLayout = false;
        }

        public List<Escaneados> Lista
        {
            get { return GetEscaneados; }
            private set
            {
                if (GetEscaneados != value)
                {
                    GetEscaneados = value;
                    RaisePropertyChanged();
                }
            }
        }

        void loadLista()
        { 
                LayoutListavazia = "Collapsed";
                LayoutHistoricos = "Collapsed";
                ProgressLayout = true; 
        }

        public string LayoutListavazia
        {
            get { return listaVazia; }
            private set
            {
                if (listaVazia != value)
                {
                    listaVazia = value;
                    RaisePropertyChanged();
                }
            }
        }


        public string LayoutHistoricos
        {
            get { return historicoVisible; }
            private set
            {
                if (historicoVisible != value)
                {
                    historicoVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ProgressLayout
        {
            get { return progressLayout; }
            private set
            {
                if (progressLayout != value)
                {
                    progressLayout = value;
                    RaisePropertyChanged();
                }
            }
        }

    }
}
