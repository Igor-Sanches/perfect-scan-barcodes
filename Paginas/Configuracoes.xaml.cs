using Perfect_Scan.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Perfect_Scan.Paginas
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Configuracoes : Page
    {
        private Settings.Settings settings;
        private ResourceLoader loader;
        private Key chave = new Key();

        public Configuracoes()
        {
            this.InitializeComponent();
        
            settings = new Settings.Settings();
            loader = new ResourceLoader();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Data.Data.AppViewPager = "Configuracoes";
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            try
            {
                ToggleSwitch btn = (ToggleSwitch)sender as ToggleSwitch;
                settings.Put(btn.Tag.ToString(), btn.IsOn);
                NarrarMassa.IsEnabled = settings.IsFalarResultadoAoEscanearMassa;
                if (btn.Tag.ToString().Equals(chave.APP_SETTINGS_APTAR))
                {
                    if (BeepScan.IsOn)
                    {
                        if (VibrateScan.IsOn)
                        {
                            VibrateScan.IsOn = false;
                      settings.Put(chave.APP_SETTINGS_VIBRAR, false);
                  }
                    }
                }
                if (btn.Tag.ToString().Equals(chave.APP_SETTINGS_VIBRAR))
                {
                    if (VibrateScan.IsOn)
                    { 

                        if (BeepScan.IsOn)
                        {


                            BeepScan.IsOn = false;

                            settings.Put(chave.APP_SETTINGS_APTAR, false);

                        }
                    }
                }  
            }

            catch 
            {
                 
            }
        }

        
        private async void LaserColor_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                ScrollViewer view = new ScrollViewer();
                StackPanel panel = new StackPanel();
                ContentDialog dialog = new ContentDialog();
                dialog.Title = loader.GetString("select");
                dialog.Foreground = ((Brush)App.Current.Resources["Titulo"]);
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);

                string[] strings = new string[] { "laser_default", "laser_blue_bright", "laser_blue_dark", "laser_blue_light", "laser_green_dark", "laser_green_light", "laser_orange_dark", "laser_orange_light", "laser_purple", "laser_red_dark", "laser_red_light" };
                for (int i = 0; i < strings.Length; i++)
                {
                    RadioButton box = new RadioButton();
                    box.Style = ((Style)App.Current.Resources["RadioButtonStyle1"]);
                    box.Content = loader.GetString(strings[i]);
                    box.Tag = i;
                    box.IsChecked = IsCheckedValue(i);
                    box.Click += (s, a) =>
                    {
                        settings.Put(chave.APP_SETTINGS_LASER_COR, (int)box.Tag);
                        dialog.Hide();
                        LaserColor.Content = settings.GetLaserColor;

                    };
                    panel.Children.Add(box);
                }
                view.Content = panel;
                dialog.Content = view;
                dialog.CloseButtonStyle = ((Style)App.Current.Resources["ButtonStyleContent"]);
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                await dialog.ShowAsync();
            }
            catch (Exception x) { Root.RootApp.Instance.GetToast(x.Message); }

        }

        private bool? IsCheckedValue(int i)
        {
            return settings.GetLaserColorValue == i;
        }

        private bool? IsChecked(int i)
        {
            return settings.GetTipoCodigoValue == i; 
        }

        private string Get(string v)
        {
            return loader.GetString(v);
        }

        private async void NarrarMassa_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                ScrollViewer view = new ScrollViewer();
                StackPanel panel = new StackPanel();
                ContentDialog dialog = new ContentDialog();
                dialog.Title = loader.GetString("select");
                dialog.Foreground = ((Brush)App.Current.Resources["Titulo"]);
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);

                string[] strings = new string[] { "tipoCodigo", "formatoCodigo", "resultado" };
                for (int i = 0; i < strings.Length; i++)
                {
                    RadioButton box = new RadioButton();
                    box.Style = ((Style)App.Current.Resources["RadioButtonStyle1"]);
                    box.Content = loader.GetString(strings[i]);
                    box.Tag = i;
                    box.IsChecked = IsChecked(i);
                    box.Click += (s, a) =>
                    {
                        settings.Put(chave.APP_SETTINGS_TIPO_NARRAR, (int)box.Tag);
                        dialog.Hide();
                        NarrarMassa.Content = settings.GetTipoCodigo;

                    };
                    panel.Children.Add(box);
                }
                view.Content = panel;
                dialog.CloseButtonStyle = ((Style)App.Current.Resources["ButtonStyleContent"]);
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                dialog.Content = view;
                await dialog.ShowAsync();
            }
            catch(Exception x) { Root.RootApp.Instance.GetToast(x.Message); }
        } 
    }
}