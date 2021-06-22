using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_Scan.ViewModel
{
    public class GeradorViewModel : ViewModelBase
    { 

        public bool IsMulti
        {
            get { return Data.Data.IsMulti; }
        }
    }
}
