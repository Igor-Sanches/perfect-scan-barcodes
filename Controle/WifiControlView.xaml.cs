using Perfect_Scan.Manager;
using Perfect_Scan.Tools;
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
    public sealed partial class WifiControlView : UserControl
    {
        public Strings StringKey = new Strings();
        private Frame frame;
        private ViewModel.WifiControlViewModel vm;
        public WifiControlView(Frame frame)
        {
            this.InitializeComponent();
            this.frame = frame;
            vm = ViewModel.ViewModelDispatcher.WifiViewModel;
            DataContext = vm;
        } 

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            vm.SetCompomentes(WifiSelector, WifiName, WifiKey, frame);
        }

        private void WifiLista_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender as Button; 
            WiFiNetworkDisplay wiFi = btn.DataContext as WiFiNetworkDisplay;
            vm.ItemClick(wiFi, WifiSelector, WifiName, WifiKey);
        }
    }
}
