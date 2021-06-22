using Perfect_Scan.ViewModel.Backups;
using Perfect_Scan.ViewModel.Visualizador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_Scan.ViewModel
{
    public static class ViewModelDispatcher
    {
        private static Lazy<GeradorViewModel> geradorView;
        private static Lazy<ContaViewModel> contaViewModel;
        private static Lazy<GerarNumeroViewModel> dialerViewModel;
        private static Lazy<ContactsViewModel> contactsViewModel;
        private static Lazy<GeralViewModel> geralViewModel;
        private static Lazy<WifiControlViewModel> wifiControlViewModel;
        private static Lazy<ProdutoEncomendaViewModel> produtoEncomendaViewModel;
        private static Lazy<ImageEscannearViewModel> ImageEscannearViewModel;
        private static Lazy<EscaneadosViewModel> escaneadosViewModel;
        private static Lazy<GeradosViewModel> geradossViewModel;
        private static Lazy<BackupViewModel> backupViewModel;
        private static Lazy<BackupInfoViewModel> backupInfoView;
        private static Lazy<RootViewModel> rootView;
        private static Lazy<SobreViewModel> SobreViewModel;
        private static Lazy<BackupsCreateViewModel> backupsCreateViewModel;
        private static Lazy<VisualizadorViewModel> visualizadorView;
        private static Lazy<WifiControlVisualizadorViewModel> WifiVisualizadorView;
        private static Lazy<NumeroControlVisualizadorViewModel> numeroControlVisualizador;
        private static Lazy<ContatosControlVisualizadorViewModel> contatosControlVisualizadorView;
        private static Lazy<HistoricosViewModel> historicosViewModel;

        public static HistoricosViewModel HistoricosViewModel
        {
            get
            {
                if (historicosViewModel == null)
                {
                    historicosViewModel = new Lazy<HistoricosViewModel>();
                }
                return historicosViewModel.Value;
            }
        }

        public static ContatosControlVisualizadorViewModel ControlVisualizadorViewModel
        {
            get
            {
                if (contatosControlVisualizadorView == null)
                {
                    contatosControlVisualizadorView = new Lazy<ContatosControlVisualizadorViewModel>();
                }
                return contatosControlVisualizadorView.Value;
            }
        }

        public static NumeroControlVisualizadorViewModel NumeroControlVisualizador
        {
            get
            {
                if(numeroControlVisualizador == null)
                {
                    numeroControlVisualizador = new Lazy<NumeroControlVisualizadorViewModel>();
                }
                return numeroControlVisualizador.Value;
            }
        }

        public static WifiControlVisualizadorViewModel WifiVisualizadorViewModel
        {
            get
            {
                if (WifiVisualizadorView == null)
                {
                    WifiVisualizadorView = new Lazy<WifiControlVisualizadorViewModel>();
                }
                return WifiVisualizadorView.Value;
            }
        }

        public static VisualizadorViewModel VisualizadorView
        {
            get
            {
                if (visualizadorView == null)
                {
                    visualizadorView = new Lazy<VisualizadorViewModel>();
                }
                return visualizadorView.Value;
            }
        }

        public static GeradorViewModel GeradorView
        {
            get
            {
                if(geradorView == null)
                {
                    geradorView = new Lazy<GeradorViewModel>();
                }
                return geradorView.Value;
            }
        }

        public static ContaViewModel ContaView
        {
            get
            {
                if (contaViewModel == null)
                {
                    contaViewModel = new Lazy<ContaViewModel>();
                }
                return contaViewModel.Value;
            }
        }

        public static SobreViewModel SobreView
        {
            get
            {
                if (SobreViewModel == null)
                {
                    SobreViewModel = new Lazy<SobreViewModel>();
                }
                return SobreViewModel.Value;
            }
        }

        public static RootViewModel RootView
        {
            get
            {
                if (rootView == null)
                {
                    rootView = new Lazy<RootViewModel>();
                }
                return rootView.Value;
            }
        }

        public static BackupsCreateViewModel BackupsCreateView
        {
            get
            {
                if (backupsCreateViewModel == null)
                {
                    backupsCreateViewModel = new Lazy<BackupsCreateViewModel>();
                }
                return backupsCreateViewModel.Value;
            }
        }

        public static BackupViewModel BackupView
        {
            get
            {
                if (backupViewModel == null)
                {
                    backupViewModel = new Lazy<BackupViewModel>();
                }
                return backupViewModel.Value;
            }
        }

        public static GeradosViewModel GeradosView
        {
            get
            {
                if (geradossViewModel == null)
                {
                    geradossViewModel = new Lazy<GeradosViewModel>();
                }
                return geradossViewModel.Value;
            }
        }

        public static EscaneadosViewModel EscaneadosView
        {
            get
            {
                if (escaneadosViewModel == null)
                {
                    escaneadosViewModel = new Lazy<EscaneadosViewModel>();
                }
                return escaneadosViewModel.Value;
            }
        }

        public static ImageEscannearViewModel ImageEscannear
        {
            get
            {
                if(ImageEscannearViewModel == null)
                {
                    ImageEscannearViewModel = new Lazy<ImageEscannearViewModel>();
                }
                return ImageEscannearViewModel.Value;
            }
        }

        public static ProdutoEncomendaViewModel ProdutoEncomendaView
        {
            get
            {
                if(produtoEncomendaViewModel == null)
                {
                    produtoEncomendaViewModel = new Lazy<ProdutoEncomendaViewModel>();
                }
                return produtoEncomendaViewModel.Value;
            }
        }

        public static WifiControlViewModel WifiViewModel
        {
            get
            {
                if(wifiControlViewModel == null)
                {
                    wifiControlViewModel = new Lazy<WifiControlViewModel>();
                }
                return wifiControlViewModel.Value;
            }
        }

        public static GeralViewModel GeralEditorViewModel
        {
            get
            {
                if(geralViewModel == null)
                {
                    geralViewModel = new Lazy<GeralViewModel>();
                }
                return geralViewModel.Value;
            }
        }

        public static GerarNumeroViewModel DialerViewModel
        {
            get
            {
                if (dialerViewModel == null)
                {
                    dialerViewModel = new Lazy<GerarNumeroViewModel>();
                }
                return dialerViewModel.Value;
            }
        }

        public static BackupInfoViewModel BackupInfoView
        {
            get
            {
                if (backupInfoView == null)
                {
                    backupInfoView = new Lazy<BackupInfoViewModel>();
                }
                return backupInfoView.Value;
            }
        }

        internal static ContactsViewModel ContactsViewModel
        {
            get
            {
                if (contactsViewModel == null)
                {
                    contactsViewModel = new Lazy<ContactsViewModel>();
                }
                return contactsViewModel.Value;
            }
        }
    }
}