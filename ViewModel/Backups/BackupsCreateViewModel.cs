using Perfect_Scan.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls; 

namespace Perfect_Scan.ViewModel.Backups
{
    public class BackupsCreateViewModel : ViewModelBase
    {
        BackupManager backup = new BackupManager();
        private ResourceLoader loader = new ResourceLoader();
        private Regex regex = new Regex("[0-9-a-z-A-Z-ãçÃÕÍÓÉéõ íó_-]+");
        private string backupName = "", remonearError;
        private RelayCommand criarBackup, cancelCommand, checkGCommand, checkSCommand;
        private bool isBtnEnabled = false, isCheckedG = false, isEnabledG = false, isCheckedS = false, isEnabledS = false;

        public ICommand CommandCriar
        {
            get
            {
                if (criarBackup == null)
                {
                    criarBackup = new RelayCommand(p => OnCriar());
                }
                return criarBackup;
            }
        }

        private void OnCriar()
        {
            
            if (NomeValidor())
            {
                IsEnabledBtn = false;
                backup.CriarBakup(backupName, isCheckedG, isCheckedS, this);
            }
        }

        private bool NomeValidor()
        {
            bool pass = false;
            if (!backupName.Equals(""))
            {
                if (!regex.IsMatch(backupName))
                {
                    RemonearError = loader.GetString("enterNameValid");
                }
                else
                {
                    pass = true;
                }
            }
            else
            {
                RemonearError = loader.GetString("enterName");
               
            }

            return pass;
        }

        public ICommand CanecelComanl
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new RelayCommand(p => OnCancel());
                }
                return cancelCommand;
            }
        }

        private void OnCancel()
        {
            try
            {
                Paginas.Root.RootApp.Instance.GoBack();
                //if (frame.CanGoBack)
                //{
                ////    frame.GoBack();
                //}
            }
            catch { }
        }

        public ICommand CheckedSCommand
        {
            get
            {
                if (checkSCommand == null)
                {
                    checkSCommand = new RelayCommand(p => OnChckedS());
                }
                return checkSCommand;
            }
        }

        public ICommand CheckedGCommand
        {
            get
            {
                if (checkGCommand == null)
                {
                    checkGCommand = new RelayCommand(p => OnChckedG());
                }
                return checkGCommand;
            }
        }

        private void OnChckedS()
        {
            isCheckedS = !isCheckedS;
            BtnState();
        }

        public void BtnState()
        {
            IsEnabledBtn = isCheckedG || isCheckedS;
        }

        private void OnChckedG()
        {
            isCheckedG = !isCheckedG;
            BtnState();
        }

        public bool IsEnabledBtn
        {
            get { return isBtnEnabled; }
            private set
            {
                if (isBtnEnabled != value)
                {
                    isBtnEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsEnabledG
        {
            get { return isEnabledG; }
            private set
            {
                if (isEnabledG != value)
                {
                    isEnabledG = value;
                    RaisePropertyChanged();
                }
            }
        }
         
        public bool IsCheckedG
        {
            get { return isCheckedG; }
            private set
            {
                if (isCheckedG != value)
                {
                    isCheckedG = value;
                    RaisePropertyChanged();
                }
            }
        }

        internal void Shot(TextBox editName)
        {
            editName.TextChanged += (sender, args) =>
              {
                  backupName = editName.Text;
                  RemonearError = "";
              };
            var data = DateTime.Now.ToString("dd_MM_yyyy HH mm");
            backupName =  $"Backup {data}";
            bool isG = backup.IsGerados;
            IsEnabledG = isG;
            IsCheckedG = isG;
            bool isS = backup.IsEscaneados;
            IsEnabledS = isS;
            IsCheckedS = isS;

            BtnState();
        }

        void Toast(string msg)
        {
            Paginas.Root.RootApp.Instance.GetToast(msg);
        }

        public bool IsEnabledS
        {
            get { return isEnabledS; }
            private set
            {
                if (isEnabledS != value)
                {
                    isEnabledS = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsCheckedS
        {
            get { return isCheckedS; }
            private set
            {
                if (isCheckedS != value)
                {
                    isCheckedS = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string RemonearError
        {
            get { return remonearError; }
            private set
            {
                if (remonearError != value)
                {
                    remonearError = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string BackupName
        {
            get
            {
                var data = DateTime.Now.ToString("dd_MM__yyyy HH mm");
                return $"Backup {data}";
            }
            private set
            {
                if (backupName != value)
                {
                    backupName = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
