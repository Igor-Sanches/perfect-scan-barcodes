using Firebase.Database.Query;
using Perfect_Scan.Database;
using Perfect_Scan.Database.Sql;
using Perfect_Scan.Manager;
using System;
using System.Collections.Generic; 
using System.Net.NetworkInformation; 
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Perfect_Scan.ViewModel
{
    public class ContaViewModel : ViewModelBase
    {
        private RelayCommand sincronizarPara, commandTo, exitCommand;
        private string progressDialog = "Collapsed";
        private ResourceLoader loader = new ResourceLoader();

        public string Nome { get { return Data.Conta.Nome; } }

        public string Email { get { return Data.Conta.Email; } }

        public ICommand CommandSair
        {
            get
            {
                if(exitCommand == null)
                {
                    exitCommand = new RelayCommand(p => OnExit());
                }
                return exitCommand;
            }
        }

        private async void OnExit()
        {
            try
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Title = loader.GetString("exit");
                dialog.Content = loader.GetString("exitSummary");
                dialog.PrimaryButtonText = loader.GetString("sairBtn");
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                dialog.Foreground = ((Brush)App.Current.Resources["Texto"]);
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);
                dialog.PrimaryButtonStyle = ((Style)App.Current.Resources["ButtonStyle1"]);
                dialog.CloseButtonStyle = ((Style)App.Current.Resources["ButtonStyleContent"]);
                dialog.PrimaryButtonClick += (s, e) =>
                {
                    Sair();
                };
                await dialog.ShowAsync();
            }
            catch { }
        }

        private async void Sair()
        {
            Firebase.Auth.FirebaseAuthClient client = App.GetInstance.GetClient;
            await client.SignOutAsync().ContinueWith(async p =>
            {
                if (p.IsCompleted || p.IsCompletedSuccessfully)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        sairContinue();
                    });
                }
                else
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        await new MessageDialog("Error ao sair").ShowAsync();
                    });
                }
            });


        }

        private void sairContinue()
        {
            ControleEscaneados escaneados = new ControleEscaneados(); 
            ControleGerados gerados = new ControleGerados();

            foreach (Escaneados escaneado in escaneados.ListaEscaneados())
            {
                if (escaneado.IsNuvem)
                {
                    escaneados.Excluir(escaneado);
                }
            }

            foreach (Gerados gerado in gerados.ListaGerados())
            {
                if (gerado.IsNuvem)
                {
                    gerados.Excluir(gerado);
                }
            }

            Data.Conta.Clear();
            App.Current.Exit();
        }

        public bool ContNuvemLimit
        {
            get { return Data.Conta.IsNuvemLimit; }
        }

        public bool ContFromEscaneadosaNuvemConected
        {
            get { return Data.Conta.IsNuvemFromEscaneados; }
        }

        public bool ContFromGeradosaNuvemConected
        {
            get { return Data.Conta.IsNuvemFromGerados; }
        }

        public bool ContToEscaneadosaNuvemConected
        {
            get { return Data.Conta.IsNuvemToEscaneados; }
        }

        public bool ContToGeradosaNuvemConected
        {
            get { return Data.Conta.IsNuvemToGerados; }
        }

        private void PutProgress(bool b)
        {
            progressDialog = b ? "Visible" : "Collapsed";
        }

        public string ProgressDialog
        {
            get
            {
                return progressDialog;
            }
            private set
            {
                if(progressDialog != value)
                {
                    progressDialog = value;
                    RaisePropertyChanged();
                }
            }

        }

        public ICommand SincronizarParaNuvem
        {
            get
            {
                if (sincronizarPara == null)
                {
                    sincronizarPara = new RelayCommand(p => OnDialogSincForCloud());
                }
                return sincronizarPara;
            }
        }

        public ICommand SincronizarDaNuvem
        {
            get
            {
                if (commandTo == null)
                {
                    commandTo = new RelayCommand(p => OnDialogSincToCloud());
                }
                return commandTo;
            }
        }

        private async void OnDialogSincToCloud()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Content = loader.GetString("OnSincToCloud");
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                dialog.PrimaryButtonText = loader.GetString("ButtonSincronizar");
                dialog.PrimaryButtonClick += (s, e) =>
                {
                    dialog.Hide();
                    OnSincToCloud();
                };

                await dialog.ShowAsync();
            }
            else
            {
                await new MessageDialog(loader.GetString("errorConnect")).ShowAsync();
            }

        }

        private async void OnSincToCloud()
        {
            try
            {
                PutProgress(true);
                List<Gerados> gerados = new List<Gerados>();
                List<Escaneados> escaneados = new List<Escaneados>();
                var db = App.GetInstance.GetDatabase;
                var historicosGerados = await db.Child($"{Data.Conta.UUID}/Gerados").OnceAsync<HistoricoNuvem>();
                var historicosEscaneados = await db.Child($"{Data.Conta.UUID}/Escaneados").OnceAsync<HistoricoNuvem>();
                foreach (var item in historicosGerados)
                {
                    HistoricoNuvem nuvem = item.Object;
                     
                    Gerados gerado = new Gerados();
                    gerado.Codigo = nuvem.Codigo;
                    gerado.Data = nuvem.Data;
                    gerado.DisplayName = nuvem.Titulo;
                    gerado.Formato = nuvem.Formato;
                    gerado.Tipo = nuvem.Tipo;
                    gerado.UUID = nuvem.UUID;
                    gerado.IsNuvem = true;
                    gerados.Add(gerado);
                     
                }
                foreach (var item in historicosEscaneados)
                {
                    HistoricoNuvem nuvem = item.Object;
 

                    Escaneados escaneado = new Escaneados();
                    escaneado.Codigo = nuvem.Codigo;
                    escaneado.Data = nuvem.Data;
                    escaneado.DisplayName = nuvem.Titulo;
                    escaneado.Formato = nuvem.Formato;
                    escaneado.Tipo = nuvem.Tipo;
                    escaneado.UUID = nuvem.UUID;
                    escaneado.IsNuvem = true;
                    escaneados.Add(escaneado);



                }

                await HistoricoManager.EscaneadoAsync(escaneados, false);
                await HistoricoManager.GeradoAsync(gerados, false);
                PutProgress(false);
                Paginas.Root.RootApp.Instance.GetToast(loader.GetString("succesfulNuvem"), Tools.ModoColor.Succes);
            }
            catch { }
        }

        private async void OnDialogSincForCloud()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Content = loader.GetString("OnSincForCloud");
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                dialog.PrimaryButtonText = loader.GetString("ButtonSincronizar");
                dialog.PrimaryButtonClick += (s, e) =>
                {
                    dialog.Hide();
                    OnSincForCloud();
                };

                await dialog.ShowAsync();
            }
            else
            {
                await new MessageDialog(loader.GetString("errorConnect")).ShowAsync();
            }

        }

        private async void OnSincForCloud()
        {
            try
            {
                PutProgress(true);
                List<HistoricoNuvem> historicoNuvem = new List<HistoricoNuvem>();
                ControleGerados controleGerados = new ControleGerados();
                ControleEscaneados controleEscaneados = new ControleEscaneados();
                foreach(Gerados gerado in controleGerados.ListaGerados())
                { 
                    HistoricoNuvem nuvem = new HistoricoNuvem();
                    nuvem.UUID = gerado.UUID;
                    nuvem.Titulo = gerado.DisplayName;
                    nuvem.Codigo = gerado.Codigo;
                    nuvem.Formato = gerado.Formato;
                    nuvem.Data = gerado.Data;
                    nuvem.Tipo = gerado.Tipo;
                    nuvem.DbType = "Gerado";
                    historicoNuvem.Add(nuvem);
                }

                foreach(Escaneados escaneados in controleEscaneados.ListaEscaneados())
                { 
                        HistoricoNuvem nuvem = new HistoricoNuvem();
                        nuvem.UUID = escaneados.UUID;
                        nuvem.Titulo = escaneados.DisplayName;
                        nuvem.Codigo = escaneados.Codigo;
                        nuvem.Formato = escaneados.Formato;
                        nuvem.Data = escaneados.Data;
                        nuvem.Tipo = escaneados.Tipo;
                        nuvem.DbType = "Escaneado";
                    historicoNuvem.Add(nuvem);
                }

                var db = App.GetInstance.GetDatabase;
                foreach (HistoricoNuvem nuvem in historicoNuvem)
                {
                    if (nuvem.DbType.Equals("Gerado"))
                    {
                        await db.Child($"{Data.Conta.UUID}/Gerados/{nuvem.UUID}").PutAsync(nuvem).ContinueWith(p => { if (!p.IsCompleted) { Data.Data.IsFaulted = true; } });
                    }
                    else
                    {
                        await db.Child($"{Data.Conta.UUID}/Escaneados/{nuvem.UUID}").PutAsync(nuvem).ContinueWith(p => { if (!p.IsCompleted) { Data.Data.IsFaulted = true; } });
                    }

                }
                PutProgress(false);
                Paginas.Root.RootApp.Instance.GetToast(loader.GetString("succesfulNuvem"), Tools.ModoColor.Succes);


            }
            catch { }
        }
    }
}

