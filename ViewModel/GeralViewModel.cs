using Perfect_Scan.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Perfect_Scan.ViewModel
{
    public class GeralViewModel : ViewModelBase
    { 
        private RelayCommand btnGerar, negritoCommand, italicoCommand, alignCommand, showFonts, clearCommand;
        private string fontfamilyVisible = "Visible", texto, negrito = "", italico = "", align_ = "";
        private MenuFlyout fontFamily, fontSize, btnFont;
        private Frame frame;
        private CheckBox CheckBoxTextItalic;
        private Manager.PersisText GeralPlanilhaEditor;
        private bool buttonEnabled = false;

        public ICommand GerarCommand
        {
            get
            {
                if(btnGerar == null)
                {
                    btnGerar = new RelayCommand(p => this.OnGerar());
                }
                return btnGerar;
            } 
        }

        private void OnGerar()
        { 
            Gerador.With(texto).GerarCodigo(Data.Data.Codigo, frame);
        }

        public string Texto
        {
            get { return texto; }
            private set
            {
                if(texto != value)
                {
                    texto = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ButtonEnabled
        {
            get { return buttonEnabled; }
            private set
            {
                if(buttonEnabled != value)
                {
                    buttonEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void ButtonsEnable()
        {
            ButtonEnabled = !GeralPlanilhaEditor.Text.Equals("");
        }
      
        public string FontFamilyVisible
        {
            get { return fontfamilyVisible; }
            private set
            {
                if (fontfamilyVisible != value)
                {
                    fontfamilyVisible = value;
                    RaisePropertyChanged();
                }
            }
        }

        public void LoadFonts(string protocol, MenuFlyout fontFamily, MenuFlyout fontSize, MenuFlyout btnFont, CheckBox CheckBoxTextItalic, Manager.PersisText GeralPlanilhaEditor, Frame frame)
        {
            this.frame = frame;
            this.GeralPlanilhaEditor = GeralPlanilhaEditor;
            this.CheckBoxTextItalic = CheckBoxTextItalic;
            this.fontFamily = fontFamily;
            this.fontSize = fontSize;
            this.btnFont = btnFont;
            GeralPlanilhaEditor.TextChanged += (sender, args) =>
            {
                texto = GeralPlanilhaEditor.Text;
                ButtonsEnable();
            };
            ButtonsEnable();
            try
            {
                // adicionar fontes
                var fonts = Microsoft.Graphics.Canvas.Text.CanvasTextFormat.GetSystemFontFamilies();
                foreach (string f in fonts)
                {
                    MenuFlyoutItem itemF = new MenuFlyoutItem
                    {
                        Text = f,
                        Foreground = ((Brush)App.Current.Resources["Texto"]),
                        FontFamily = new FontFamily(f)
                    };
                    itemF.Click += (fx, f2) =>
                    {
                        string fontF = ((MenuFlyoutItem)fx).Text;
                        FonteFamilia = fontF;
                    };

                    fontFamily.Items.Add(itemF);
                }
                FontFamilyVisible = "Visible";
            }
            catch { FontFamilyVisible = "Collapsed"; }


            // adicionar tamanhos
            int[] sizes = { 12, 15, 18, 20, 22, 24, 28, 32, 36, 40, 48, 56, 64 };

            foreach (int s in sizes)
            {
                MenuFlyoutItem itemS = new MenuFlyoutItem
                {
                    Text = s.ToString(),
                    Foreground = ((Brush)App.Current.Resources["Texto"]),
                    FontSize = s
                };
                itemS.Click += (fx, f2) =>
                {
                    string fontS = ((MenuFlyoutItem)fx).Text;
                    TamanhoDaFonte = fontS;
                };

                fontSize.Items.Add(itemS);
            }
            if(protocol != "")
            {
                GeralPlanilhaEditor.Text = protocol;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if(clearCommand == null)
                {
                    clearCommand = new RelayCommand(p => ClearEditor());
                }
                return clearCommand;
            }
        }

        private void ClearEditor()
        {
            GeralPlanilhaEditor.Text = ""; 
        }

        public ICommand AlignCommand
        {
            get
            {
                if(alignCommand == null)
                {
                    alignCommand = new RelayCommand(
                        this.AlignControlCommand);
                }
                return alignCommand;
            }
        }

        private void AlignControlCommand(object param)
        {
            string align = param as string;
            Align = align;
        }
        private void ShowFontsControlCommand(object param)
        {
            string fontType = param as string;
            this.btnFont.Hide();

            string tag = fontType;

            if (tag.Equals("Family"))
            {
                this.fontFamily.ShowAt(this.CheckBoxTextItalic);
            }
            else
            {
                this.fontSize.ShowAt(this.CheckBoxTextItalic);
            }
        }

        public ICommand ShowFontsCommand
        {
            get
            {
                if(showFonts == null)
                {
                    showFonts = new RelayCommand(this.ShowFontsControlCommand);

                }
                return showFonts;
            }
    }

        public ICommand NegritoCommand
        {
            get
            {
                if (negritoCommand == null)
                {
                    negritoCommand = new RelayCommand(p => this.OnNegrito());
                }
                return negritoCommand;
            }

        }
        public ICommand ItalicoCommand
        {
            get
            {
                if (italicoCommand == null)
                {
                    italicoCommand = new RelayCommand(p => this.OnItalico());
                }
                return italicoCommand;
            }

        }

        private void OnItalico()
        {
            bool value = !ItalicoEnabled;
            ItalicoEnabled = value;
            Italico = ItalicoEnabled ? "Italic" : "Normal";
        }

        private void OnNegrito()
        {
            bool value = !NegritoEnabled;
            NegritoEnabled = value;
            Negrito = NegritoEnabled ? "Bold" : "Normal";
        }

        public string Align
        {
            get
            {
                return Data.Data.SttTextAlignment;
            }
            private set
            {
                if(Data.Data.SttTextAlignment != value)
                {
                    Data.Data.SttTextAlignment = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool AlignLeft
        {
            get
            {
                return Data.Data.IsAlignLeft;
            }
        }
        public bool AlignCenter
        {
            get
            {
                return Data.Data.IsAlignCenter;
            }
        }
        public bool AlignRight
        {
            get
            {
                return Data.Data.IsAlignRight;
            }
        }

        public string FonteFamilia
        {
            get { return Data.Data.SttFontFamily; }
            private set
            {
                if(Data.Data.SttFontFamily != value)
                {
                    Data.Data.SttFontFamily = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string TamanhoDaFonte
        {
            get
            {
                return Data.Data.SttFontSize.ToString();
            }
            private set
            {
                if(Data.Data.SttFontSize.ToString() != value)
                {
                    Data.Data.SttFontSize = System.Convert.ToInt32(value);
                    RaisePropertyChanged();
                }
            }
        }

        public string Italico
        {
            get { return ItalicoEnabled ? "Italic" : "Normal"; }
            private set
            {
                if (italico != value)
                {
                    italico = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Negrito
        {
            get
            {
                return NegritoEnabled ? "Bold" : "Normal";
            }
            private set
            {
                if (negrito != value)
                {
                    negrito = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ItalicoEnabled
        {
            get { return Data.Data.SttItalicIsEnabled; }
            private set
            {
                if (Data.Data.SttItalicIsEnabled != value)
                {
                    Data.Data.SttItalicIsEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }


        public bool NegritoEnabled
        {
            get { return Data.Data.SttBoldIsEnabled; }
            private set
            {
                if (Data.Data.SttBoldIsEnabled != value)
                {
                    Data.Data.SttBoldIsEnabled = value;
                    RaisePropertyChanged();
                }
            }

        }
         
    }
}
