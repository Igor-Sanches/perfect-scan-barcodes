using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZXing;
using ZXing.Mobile; 
using Perfect_Scan.Models;
using System.Net.NetworkInformation;
using Perfect_Scan.Manager;
using Windows.ApplicationModel.Resources;
using System.Threading.Tasks;
using Perfect_Scan.Tools;
using System.Threading;
using Microsoft.Toolkit.Extensions;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Perfect_Scan.Paginas
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private Firebase.Auth.FirebaseAuthClient client;
        private ResourceLoader loader = new ResourceLoader();
        private bool isLoging = false;
        private static LoginPage instance;
        public static LoginPage Instance { get { return instance; } }

        public LoginPage()
        {
            this.InitializeComponent();
            instance = this;
            client = App.GetInstance.GetClient;
            emailUser.TextChanged += (s, a) =>
            {
                GetErroId("");
            };
            SenhaUser.PasswordChanged += (s, a) =>
            {
                GetErroPass("");
            };
            emailResetUser.TextChanged += (s, a) =>
            {
                GetErroEmailReset("");
            };

        }

        private bool ValidId()
        {
            bool valid = false;
            string email = emailUser.Text.Trim();
            if (!email.Equals(""))
            {
                if (Email.Validor(email))
                {
                    valid = true;
                    GetErroId("");
                }
                else
                {
                    GetErroId(loader.GetString("ErroDigiteIdInvalEmail"));

                }
            }
            else
            {
                GetErroId(loader.GetString("ErroDigiteEmail"));
            }

            return valid;
        }

        private void GetErroId(string v)
        {
            erroEmail.Text = v;
        }

        private bool ValidSenha()
        {
            bool valid = false;

            string senha = SenhaUser.Password.Trim();
            if (!senha.Equals(""))
            {
                if (senha.Length > 6)
                {
                    GetErroPass("");
                    valid = true;
                }
                else
                {
                    GetErroPass(loader.GetString("ErroDigiteSenhaCurta"));

                }
            }
            else
            {
                GetErroPass(loader.GetString("ErroDigiteSenha"));

            }
            return valid;
        }

        private void GetErroPass(string v)
        {
            erroSenha.Text = v;
        }

        private async void ResetKey_Click(object sender, RoutedEventArgs e)
        {
            if (isLoging)
            {
                Alert(loader.GetString("aguarde"), ModoColor.Error);
            }
            else
            {
                if(!emailUser.Text.Trim().Equals("") && emailResetUser.Text.Trim().Equals(""))
                {
                    emailResetUser.Text = emailUser.Text.Trim();
                     
                }
                GetErroEmailReset("");

                ContentDialog dialog = PanelReset;
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);
                await dialog.ShowAsync();
            }
        }

        private void GetErroEmailReset(string v)
        {
            erroResetEmail.Text = v;
        }

        private bool ValidEmailReset()
        {
            bool valid = false;
            string id = emailResetUser.Text.Trim();
            if (id.Equals(""))
            {
                GetErroEmailReset(loader.GetString("ErroDigiteEmail"));
            }
            else if (id.IsEmail())
            {
                GetErroEmailReset(loader.GetString("ErroDigiteIdInvalEmail"));
            }
            else
            {
                valid = true;
                GetErroEmailReset("");
            }
            return valid;
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            if (isLoging)
            {
                Alert(loader.GetString("aguarde"), ModoColor.Error);
            }
            else
            {
                if (IsText())
                {
                    ContentDialog dialog = new ContentDialog();
                    dialog.Background = ((SolidColorBrush)App.Current.Resources["SolidColorPrimaryDark"]);
                    dialog.Content = loader.GetString("certezaSair");
                    dialog.PrimaryButtonText = loader.GetString("btnSair");
                    dialog.SecondaryButtonText = loader.GetString("btnCancelar");
                    dialog.PrimaryButtonClick += (d, f) =>
                    {
                        dialog.Hide(); 
                        Frame.Navigate(typeof(RegisterPage));

                    };
                    dialog.SecondaryButtonClick += (d, f) =>
                    {
                        dialog.Hide();


                    };
                    await dialog.ShowAsync();
                }
                else
                {
                    Frame.Navigate(typeof(RegisterPage));
                }
            }
        }

        private bool IsText()
        {
            return !emailUser.Text.Trim().Equals("") && !SenhaUser.Password.Trim().Equals("");
        }

        public void GetToast(string msg, ModoColor color) => Toast.MakerView(ToastGrid, ToastText, ToastIcon, color, msg).Show();

        private void Alert(string v, ModoColor color)
        {
            GetToast(v, color);
        }

        private async void Enter_Click(object sender, RoutedEventArgs e)
        {
            ShowProgress(true);

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (ValidId() && ValidSenha())
                {

                    if (client != null)
                    {
                        string email = emailUser.Text.Trim();
                        string senha = SenhaUser.Password.Trim();

                        try
                        {
                            var result = await client.SignInWithEmailAndPasswordAsync(email, senha);
                            if (result.User != null)
                            {
                                ShowProgress(false);
                                Data.Conta.Put(result);
                                Frame.Navigate(typeof(Root.RootApp));
                                new UpdateTask();
                            }
                        }
                        catch 
                        {
                            ShowProgress(false);
                            GetToast(loader.GetString("ErrorLogar"), Tools.ModoColor.Error);
                        }
                    }

                }
                else
                {
                    ShowProgress(false);
                }
             }
            else
            {
                ShowProgress(false);
                Alert(loader.GetString("ErroConexao"), ModoColor.Error);

            } 
        }

        private void ShowProgress(bool v)
        {
            isLoging = v;
            PanelProgress.Visibility = v ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            PanelReset.Hide();
        }

        private async void Recuperar_Click(object sender, RoutedEventArgs e)
        {
            PanelReset.Hide();
            ShowProgress(true);

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (ValidEmailReset())
                {
                    if (client != null)
                    {
                        string email = emailResetUser.Text.Trim();

                        try
                        {
                            bool success = false;
                            await client.ResetEmailPasswordAsync(email).ContinueWith((result) =>
                            {
                                success = result.IsCompleted; 
                            });
                            if (success)
                            {

                            }

                        }
                        catch 
                        {
                            ShowProgress(false);
                            Alert(loader.GetString("ErrorLogarReset"), Tools.ModoColor.Error);
                        }

                    }

                }
                else
                {
                    ShowProgress(false);
                }
            }
            else
            {
                ShowProgress(false);
                Alert(loader.GetString("ErroConexao"), ModoColor.Error);

            }
        }

        private void AlertMS(string v, ModoColor error)
        {
            ShowProgress(false);
            emailResetUser.Text = v;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (isLoging)
            {
                Alert(loader.GetString("aguarde"), ModoColor.Error);
            }
            else
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }
    }
}
