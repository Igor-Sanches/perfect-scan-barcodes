using Perfect_Scan.Models;
using Perfect_Scan.Paginas;
using Perfect_Scan.Paginas.Root;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace Perfect_Scan
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    { 
        private ResourceLoader loader = new ResourceLoader(); 
        private string[] content;
        private bool isGo = false;
        

        public MainPage()
        {
            this.InitializeComponent();
             
            content = new string[] 
            {
                loader.GetString("cloud_database"),
                loader.GetString("backup_storage"),
                loader.GetString("download_icon"),
                loader.GetString("scan_gerador")
            };
            cloud_database.Text = content[0];
            backup_storage.Text = content[1];
            download_icon.Text = content[2];
            scan_gerador.Text = content[3];
            splitViewNext(0);
        }

        private void splitViewNext(double v)
        {
            point1.Background = v == 0 ? ((SolidColorBrush)App.Current.Resources["CorPrimaria"]) : ((SolidColorBrush)App.Current.Resources["CorSegundaria"]);
            point2.Background = v == 1 ? ((SolidColorBrush)App.Current.Resources["CorPrimaria"]) : ((SolidColorBrush)App.Current.Resources["CorSegundaria"]);
            point3.Background = v == 2 ? ((SolidColorBrush)App.Current.Resources["CorPrimaria"]) : ((SolidColorBrush)App.Current.Resources["CorSegundaria"]);
            point4.Background = v == 3 ? ((SolidColorBrush)App.Current.Resources["CorPrimaria"]) : ((SolidColorBrush)App.Current.Resources["CorSegundaria"]);
        
            if(v == 3)
            {
                btnGoIt.IsEnabled = false;
                //btnGoIt.Content = loader.GetString("finalizar");
            }
            else
            {

                btnGoIt.Content = loader.GetString("proximo");
                btnGoIt.IsEnabled = true;
            }
            isGo = v == 3;
        }

        private void BtnGoIt_Click(object sender, RoutedEventArgs e)
        { 
            if (splitView.SelectedIndex == 0)
            {
                splitView.SelectedIndex = 1;
            }else if(splitView.SelectedIndex == 1)
            {
                splitView.SelectedIndex = 2;
            }
            else if (splitView.SelectedIndex == 2)
            {
                splitView.SelectedIndex = 3;
            } 
            
        }

        private void SplitView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            splitViewNext(splitView.SelectedIndex);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
 