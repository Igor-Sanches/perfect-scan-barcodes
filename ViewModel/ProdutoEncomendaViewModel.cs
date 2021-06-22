using Perfect_Scan.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Perfect_Scan.ViewModel
{
    public class ProdutoEncomendaViewModel : ViewModelBase
    {
        private Frame frame;
        private bool buttonGerar = false;
        private ResourceLoader loader = new ResourceLoader();
        private string digiteProduto = "", textButton = "", visibilityCopy = "Collapsed", dialerEncomendaCorreios = "", dialerEncomendaSY = "", dialerEncomendaNL = "", dialerEncomendaSYBAA = "", dialerEncomendaSYBAB = "", dialerEncomendaYDYW = "", dialerEncomendaYT = "", dialerEncomendaLB = "", dialerVisibility = "Collapsed", dialerNumberVisibility = "Visible";
        private ComboBox encTypes;  
        private string produtoVisibility = "Visible", encomendaVisibility = "Collapsed", dialerEncomendaCorreiosVisibility = "Visible", dialerEncomendaSYVisibility = "Collapsed", dialerEncomendaNLVisibility = "Collapsed", dialerEncomendaSYBAAVisibility = "Collapsed", dialerEncomendaSYBABVisibility = "Collapsed", dialerEncomendaYDYWVisibility = "Collapsed", dialerEncomendaYTVisibility = "Collapsed", dialerEncomendaLBVisibility = "Collapsed";
        private Regex singaLetra = new Regex("[A-Z]+"); 

        private RelayCommand btnGerar, processDialPad, processDialDeletePad, processDialCopyPad, processInfoCommand, processCopyCommand, processDialPadNumberCommand, processDialPadLetrasCommand, processDialDelete;

        public string DigiteProduto
        {
            get { return digiteProduto; }
            private set
            {
                if(digiteProduto != value)
                {
                    digiteProduto = value;
                    RaisePropertyChanged();
                }
            }
        }


        public ICommand GerarCommand
        {
            get
            {
                if (btnGerar == null)
                {
                    btnGerar = new RelayCommand(p => this.OnGerar());
                }
                return btnGerar;
            }
        }

        private void OnGerar()
        { 
            string codigo = "";
            bool pass = true;
            if (Data.Data.IsProduto)
            {
                   codigo = digiteProduto; 
            }
            else
            {
                switch (GetIndex)
                {
                    case 0:
                        {
                            if (dialerEncomendaCorreios.Length > 0)
                            {
                                codigo = dialerEncomendaCorreios;
                            }
                        }
                        break;
                    case 1:
                        {
                            if (dialerEncomendaLB.Length > 0)
                            {
                                codigo = $"LP{dialerEncomendaLB}";
                            }
                        }
                        break;
                    case 2:
                        {
                            if (dialerEncomendaYT.Length > 0)
                            {
                                codigo = $"YT{dialerEncomendaYT}";
                            }
                        }
                        break;
                    case 3:
                        {
                            if (dialerEncomendaYDYW.Length > 0)
                            { 
                                codigo = $"YD{dialerEncomendaYDYW}YW";
                            }
                        }
                        break;
                    case 4:
                        {
                            if (dialerEncomendaSYBAA.Length > 0)
                            {
                                codigo = $"SYBAA{dialerEncomendaSYBAA}";
                            }
                        }
                        break;
                    case 5:
                        {
                            if (dialerEncomendaSYBAB.Length > 0)
                            {
                                codigo = $"SYBAB{dialerEncomendaSYBAB}";
                            }
                        }
                        break;
                    case 6:
                        {
                            if (dialerEncomendaNL.Length > 0)
                            {
                                codigo = $"NL{dialerEncomendaNL}";
                            }
                        }
                        break;
                    case 7:
                        {
                            if (dialerEncomendaSY.Length > 0)
                            {
                                codigo = $"SY{dialerEncomendaSY}";
                            }
                        }
                        break;
                }
                codigo = $"package:{codigo}";
            }

            if (pass)
            {
                Gerador.With(codigo).GerarCodigo(Data.Data.Codigo, frame);
            }
        } 

        public string ContentButtonGerar
        {
            get
            {
                return Data.Data.IsProduto ? loader.GetString("loaderProdutoContent") : loader.GetString("loaderEnomendaContent");
            }
            private set
            {
                if(textButton != value)
                {
                    textButton = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ProdutoVisibility
        {
            get { return Data.Data.IsProduto ? "Visible" : "Collapsed"; }
            private set
            {
                if (produtoVisibility != value)
                {
                    produtoVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void ProdutoEnabled()
        {
            ProdutoVisibility = "Visible";
            EncomendaVisibility = "Collapsed";
            CopyFunctionVisible = "Collapsed"; 
            ContentButtonGerar = loader.GetString("loaderProdutoContent");
        }

        private void EncomendaEnabled()
        {
            ProdutoVisibility = "Collapsed";
            EncomendaVisibility = "Visible";
            ContentButtonGerar = loader.GetString("loaderEnomendaContent");
        }

        public string EncomendaVisibility
        {
            get { return Data.Data.IsProduto ? "Collapsed" : "Visible"; }
            private set
            {
                if(encomendaVisibility != value)
                {
                    encomendaVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ToggleButtonOnChecked
        {
            get { return Data.Data.IsProduto; }
            private set { if (Data.Data.IsProduto != value) { Data.Data.IsProduto = value; RaisePropertyChanged(); } }
        }

        public string CopyFunctionVisible
        {
            get { return visibilityCopy; }
            private set
            {
                if(visibilityCopy != value)
                {
                    visibilityCopy = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand ProcessDialCopyPad
        {
            get
            {
                if (processDialCopyPad == null)
                { processDialCopyPad = new RelayCommand(p=>this.ProdutoCopyDigitado()); }
                return processDialCopyPad;

            }
        }

        public void ProdutoCopyDigitado()
        {
            string txtCopy = "";
            if (!digiteProduto.Equals(""))
            {
                txtCopy = digiteProduto;
            }

            if (!txtCopy.Equals(""))
            {
                var dataPackage = new DataPackage();
                dataPackage.SetText(txtCopy);
                Clipboard.SetContent(dataPackage);
                Paginas.Root.RootApp.Instance.GetToast(loader.GetString("CopyText"), Tools.ModoColor.Succes);
            }
            else
            {
                Paginas.Root.RootApp.Instance.GetToast(loader.GetString("CopyClipboardError"), Tools.ModoColor.Error);
            }
        }

        public ICommand ProcessDialPad
        {
            get
            {
                if (processDialPad == null)
                { processDialPad = new RelayCommand(this.ProdutoDigitado); }
                return processDialPad;

            }
        }

        public ICommand ProcessDialDeletePad
        {
            get
            {
                if (processDialDeletePad == null)
                { processDialDeletePad = new RelayCommand(p=>this.ProdutoDeleteDigitado()); }
                return processDialDeletePad;

            }
        }

        public void ProdutoDeleteDigitado()
        {
            if (digiteProduto.Length > 0)
            {
                DigiteProduto = DigiteProduto.Remove(digiteProduto.Length - 1);
                StatusProduto();
            }
        }

        public void ProdutoDeleteAllDigitado()
        {
            if (digiteProduto.Length > 0)
            {
                DigiteProduto = "";
                StatusProduto();
            }
        }

        private void ProdutoDigitado(object param)
        {
            Digit = param as string;   
        }

        public string Digit { set { clearProduto(value); } }

        private void clearProduto(string value)
        {
            foreach(char c in value)
            {
                if ("0123456789".Contains(c))
                {

                    if (digiteProduto.Length < 13)
                    {
                        DigiteProduto += c;
                        StatusProduto();
                    }
                }
            }
        }

        private void StatusProduto()
        {
            int count = digiteProduto.Length;
            ButtonGerarEnabled = count == 8 || count == 13;
        }

        public void OnToggleButton()
        {
            Shot();
        }

        private void CheckView()
        {
            if (Data.Data.IsProduto)
            {
                ProdutoEnabled();
            }
            else
            {
                EncomendaEnabled();
            }
        }

        public bool ButtonGerarEnabled
        {
            get { return buttonGerar; }
            private set
            {
                if(buttonGerar != value)
                {
                    buttonGerar = value;
                    RaisePropertyChanged();
                }
            }
        }

        private async void OnInfo(string resumido)
        {
            try
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Title = GetInfo(0);
                dialog.Content = loader.GetString($"EncInfo{resumido}{GetInfo(1)}");
                dialog.Foreground = ((Brush)App.Current.Resources["Texto"]);
                dialog.Background = ((Brush)App.Current.Resources["DialogoBackground"]);
                dialog.CloseButtonStyle = ((Style)App.Current.Resources["ButtonStyleContent"]);
                dialog.CloseButtonText = loader.GetString("ButtonClose");
                await dialog.ShowAsync();

            }
            catch { }
        }

        private string GetInfo(int value)
        {
            string txt = "";
            switch (GetIndex)
            {
                case 0: txt = "SW123456789BR"; break;
                case 1: txt = "LP00060609526813"; break;
                case 2: txt = "YT1701990005858243"; break;
                case 3: txt = "YD197747595YW"; break;
                case 4: txt = value == 1 ? "SYBAA11849265SYBAB21777027" : "SYBAA11849265 (PosMalaysia)"; break;
                case 5: txt = value == 1 ? "SYBAA11849265SYBAB21777027" : "SYBAB11849265 (PosMalaysia)"; break;
                case 6: txt = "NL14869665773829"; break;
                case 7: txt = "SY10994025345"; break;
            }
            return txt;
        }

        public ICommand ProcessInfoCommand
        {
            get
            {
                if(processInfoCommand == null)
                {
                    processInfoCommand = new RelayCommand(p => OnInfo(""));
                }
                return processInfoCommand;
            }
        }

        public ICommand ProcessCopyCommand
        {
            get
            {
                if(processCopyCommand == null)
                {
                    processCopyCommand = new RelayCommand(p => OnCopy());
                }
                return processCopyCommand;
            }
        }

        public ICommand ProcessDialPadDelete
        {
            get
            {
                if(processDialDelete == null)
                {
                    processDialDelete = new RelayCommand(p => this.DeleteInDial());
                }
                return processDialDelete;
            }
        }

        public void DeleteInDial()
        {
            switch (GetIndex)
            {
                case 0:
                    {
                        if (dialerEncomendaCorreios.Length > 0)
                        {
                            DialerEncomendaCorreios = DialerEncomendaCorreios.Remove(dialerEncomendaCorreios.Length - 1);
                            StatusViewDigit();
                        }
                    }
                    break;
                case 1:
                    {
                        if (dialerEncomendaLB.Length > 0)
                        {
                            DialerEncomendaLB = DialerEncomendaLB.Remove(dialerEncomendaLB.Length - 1);
                            StatusViewDigit();
                        }
                    }
                    break;
                case 2:
                    {
                        if (dialerEncomendaYT.Length > 0)
                        {
                            DialerEncomendaYT = DialerEncomendaYT.Remove(dialerEncomendaYT.Length - 1);
                            StatusViewDigit();
                        }
                    }
                    break;
                case 3:
                    {
                        if (dialerEncomendaYDYW.Length > 0)
                        {
                            DialerEncomendaYDYW = DialerEncomendaYDYW.Remove(dialerEncomendaYDYW.Length - 1);
                            StatusViewDigit();
                        }
                    }
                    break;
                case 4:
                    {
                        if (dialerEncomendaSYBAA.Length > 0)
                        {
                            DialerEncomendaSYBAA = DialerEncomendaSYBAA.Remove(dialerEncomendaSYBAA.Length - 1);
                            StatusViewDigit();
                        }
                    }
                    break;
                case 5:
                    {
                        if (dialerEncomendaSYBAB.Length > 0)
                        {
                            DialerEncomendaSYBAB = DialerEncomendaSYBAB.Remove(dialerEncomendaSYBAB.Length - 1);
                            StatusViewDigit();
                        }
                    }
                    break;
                case 6:
                    {
                        if (dialerEncomendaNL.Length > 0)
                        {
                            DialerEncomendaNL = DialerEncomendaNL.Remove(dialerEncomendaNL.Length - 1);
                            StatusViewDigit();
                        }
                    }
                    break;
                case 7:
                    {
                        if (dialerEncomendaSY.Length > 0)
                        {
                            DialerEncomendaSY = DialerEncomendaSY.Remove(dialerEncomendaSY.Length - 1);
                            StatusViewDigit();
                        }
                    }
                    break;
            }
        }


        public void DeleteAllInDial()
        {
            switch (GetIndex)
            {
                case 0:
                    {
                        if (dialerEncomendaCorreios.Length > 0)
                        {
                            DialerEncomendaCorreios = "";
                            StatusViewDigit();
                        }
                    }
                    break;
                case 1:
                    {
                        if (dialerEncomendaLB.Length > 0)
                        {
                            DialerEncomendaLB = "";
                            StatusViewDigit();
                        }
                    }
                    break;
                case 2:
                    {
                        if (dialerEncomendaYT.Length > 0)
                        {
                            DialerEncomendaYT = "";
                            StatusViewDigit();
                        }
                    }
                    break;
                case 3:
                    {
                        if (dialerEncomendaYDYW.Length > 0)
                        {
                            DialerEncomendaYDYW = "";
                            StatusViewDigit();
                        }
                    }
                    break;
                case 4:
                    {
                        if (dialerEncomendaSYBAA.Length > 0)
                        {
                            DialerEncomendaSYBAA = "";
                            StatusViewDigit();
                        }
                    }
                    break;
                case 5:
                    {
                        if (dialerEncomendaSYBAB.Length > 0)
                        {
                            DialerEncomendaSYBAB = "";
                            StatusViewDigit();
                        }
                    }
                    break;
                case 6:
                    {
                        if (dialerEncomendaNL.Length > 0)
                        {
                            DialerEncomendaNL = "";
                            StatusViewDigit();
                        }
                    }
                    break;
                case 7:
                    {
                        if (dialerEncomendaSY.Length > 0)
                        {
                            DialerEncomendaSY = "";
                            StatusViewDigit();
                        }
                    }
                    break;
            }
        }


        public ICommand ProcessDialPadNumber
        {
            get
            {
                if (processDialPadNumberCommand == null)
                {
                    processDialPadNumberCommand = new RelayCommand(this.ShotNumberInDialerNumber);
                }
                return processDialPadNumberCommand;
            }
        }
        public ICommand ProcessDialPadLetras
        {
            get
            {
                if (processDialPadLetrasCommand == null)
                {
                    processDialPadLetrasCommand = new RelayCommand(this.ShotNumberInDialerLetras);
                }
                return processDialPadLetrasCommand;
            }
        }



        private void ShotNumberInDialerNumber(object param)
        {
            DigitNumber = param as string;
        }
        private void ShotNumberInDialerLetras(object param)
        {
            DigitLetra = param as string;
        }

        internal void SetCompomentes(Frame frame)
        {
            this.frame = frame;
             Shot();
        }
         

        private void ShotDialerNumber()
        {
            DialerVisibility = "Collapsed";
            DialerNumberVisibility = "Visible";
            CopyFunctionVisible = "Collapsed";
        }

        private void ShotDialer()
        {
            DialerNumberVisibility = "Collapsed";
            DialerVisibility = "Visible";
            CopyFunctionVisible = "Visible";
        }

        public string DialerNumberVisibility
        {
            get { return dialerNumberVisibility; }
            private set
            {
                if (dialerNumberVisibility != value)
                {
                    dialerNumberVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerVisibility
        {
            get { return dialerVisibility; }
            private set
            {
                if (dialerVisibility != value)
                {
                    dialerVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }
         

        internal void Shot()
        {
            if (Data.Data.IsProduto)
            {
                StatusProduto();
            }
            else
            {
                switch (GetIndex)
                {
                    case 0:
                        CorreiosVisibility();
                        break;
                    case 1:
                        LBVisibility();
                        break;
                    case 2:
                        YTVisibility();
                        break;
                    case 3:
                        YDYWVisibility();
                        break;
                    case 4:
                        SYBAAVisibility();
                        break;
                    case 5:
                        SYBABVisibility();
                        break;
                    case 6:
                        NLVisibility();
                        break;
                    case 7:
                        SYVisibility();
                        break;
                }
                if (GetIndex == 0)
                {
                    ShotDialer();
                }
                else ShotDialerNumber();
                StatusViewDigit();
            }

            CheckView();
        }

        public void OnCopy()
        {
            string txtCopy = "";

            switch (GetIndex)
            {
                case 0:
                    {
                        if (dialerEncomendaCorreios.Length > 0)
                        {
                            txtCopy = dialerEncomendaCorreios;
                        }
                    }
                    break;
                case 1:
                    {
                        if (dialerEncomendaLB.Length > 0)
                        {
                            txtCopy = $"LP{dialerEncomendaLB}";
                        }
                    }
                    break;
                case 2:
                    {
                        if (dialerEncomendaYT.Length > 0)
                        {
                            txtCopy = $"YT{dialerEncomendaYT}";
                        }
                    }
                    break;
                case 3:
                    {
                        if (dialerEncomendaYDYW.Length > 0)
                        {
                            string final = dialerEncomendaYDYW.Length == 9 ? "YW" : "";
                            txtCopy = $"YD{dialerEncomendaYDYW}{final}";
                        }
                    }
                    break;
                case 4:
                    {
                        if (dialerEncomendaSYBAA.Length > 0)
                        {
                            txtCopy = $"SYBAA{dialerEncomendaSYBAA}";
                        }
                    }
                    break;
                case 5:
                    {
                        if (dialerEncomendaSYBAB.Length > 0)
                        {
                            txtCopy = $"SYBAB{dialerEncomendaSYBAB}";
                        }
                    }
                    break;
                case 6:
                    {
                        if (dialerEncomendaNL.Length > 0)
                        {
                            txtCopy = $"NL{dialerEncomendaNL}";
                        }
                    }
                    break;
                case 7:
                    {
                        if (dialerEncomendaSY.Length > 0)
                        {
                            txtCopy = $"SY{dialerEncomendaSY}";
                        }
                    }
                    break;
            }

            if (!txtCopy.Equals(""))
            {
                var dataPackage = new DataPackage();
                dataPackage.SetText(txtCopy);
                Clipboard.SetContent(dataPackage);
                Paginas.Root.RootApp.Instance.GetToast(loader.GetString("CopyText"), Tools.ModoColor.Succes);
            }
            else
            {
                Paginas.Root.RootApp.Instance.GetToast(loader.GetString("CopyClipboardError"), Tools.ModoColor.Error);
            }
        }

        private string ConvertForNumber(string text)
        {
            string x = "";
            foreach(char c in text)
            {
                if ("0123456789".Contains(c))
                {
                    x += c;
                }
            }
            return x;
        }

        private string Trim(string v)
        {
            return v.Trim();
        }

        public int GetIndex
        {
            get { return Data.Data.SttEncTypes; }
            private set
            {
                if(Data.Data.SttEncTypes != value)
                {
                    Data.Data.SttEncTypes = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DigitNumber { set { clearInputNumber(value); } }
        public string DigitLetra { set { clearInputLetra(value); } }

        private void clearInputLetra(string value)
        {
            try
            {
                foreach (char c in value)
                {
                    if ("QWERTYUIOPASDFGHJKLZXCVBNM".Contains(c))
                    {
                        switch (GetIndex)
                        {
                            case 0:
                                if(dialerEncomendaCorreios.Length < 13)
                                {
                                    DialerEncomendaCorreios += c;
                                    StatusViewDigit();
                                }
                                break;  
                        }
                       
                    }
                }
            }
            catch { }
        }

        private void clearInputNumber(string value)
        {
            try
            {
                foreach (char c in value)
                {
                    if ("1234567890".Contains(c))
                    { 
                        switch (GetIndex)
                        {
                            case 0:
                                if (dialerEncomendaCorreios.Length < 13)
                                {
                                    DialerEncomendaCorreios += c;
                                    StatusViewDigit();
                                }
                                break;
                            case 1:
                                if (dialerEncomendaLB.Length < 14)
                                {
                                    DialerEncomendaLB += c;
                                    StatusViewDigit();
                                }
                                break;
                            case 2:
                                if (dialerEncomendaYT.Length < 16)
                                {
                                    DialerEncomendaYT += c;
                                    StatusViewDigit();
                                }
                                break;
                            case 3:
                                if (dialerEncomendaYDYW.Length < 9)
                                {
                                    DialerEncomendaYDYW += c;
                                    StatusViewDigit();
                                }
                                break;
                            case 4:
                                if (dialerEncomendaSYBAA.Length < 8)
                                {
                                    DialerEncomendaSYBAA += c;
                                    StatusViewDigit();
                                }
                                break;
                            case 5:
                                if (dialerEncomendaSYBAB.Length < 8)
                                {
                                    DialerEncomendaSYBAB += c;
                                    StatusViewDigit();
                                }
                                break;
                            case 6:
                                if (dialerEncomendaNL.Length < 14)
                                {
                                    DialerEncomendaNL += c;
                                    StatusViewDigit();
                                }
                                break;
                            case 7:
                                if (dialerEncomendaSY.Length < 11)
                                {
                                    DialerEncomendaSY += c;
                                    StatusViewDigit();
                                }
                                break;
                        }

                    }
                }
            }
            catch { }
        }

        private void StatusViewDigit()
        {
            switch (GetIndex)
            {
                case 0:
                    {

                        string digit = dialerEncomendaCorreios;
                        int count = dialerEncomendaCorreios.Length;

                        if (count >= 11)
                        {
                            ShotDialer();
                        }
                        else if(count <= 1)
                        {
                            ShotDialer(); 
                        }
                        else if(count <= 10)
                        {
                            ShotDialerNumber();
                        }
                        ButtonGerarEnabled = count == 13;

                    } break;
                case 1:
                    {
                        int count = dialerEncomendaLB.Length;
                        ButtonGerarEnabled = count == 14;
                    }
                    break;
                case 2:
                    {
                        int count = dialerEncomendaYT.Length;
                        ButtonGerarEnabled = count == 16;
                    }
                    break;
                case 3:
                    {
                        int count = dialerEncomendaYDYW.Length;
                        ButtonGerarEnabled = count == 9;
                    }
                    break;
                case 4:
                    {
                        int count = dialerEncomendaSYBAA.Length;
                        ButtonGerarEnabled = count == 8;
                    }
                    break;
                case 5:
                    {
                        int count = dialerEncomendaSYBAB.Length;
                        ButtonGerarEnabled = count == 8;
                    }
                    break;
                case 6:
                    {
                        int count = dialerEncomendaNL.Length;
                        ButtonGerarEnabled = count == 14;
                    }
                    break;
                case 7:
                    {
                        int count = dialerEncomendaSY.Length;
                        ButtonGerarEnabled = count == 11;
                    }
                    break;
            }
        }

        public string DialerEncomendaSY
        {
            get { return dialerEncomendaSY; }
            private set
            {
                if (dialerEncomendaSY != value)
                {
                    dialerEncomendaSY = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerEncomendaNL
        {
            get { return dialerEncomendaNL; }
            private set
            {
                if (dialerEncomendaNL != value)
                {
                    dialerEncomendaNL = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerEncomendaSYBAA
        {
            get { return dialerEncomendaSYBAA; }
            private set
            {
                if (dialerEncomendaSYBAA != value)
                {
                    dialerEncomendaSYBAA = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerEncomendaSYBAB
        {
            get { return dialerEncomendaSYBAB; }
            private set
            {
                if (dialerEncomendaSYBAB != value)
                {
                    dialerEncomendaSYBAB = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerEncomendaYDYW
        {
            get { return dialerEncomendaYDYW; }
            private set
            {
                if (dialerEncomendaYDYW != value)
                {
                    dialerEncomendaYDYW = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerEncomendaYT
        {
            get { return dialerEncomendaYT; }
            private set
            {
                if (dialerEncomendaYT != value)
                {
                    dialerEncomendaYT = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerEncomendaLB
        {
            get { return dialerEncomendaLB; }
            private set
            {
                if (dialerEncomendaLB != value)
                {
                    dialerEncomendaLB = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerEncomendaCorreios
        {
            get { return dialerEncomendaCorreios; }
            private set
            {
                if (dialerEncomendaCorreios != value)
                {
                    dialerEncomendaCorreios = value;
                    RaisePropertyChanged();
                }
            }
        }

        #region Visibility

   
        public string DialerEncomendaSYVisibility
        {
            get { return dialerEncomendaSYVisibility; }
            private set
            {
                if (dialerEncomendaSYVisibility != value)
                {
                    dialerEncomendaSYVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerEncomendaNLVisibility
        {
            get { return dialerEncomendaNLVisibility; }
            private set
            {
                if (dialerEncomendaNLVisibility != value)
                {
                    dialerEncomendaNLVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerEncomendaSYBAAVisibility
        {
            get { return dialerEncomendaSYBAAVisibility; }
            private set
            {
                if (dialerEncomendaSYBAAVisibility != value)
                {
                    dialerEncomendaSYBAAVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string DialerEncomendaSYBABVisibility
        {
            get { return dialerEncomendaSYBABVisibility; }
            private set
            {
                if (dialerEncomendaSYBABVisibility != value)
                {
                    dialerEncomendaSYBABVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerEncomendaYDYWVisibility
        {
            get { return dialerEncomendaYDYWVisibility; }
            private set
            {
                if (dialerEncomendaYDYWVisibility != value)
                {
                    dialerEncomendaYDYWVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerEncomendaYTVisibility
        {
            get { return dialerEncomendaYTVisibility; }
            private set
            {
                if (dialerEncomendaYTVisibility != value)
                {
                    dialerEncomendaYTVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerEncomendaLBVisibility
        {
            get { return dialerEncomendaLBVisibility; }
            private set
            {
                if (dialerEncomendaLBVisibility != value)
                {
                    dialerEncomendaLBVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string DialerEncomendaCorreiosVisibility
        {
            get { return dialerEncomendaCorreiosVisibility; }
            private set
            {
                if (dialerEncomendaCorreiosVisibility != value)
                {
                    dialerEncomendaCorreiosVisibility = value;
                    RaisePropertyChanged();
                }
            }
        }



        private void SYVisibility()
        {
            DialerEncomendaSYVisibility = "Visible";
            DialerEncomendaNLVisibility = "Collapsed";
            DialerEncomendaSYBAAVisibility = "Collapsed";
            DialerEncomendaSYBABVisibility = "Collapsed";
            DialerEncomendaYDYWVisibility = "Collapsed";
            DialerEncomendaYTVisibility = "Collapsed";
            DialerEncomendaLBVisibility = "Collapsed";
            DialerEncomendaCorreiosVisibility = "Collapsed";
        }

        private void NLVisibility()
        {
            DialerEncomendaSYVisibility = "Collapsed";
            DialerEncomendaNLVisibility = "Visible";
            DialerEncomendaSYBAAVisibility = "Collapsed";
            DialerEncomendaSYBABVisibility = "Collapsed";
            DialerEncomendaYDYWVisibility = "Collapsed";
            DialerEncomendaYTVisibility = "Collapsed";
            DialerEncomendaLBVisibility = "Collapsed";
            DialerEncomendaCorreiosVisibility = "Collapsed";
        }

        private void SYBAAVisibility()
        {
            DialerEncomendaSYVisibility = "Collapsed";
            DialerEncomendaNLVisibility = "Collapsed";
            DialerEncomendaSYBAAVisibility = "Visible";
            DialerEncomendaSYBABVisibility = "Collapsed";
            DialerEncomendaYDYWVisibility = "Collapsed";
            DialerEncomendaYTVisibility = "Collapsed";
            DialerEncomendaLBVisibility = "Collapsed";
            DialerEncomendaCorreiosVisibility = "Collapsed";
        }

        private void SYBABVisibility()
        {
            DialerEncomendaSYVisibility = "Collapsed";
            DialerEncomendaNLVisibility = "Collapsed";
            DialerEncomendaSYBAAVisibility = "Collapsed";
            DialerEncomendaSYBABVisibility = "Visible";
            DialerEncomendaYDYWVisibility = "Collapsed";
            DialerEncomendaYTVisibility = "Collapsed";
            DialerEncomendaLBVisibility = "Collapsed";
            DialerEncomendaCorreiosVisibility = "Collapsed";
        }


        private void YDYWVisibility()
        {
            DialerEncomendaSYVisibility = "Collapsed";
            DialerEncomendaNLVisibility = "Collapsed";
            DialerEncomendaSYBAAVisibility = "Collapsed";
            DialerEncomendaSYBABVisibility = "Collapsed";
            DialerEncomendaYDYWVisibility = "Visible";
            DialerEncomendaYTVisibility = "Collapsed";
            DialerEncomendaLBVisibility = "Collapsed";
            DialerEncomendaCorreiosVisibility = "Collapsed";
        }


        private void YTVisibility()
        {
            DialerEncomendaSYVisibility = "Collapsed";
            DialerEncomendaNLVisibility = "Collapsed";
            DialerEncomendaSYBAAVisibility = "Collapsed";
            DialerEncomendaSYBABVisibility = "Collapsed";
            DialerEncomendaYDYWVisibility = "Collapsed";
            DialerEncomendaYTVisibility = "Visible";
            DialerEncomendaLBVisibility = "Collapsed";
            DialerEncomendaCorreiosVisibility = "Collapsed";
        }

        private void LBVisibility()
        {
            DialerEncomendaSYVisibility = "Collapsed";
            DialerEncomendaNLVisibility = "Collapsed";
            DialerEncomendaSYBAAVisibility = "Collapsed";
            DialerEncomendaSYBABVisibility = "Collapsed";
            DialerEncomendaYDYWVisibility = "Collapsed";
            DialerEncomendaYTVisibility = "Collapsed";
            DialerEncomendaLBVisibility = "Visible";
            DialerEncomendaCorreiosVisibility = "Collapsed";
        }

        private void CorreiosVisibility()
        {
            DialerEncomendaSYVisibility = "Collapsed";
            DialerEncomendaNLVisibility = "Collapsed";
            DialerEncomendaSYBAAVisibility = "Collapsed";
            DialerEncomendaSYBABVisibility = "Collapsed";
            DialerEncomendaYDYWVisibility = "Collapsed";
            DialerEncomendaYTVisibility = "Collapsed";
            DialerEncomendaLBVisibility = "Collapsed";
            DialerEncomendaCorreiosVisibility = "Visible";
        }



        #endregion
    }
}