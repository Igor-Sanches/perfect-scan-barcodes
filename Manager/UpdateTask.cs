using Perfect_Scan.Database;
using Perfect_Scan.Database.Sql;
using Perfect_Scan.Tools;
using Perfect_Scan.ViewModel;
using System;
using System.Collections.Generic; 
using System.Net.NetworkInformation; 
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Perfect_Scan.Manager
{
    public class UpdateTask
    {
        private Timer _timer;
        private bool isLimitTask = false;

        public UpdateTask()
        {
            _timer = new Timer();
            _timer.StartTimer();
            _timer.TimerEnded += async (s, e) =>
            {
               
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        if (Data.Conta.IsNuvemLimit)
                        {
                            if (NetworkInterface.GetIsNetworkAvailable())
                            {
                                HistoricosViewModel vm = ViewModelDispatcher.HistoricosViewModel;
                                if (vm.IsOpenHistorical)
                                {
                                    ShotTask();
                                }
                            }
                            else
                            {
                                _timer.ResetTimer();
                                _timer.StartTimer();
                            }
                        }
                        else
                        {
                            if (NetworkInterface.GetIsNetworkAvailable())
                            {
                                ShotTask();
                            }
                            else
                            {
                                _timer.ResetTimer();
                                _timer.StartTimer();
                            }
                        }
                    }); 
               
               
            };
        }

        private async void ShotTask()
        {
            try
            {
                var db = App.GetInstance.GetDatabase;
              
                if (Data.Conta.IsNuvemFromGerados)
                {
                    List<Gerados> gerados = new List<Gerados>();
                    var historicosGerados = await db.Child($"{Data.Conta.UUID}/Gerados").OnceAsync<HistoricoNuvem>();
                    foreach (var item in historicosGerados)
                    {
                        HistoricoNuvem nuvem = item.Object;
                        Gerados gerado = new Gerados();
                        gerado.DisplayName = nuvem.Titulo;
                        gerado.Codigo = nuvem.Codigo;
                        gerado.Data = nuvem.Data;
                        gerado.Formato = nuvem.Formato;
                        gerado.Tipo = nuvem.Tipo;
                        gerado.UUID = nuvem.UUID;
                        gerado.IsNuvem = true;
                        gerados.Add(gerado);
                    }
                    GeradosViewModel vm = ViewModelDispatcher.GeradosView;
                    var ok = await HistoricoManager.GeradoAsync(gerados, false);
                    if (ok)
                    {
                        vm.AtualizarLista();
                    }
                }

                if (Data.Conta.IsNuvemFromEscaneados)
                {
                    List<Escaneados> escaneados = new List<Escaneados>();
                    var historicosEscaneados = await db.Child($"{Data.Conta.UUID}/Escaneados").OnceAsync<HistoricoNuvem>();
                    foreach (var item in historicosEscaneados)
                    {
                        HistoricoNuvem nuvem = item.Object;

                        Escaneados escaneado = new Escaneados();
                        escaneado.DisplayName = nuvem.Titulo;
                        escaneado.Codigo = nuvem.Codigo;
                        escaneado.Data = nuvem.Data;
                        escaneado.Formato = nuvem.Formato;
                        escaneado.Tipo = nuvem.Tipo;
                        escaneado.UUID = nuvem.UUID;
                        escaneado.IsNuvem = true;
                        escaneados.Add(escaneado);

                    }

                    EscaneadosViewModel model = ViewModelDispatcher.EscaneadosView;
                    var ok2 = await HistoricoManager.EscaneadoAsync(escaneados, false);
                    if (ok2)
                    {
                        model.AtualizarLista();
                    }
                }
               
               _timer.ResetTimer();
                _timer.StartTimer();
            }
            catch { }
        }
    }
}
