using Perfect_Scan.ViewModel.Visualizador;
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

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace Perfect_Scan.Controle.Visualizador
{
    public sealed partial class WifiControlVisualizador : UserControl
    {
        private WifiControlVisualizadorViewModel vm;

        public WifiControlVisualizador()
        {
            this.InitializeComponent();
            vm = ViewModel.ViewModelDispatcher.WifiVisualizadorViewModel;
            DataContext = vm;

        }
    }
}
