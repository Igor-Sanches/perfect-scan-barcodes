using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Perfect_Scan.Database.Sql;
using Perfect_Scan.Manager.ResultScanner;
using Perfect_Scan.ViewModel.Visualizador;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Perfect_Scan.ViewModel
{
    public class VisualizadorViewModel : ViewModelBase
    {
        private bool isProgress = true; 
        private string gridView = "Collapsed";
        private WifiControlVisualizadorViewModel vmWIFI;
        private NumeroControlVisualizadorViewModel vmNumero;
        private ContatosControlVisualizadorViewModel vmContato;


        public bool IsProgress
        {
            get { return isProgress; }
            private set
            {
                if (isProgress != value)
                {
                    isProgress = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string GridViewLayout
        {
            get { return gridView; }
            private set
            {
                if (gridView != value)
                {
                    gridView = value;
                    RaisePropertyChanged();
                }
            }
        }

        internal int PutHistorico(Gerados gerado)
        {
            int r = 0;
            if(gerado != null)
            {
                string codigo = gerado.Codigo;
                if (Manager.CodigoVerificador.From(codigo).IsWIFI())
                {
                    WifiResult wifi = new WifiResult(codigo);
                    wifi.CodificarWifi();
                    if (wifi.IsValidate)
                    {
                        vmWIFI = ViewModelDispatcher.WifiVisualizadorViewModel;
                        vmWIFI.PutWIFI(wifi);
                        r = 1;
                    }
                }else if (Manager.CodigoVerificador.From(codigo).IsNumeroTelefone())
                {
                    NumeroResult numero = new NumeroResult(codigo);
                    numero.DecodificarNumero();
                    if (numero.IsValidate)
                    {
                        vmNumero = ViewModelDispatcher.NumeroControlVisualizador;
                        vmNumero.PutNumero(numero);
                        r = 2;
                    }
                }else if (Manager.CodigoVerificador.From(codigo).IsContato())
                {
                    ContatoResult contato = new ContatoResult(codigo);
                    contato.DecodificarContato();
                    if (contato.IsValidate)
                    {
                        vmContato = ViewModelDispatcher.ControlVisualizadorViewModel;
                        vmContato.PuContato(contato);
                    }
                }
                PuProgress(true);

            }

            return r;
        }


        private void PuProgress(bool v)
        {
            if (v)
            {
                IsProgress = false;
                GridViewLayout = "Visible";
            }
            else
            {
                IsProgress = true;
                GridViewLayout = "Collapsed";
            }
        }
        private void Toast(String msg)
        {
            Paginas.Root.RootApp.Instance.GetToast(msg, Tools.ModoColor.None);
        }
        

    }

}
