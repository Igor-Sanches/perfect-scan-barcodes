using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Resources;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Perfect_Scan.Manager
{
    public class ApplicationID
    {
        private static ResourceLoader loader = new ResourceLoader(); 
        private static EmailMessage objEmail = new EmailMessage();

        public static string AppNome => Package.Current.DisplayName;
        public static string VersaoApp => $"{loader.GetString("version")}: {Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}.{Package.Current.Id.Version.Revision}";

        public static string AppId = "";

        public static string DisplayDeveloperName = "Igor Dutra Sanches";
        private static Uri RateOurReview => new Uri($"ms-windows-store://review/?ProductId={AppId}");
        private static Uri MaisAppsWindows => new Uri($"ms-windows-store://publisher/?name={DisplayDeveloperName}");
        private static Uri MaisAppsAndroid => new Uri("ms-windows-store://publisher/?name=Igor Dutra Sanches");
        private static Uri MaisApps => new Uri("https://meu-windows10channel.blogspot.com/2019/10/meus-apps.html?m=1");
         
        public async static void GoLaunchRateOurReview()
        {
            //LauncherOptions options = new LauncherOptions();

            await Launcher.LaunchUriAsync(RateOurReview);
        }

        public async static void GoLaunchMaisAppsWindows()
        {
            await Launcher.LaunchUriAsync(MaisAppsWindows);
        }

        public async static void GoLaunchMaisAppsAndroid()
        {
            await Launcher.LaunchUriAsync(MaisAppsAndroid);
        }

        public async static void OnSuporte()
        {
            try
            {
                var mensagem = loader.GetString("mensagemFeed");
                objEmail.Subject = mensagem; //Titulo do Feedback
                objEmail.To.Add(new EmailRecipient("perfectscan_feedback@outlook.com")); //E-Mail que recebera o Feedback
                await EmailManager.ShowComposeNewEmailAsync(objEmail); //Pega todas as imformações e mande o feedback
            }
            catch { }

        }

        public async static void GoLaunchMaisApps()
        {
            await Launcher.LaunchUriAsync(MaisApps);
        }

        internal static void Protocol(Frame _frame, IActivatedEventArgs args)
        {
            try
            {
                ProtocolActivatedEventArgs protocol = args as ProtocolActivatedEventArgs;
                string url = protocol.Uri.ToString();
                if (url.StartsWith("perfect-scan://"))
                {
                    int lenght = "perfect-scan://".Length;
                    string replace = url.Substring(lenght, url.Length - lenght);

                    if (replace.StartsWith("editor/?code="))
                    {
                        int lenghtEditor = "editor/?code=".Length;
                        string codigo = replace.Substring(lenghtEditor, replace.Length - lenghtEditor);
                        string[] tel = new string[] { "0", codigo };
                        _frame.Navigate(typeof(Paginas.GerarCodigos), tel);
                    }
                    else if (replace.StartsWith("gerador/?code="))
                    {

                    }
                    else if (replace.StartsWith("image/?"))
                    {
                        int lenghtEditor = "image/?source=".Length;
                        int lenghtEditorpath = "image/?path-file=".Length;
                        string path = "";

                        path = replace.Substring(lenghtEditorpath, replace.Length - lenghtEditorpath);
                        _frame.Navigate(typeof(Paginas.EscanearImagem), path);
                    }

                    else if (replace.StartsWith("scan/?init"))
                    {
                        _frame.Navigate(typeof(Paginas.EscaneamentoPage));
                    }
                    else
                    {
                        _frame.Navigate(typeof(Paginas.Inicio));
                    }
                }
                else
                {
                    int lenght = "tel://".Length;
                    string replace = url.Substring(lenght, url.Length - lenght);
                    string[] tel = new string[] { "1", replace };
                    _frame.Navigate(typeof(Paginas.GerarCodigos), tel);


                }
            }
            catch { _frame.Navigate(typeof(Paginas.Inicio)); }
        }
    }
}
