using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_Scan.ViewModel
{
    public class HistoricosViewModel : ViewModelBase
    {
        private bool isOpen = false;
        public bool IsOpenHistorical
        {
            get
            {
                return isOpen;
            }
            private set
            {
                if(isOpen!= value)
                {
                    isOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public void OnNavigation(bool value)
        {
            IsOpenHistorical = value;
        }
    }
}
