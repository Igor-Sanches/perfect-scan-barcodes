using System; 
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Resources;
using Perfect_Scan.Tools;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Media; 
using Microsoft.Toolkit.Extensions;
using Perfect_Scan.Manager;
using Firebase.Database.Query;
using Windows.UI.Popups;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Perfect_Scan.Paginas
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        private Firebase.Auth.FirebaseAuthClient client;
        private ResourceLoader loader = new ResourceLoader();
        private bool isRegisting = false;
        private Regex regex = new Regex("[0-9-a-z-A-Z-ãçÃÕÍÓÉéõ íó-]+");

        public RegisterPage()
        {
            this.InitializeComponent();
            client = App.GetInstance.GetClient;
            nameUser.TextChanged += (s, a) =>
            {
                GetErroName("");
            };
            emailUser.TextChanged += (s, a) =>
            {
                GetErroName("");
            };
            SenhaUser.PasswordChanged += (s, a) =>
            {
                GetErroSenha("");
            };
            pass1.TextChanged += (s, a) =>
            {
                GetErroPass("");
                if (pass1.Text.Trim().Length == 2)
                {
                    pass2.Focus(FocusState.Keyboard);
                }
            };
            pass2.TextChanged += (s, a) =>
            {
                GetErroPass("");
                if (pass2.Text.Trim().Length == 2)
                {
                    pass3.Focus(FocusState.Keyboard);
                }
                if (pass2.Text.Trim().Equals(""))
                {
                    pass1.Focus(FocusState.Keyboard);
                }
            };
            pass3.TextChanged += (s, a) =>
            {
                GetErroPass("");
                if (pass3.Text.Trim().Length == 2)
                {
                    pass4.Focus(FocusState.Keyboard);
                }
                if (pass3.Text.Trim().Equals(""))
                {
                    pass2.Focus(FocusState.Keyboard);
                }
            };
            pass4.TextChanged += (s, a) =>
            {
                GetErroPass("");
                if (pass4.Text.Trim().Equals(""))
                {
                    pass3.Focus(FocusState.Keyboard);
                }
            };

        }

        private void GetErroSenha(string v)
        {
            erroSenha.Text = v;
        }

        private bool ValidId()
        {
            bool valid = false;
            string email = emailUser.Text.Trim();
            if (!email.Equals(""))
            {
                if (email.IsEmail())
                {
                    valid = true;
                    GetErroEmail("");
                }
                else
                {
                    GetErroEmail(loader.GetString("ErroDigiteIdInvalEmail"));

                }
            }
            else
            {
                GetErroEmail(loader.GetString("ErroDigiteEmail"));
            }

            return valid;
        }

        private void GetErroEmail(string v)
        {
            erroEmail.Text = v;
        }

        private void GetErroName(string v)
        {
            erroName.Text = v;
        }

        private bool IsKeyPass()
        {
            bool valid = false;

            string a1 = pass1.Text.Trim();
            string a2 = pass2.Text.Trim();
            string a3 = pass3.Text.Trim();
            string a4 = pass4.Text.Trim();

            if (a1.Equals("") || a2.Equals("") || a3.Equals("") || a3.Equals(""))
            {
                GetErroPass(loader.GetString("ErroDigiteSenhaKey"));
            }
            else if (a1.Length < 2 || a2.Length < 2 || a3.Length < 2 || a3.Length < 2)
            {
                GetErroPass(loader.GetString("ErroDigiteSenhaKey"));
            }
            else
            {
                valid = true;
                GetErroPass("");
            }

            return valid;
        }

        private void GetErroPass(string v)
        {
            errroPassKey.Text = v;
        }

        private async void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (isRegisting)
            {
                Alert(loader.GetString("aguarde"), ModoColor.Error);
            }
            else
            {
                if (IsText())
                {
                    try
                    {
                        ContentDialog dialog = new ContentDialog();
                        dialog.Background = ((SolidColorBrush)App.Current.Resources["SolidColorPrimaryDark"]);
                        dialog.Content = loader.GetString("certezaSair");
                        dialog.PrimaryButtonText = loader.GetString("btnSair");
                        dialog.SecondaryButtonText = loader.GetString("btnCancelar");
                        dialog.PrimaryButtonClick += (d, f) =>
                        {
                            dialog.Hide();
                            Frame.Navigate(typeof(LoginPage));

                        };
                        dialog.SecondaryButtonClick += (d, f) =>
                        {
                            dialog.Hide();


                        };
                        await dialog.ShowAsync();
                    }
                    catch { }
                }
                else
                {
                    Frame.Navigate(typeof(LoginPage));
                }
            }
        }

        private bool IsText()
        {
            return !emailUser.Text.Trim().Equals("") && !SenhaUser.Password.Trim().Equals("");
        }

        public void GetToast(string msg, ModoColor color) => Tools.Toast.MakerView(Toast, ToastText, ToastIcon, color, msg).Show();

        private void Alert(string v, ModoColor color)
        {
            GetToast(v, color);

        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        { 
            ShowProgress(true);


            if (NetworkInterface.GetIsNetworkAvailable())
            {

                if (ValidName() && ValidId() && ValidSenha() && IsKeyPass())
                {

                    if (client != null)
                    {
                        string email = emailUser.Text.Trim();
                        string senha = SenhaUser.Password.Trim();

                        try
                        {
                            
                            var result = await client.CreateUserWithEmailAndPasswordAsync(email, senha);
                            if (result.User != null)
                            {
                                await result.User.ChangeDisplayNameAsync(nameUser.Text.Trim());
                                string uuid = result.User.Uid;
                                Models.ContaUsuario conta = new Models.ContaUsuario();
                                conta.UUID = uuid;
                                conta.DisplayName = nameUser.Text.Trim();
                                conta.Email = email;
                                conta.KeyPass = $"{pass1.Text.Trim()}-{pass2.Text.Trim()}-{pass3.Text.Trim()}-{pass4.Text.Trim()}";
                                conta.IsIlimited = false;
                                conta.Data = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                                await App.GetInstance.GetDatabase.Child($"{uuid}/Conta").PutAsync(conta);
                                ShowProgress(false);
                                Data.Conta.Put(result);
                                Frame.Navigate(typeof(Root.RootApp));
                                new UpdateTask();
                            }
                 
                        }
                        catch(Exception x)
                        {
                            await new MessageDialog(x.Message).ShowAsync();
                            ShowProgress(false);
                            Alert(loader.GetString("ErrorRegister"), Tools.ModoColor.Error);
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
              
                }
            }
            else
            {
                ShowProgress(false);
                Alert(loader.GetString("ErroConexao"), ModoColor.Error);

            }  
        }

        private bool ValidSenha()
        {
            bool valid = false;

            string senha = SenhaUser.Password.Trim();
            if (!senha.Equals(""))
            {
                if (senha.Length > 6)
                {
                    GetErroSenha("");
                    valid = true;
                }
                else
                {
                    GetErroSenha(loader.GetString("ErroDigiteSenhaCurta"));

                }
            }
            else
            {
                GetErroSenha(loader.GetString("ErroDigiteSenha"));

            }
            return valid;
        }

        private bool ValidName()
        {
            bool valid = false;

            string nome = nameUser.Text.Trim();
            if (!nome.Equals(""))
            {
                if (regex.IsMatch(nome) && nome.Length >= 3)
                {
                    GetErroName("");
                    valid = true;
                }
                else
                {
                    GetErroName(loader.GetString("digitenomeValido"));
                }

            }
            else
            {
                GetErroName(loader.GetString("digitenome"));
            }


            return valid;
        }



        private void ShowProgress(bool v)
        {
            isRegisting = v;
            PanelProgress.Visibility = v ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (isRegisting)
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
