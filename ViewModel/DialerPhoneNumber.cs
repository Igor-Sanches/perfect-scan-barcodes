using Perfect_Scan.Manager;
using System;
using System.Windows.Input;
using Windows.ApplicationModel.Contacts;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Globalization.PhoneNumberFormatting;
using Microsoft.Toolkit.Extensions;  

namespace Perfect_Scan.ViewModel
{
    public class DialerPhoneNumber : ViewModelBase
    {
        private string numberToDial = "";
        private bool dialPadEnabled = false;
        private ResourceLoader loader = new ResourceLoader();
        private PhoneNumberFormatter phone = new PhoneNumberFormatter();
        private RelayCommand backSpaceCommand;
        private RelayCommand backSpaceHoldingCommand;
        private RelayCommand backPickerCommand;
        private RelayCommand backPasteCommand;

        /// <summary>
        /// Takes an input, ensures its a dialable chanracter.
        /// Then appends this input to the number field
        /// </summary>
        private void cleanInput(string content)
        {
            foreach (char c in content)
            {
                if (",;+#*0123456789".Contains(c.ToString()))
                {
                    this.NumberToDial += c;
                    EvalDialerState();
                }
            }
        }

        /// <summary>
        /// Evaluates the dialpad buttons with secondary characters.
        /// These characters are replaced with the primary on press and hold
        /// </summary>
        public void ReplaceOnHoldingDigit(string newDigit)
        {
            if (numberToDial.Length > 0)
            {
                if ((newDigit == "+") && (numberToDial.Length > 1))
                {
                    return;
                }
                this.NumberToDial = this.NumberToDial.Remove(numberToDial.Length - 1);
                this.NumberToDial += newDigit;
            }
        }

        internal void SetDialNumberKey(string v)
        {
            NumberToDial += v;
            EvalDialerState();
        }

        internal void BackPasteInDial()
        {
            PasteText();
        }

        /// <summary>
        /// Removes the last character in the number field.
        /// </summary>
        private void BackSpaceInvoked()
        {
            if (numberToDial.Length > 0)
            {
                this.NumberToDial = this.NumberToDial.Remove(numberToDial.Length - 1);
                EvalDialerState();
            }
        }

        /// <summary>
        /// Clears the entire number field.
        /// </summary>
        public void ClearDialerNumberHeap()
        {
            if (numberToDial.Length > 0)
            {
                this.NumberToDial = "";
                EvalDialerState();
            }
        }

        /// <summary>
        /// Evaluate the number field.
        /// if its empty, it returns false, which disables the call and backspace button in the dialer
        /// if its not empty, it returns true, which enables the call and backspace button in the dialer
        /// </summary>
        private void EvalDialerState()
        {
            if (numberToDial.Length > 3 && numberToDial.Length <= 16)
            {
                DialPadEnabled = true;
            }
            else
            {
                DialPadEnabled = false;
            }
        }

        internal void SetBackSpaceCommand()
        {
            BackSpaceInvoked();
        }

        internal void SetBackSpaceCommandDialClear()
        {
            ClearDialerNumberHeap();
        }
         

        private async void PasteText()
        {
            DataPackageView view = Clipboard.GetContent();
            if (view.Contains(StandardDataFormats.Text))
            {
                string text = await view.GetTextAsync();
                cleanInput(text);
                if (NumberToDial.Equals(""))
                {
                    Paginas.Root.RootApp.Instance.GetToast(loader.GetString("ClipboardError"), Tools.ModoColor.Error);
                }
            }
        }

       public ICommand BackPasteCommand
        {
            get
            {
                if (backPasteCommand == null)
                {
                    backPasteCommand = new RelayCommand(p => this.PasteText());
                }

                return backPasteCommand;
            }
        }

        public ICommand BackPickerCommand
        {
            get
            {
                if(backPickerCommand == null)
                {
                    backPickerCommand = new RelayCommand(p => this.ContactInvoke());
                }
                return backPickerCommand;
            }
        }

        private async void ContactInvoke()
        {
            try
            {
                var picker = new ContactPicker();
                picker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
                var contato = await picker.PickContactAsync();
                if (contato != null)
                {
                    foreach (var phone in contato.Phones)
                    {
                        NumberToDial = phone.Number;
                        EvalDialerState();
                    }
                }
            }
            catch (Exception x) { Paginas.Root.RootApp.Instance.GetToast(x.Message); }
        } 

        /// <summary>
        /// Relay command to execute the backspace action from the dialer view.
        /// </summary>
        public ICommand BackSpaceCommand
        {
            get
            {
                if (backSpaceCommand == null)
                {
                    backSpaceCommand = new RelayCommand(p =>
                        this.BackSpaceInvoked());
                }
                return backSpaceCommand;
            }
        }

        /// <summary>
        /// Relay command to execute the clear phone number action from the dialer view.
        /// </summary>
        public ICommand BackSpaceHoldingCommand
        {
            get
            {
                if (backSpaceHoldingCommand == null)
                {
                    backSpaceHoldingCommand = new RelayCommand(p =>
                        this.ClearDialerNumberHeap());
                }
                return backSpaceHoldingCommand;
            }
        }

        #region Setters and Getters
        /// <summary>
        /// Appends the latest dialed character to the number field.
        /// </summary>
        public string Digit
        {
            set
            {
                cleanInput(value);
            }
        }

        /// <summary>
        /// Gets and sets the phone number to be dialed.
        /// </summary>
        public string NumberToDial
        {
            get
            { 
                return phone.FormatString(numberToDial);
            }
            private set
            {
                if (numberToDial != value)
                {
                    numberToDial = phone.FormatString(value);
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the state of the dialer (enable/disable the call and backspace button).
        /// </summary>
        public bool DialPadEnabled
        {
            get
            {
                return dialPadEnabled;
            }
            private set
            {
                if (dialPadEnabled != value)
                {
                    dialPadEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }
         
        #endregion

    }
}
