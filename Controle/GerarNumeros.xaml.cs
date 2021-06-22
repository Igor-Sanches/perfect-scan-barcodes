using Perfect_Scan.ViewModel;
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

namespace Perfect_Scan.Controle
{
    public sealed partial class GerarNumeros : UserControl
    {
        GerarNumeroViewModel vm; 
        public GerarNumeros()
        {
            this.InitializeComponent(); 
            vm = (GerarNumeroViewModel)DataContext;
        
        }

        private void OnDialPadHolding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            Button button = (Button)sender;
            if ((vm != null) && (e.HoldingState == Windows.UI.Input.HoldingState.Started))
            {
                vm.ProcessDialPadHolding.Execute(button.Tag);
            }

        }
         
    }
}
