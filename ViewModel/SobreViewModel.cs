using Perfect_Scan.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;

namespace Perfect_Scan.ViewModel
{
    public class SobreViewModel : ViewModelBase
    {
        private RelayCommand btnBar, btnContato, btnPrivacy;
          

        public ICommand BtnPrivacy
        {
            get
            {
                if (btnPrivacy == null)
                {
                    btnPrivacy = new RelayCommand(p => OnBtnPrivacy());
                }
                return btnPrivacy;
            }
        }

        private void OnBtnPrivacy()
        {
             
        }

        public ICommand BtnContato
        {
            get
            {
                if (btnContato == null)
                {
                    btnContato = new RelayCommand(OnBtnContato);
                }
                return btnContato;
            }
        }


        private async void OnBtnContato(object param)
        {
            string tag = param as string;
            string link = "";
            switch (tag)
            {
                case "0":
                    link = "https://api.whatsapp.com/send?phone=5598985766514&text=Olá!";
                    break;
                case "1":
                    link = "tel://5598985766514";
                    break;
                case "2":
                    link = "http://www.facebook.com/igor.dutra.3557";
                    break;
                case "3":
                    link = "http://www.instagram.com/igor_sanches";
                    break;
                case "4":
                    link = "http://www.twitter.com/igordutra2014";
                    break;
            }
            await Launcher.LaunchUriAsync(new Uri(link));
        }

        public ICommand BtnBar
        {
            get
            {
                if (btnBar == null)
                {
                    btnBar = new RelayCommand(OnBtn);
                }
                return btnBar;
            }
        }

        private void OnBtn(object param)
        {
            var v = ViewModelDispatcher.RootView;
            string frame = param as string;
            v.NavigationFrame(frame);

        }

        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;
        public string Version => ApplicationID.VersaoApp;
        public string NameApp => ApplicationID.AppNome; 
        public string DeveloperName => ApplicationID.DisplayDeveloperName;
    }
}
