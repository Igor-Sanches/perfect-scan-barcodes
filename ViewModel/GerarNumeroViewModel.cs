using Perfect_Scan.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Microsoft.Toolkit.Extensions;
using Windows.UI.Xaml.Controls;

namespace Perfect_Scan.ViewModel
{
    public class GerarNumeroViewModel : ViewModelBase
    {
        private DialerPhoneNumber dialerPhoneNumber;
        private RelayCommand dialPadCommand;
        private RelayCommand dialPadHoldingCommand; 
        private ResourceLoader loader = new ResourceLoader();
        private Regex singaLetra = new Regex("[A-Z]+"); 
        private Regex regex = new Regex(@"[^\d]");
        private RelayCommand dialDialerNumberHeapCommand;
        private Frame frame;


        /// <summary>
        /// A view model class for the dialer panel/module.
        /// </summary>
        public GerarNumeroViewModel()
        {
            dialerPhoneNumber = new DialerPhoneNumber();
        }

        /// <summary>
        /// Relay command action to append the primary newly dialed digit to the string 
        /// of number to dial
        /// </summary>
        /// <param name="CommandParam">Primary newly dialed digit string</param>
        private void DialPadInvoked(object CommandParam)
        {
            dialerPhoneNumber.Digit = CommandParam as string;
        }

        public void Loader(Frame frame)
        {
            this.frame = frame;
        }

        internal void Shot(string tel)
        {
            dialerPhoneNumber.Digit = tel;
        }

        /// <summary>
        /// Relay command action to append the secdondary newly dialed digit to the string 
        /// of number to dial.
        /// For "1" this just dials voicemail. 
        /// </summary>
        /// <param name="CommandParam">Secdondary newly dialed digit string</param>
        private void DialPadHoldingInvoked(object CommandParam)
        {
            string OnHoldingDigit = CommandParam as string;

            if ((OnHoldingDigit == "1") && (dialerPhoneNumber.NumberToDial.Length == 1)  )
            {
                dialerPhoneNumber.ClearDialerNumberHeap(); 
            }
            else
            {
                dialerPhoneNumber.ReplaceOnHoldingDigit(OnHoldingDigit);
            }
        }

        private string ClearNumero(string input)
        {
            string n = "";

            foreach(char c in input)
            {
                if ("1234567890".Contains(c))
                {
                    n += c;
                }
            }

            return n;
        }

        /// <summary>
        /// Relay command action to place a call to the numbers dialed.
        /// </summary>
        private void OnGerar()
        {
            if (dialerPhoneNumber.NumberToDial.Length > 0)
            {
                string phoneNumber = this.dialerPhoneNumber.NumberToDial;
                string cha = "TEL:";
                string codigo = "";
                bool pass = true;

                if (!phoneNumber.Equals(""))
                {
                    string numero = ClearNumero(phoneNumber); 
                    codigo = cha + numero; 
                     
                }
                if (pass)
                {
                    Gerador.With(codigo).GerarCodigo(Data.Data.Codigo, frame);
                }
            }
        }

        /// <summary>
        /// Relay command to execute the dial invoked action from the dialer view.
        /// </summary>
        public ICommand ProcessDialPad
        {
            get
            {
                if (dialPadCommand == null)
                {
                    dialPadCommand = new RelayCommand(
                        this.DialPadInvoked);
                }
                return dialPadCommand;
            }
        }

        public ICommand ProgressPasteTexto
        {
            get
            {
                return dialerPhoneNumber.BackPasteCommand;
            }
        }
         
        /// <summary>
        /// Relay command to execute the dial pad holding action from the dialer view.
        /// </summary>
        public ICommand ProcessDialPadHolding
        {
            get
            {
                if (dialPadHoldingCommand == null)
                {
                    dialPadHoldingCommand = new RelayCommand(
                        this.DialPadHoldingInvoked);
                }
                return dialPadHoldingCommand;
            }
        }

        /// <summary>
        /// Relay command to execute the DialDialerNumberHeap action from the dialer view.
        /// </summary>
        public ICommand GerarCommand
        {
            get
            {
                if (dialDialerNumberHeapCommand == null)
                {
                    dialDialerNumberHeapCommand = new RelayCommand(p =>
                        this.OnGerar());
                }
                return dialDialerNumberHeapCommand;
            }
        }

        /// <summary>
        /// Relay command to execute the backspace action from the dialer view.
        /// </summary>
        public ICommand ProcessBackspace
        {
            get
            {
                return dialerPhoneNumber.BackSpaceCommand;
            }
        }

        public ICommand ProgressContactPicker
        {
            get
            {
                return dialerPhoneNumber.BackPickerCommand;
            }
        }

        /// <summary>
        /// Relay command to execute the backspace holding (clear dialer number heap) action from the dialer view.
        /// </summary>
        public ICommand ProcessBackSpaceHolding
        {
            get
            {
                return dialerPhoneNumber.BackSpaceHoldingCommand;
            }
        }

        #region Getters
        /// <summary>
        /// Get the phone number to dial object.
        /// </summary>
        public DialerPhoneNumber DialerPhoneNumber
        {
            get
            {
                return dialerPhoneNumber;
            }
        }
         
        #endregion
    }
}
