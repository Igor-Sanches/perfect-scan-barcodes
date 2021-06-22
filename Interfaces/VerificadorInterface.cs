using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_Scan.Interfaces
{
    public interface VerificadorInterface
    {
        bool IsContato();
        bool IsNumeroTelefone();
        bool IsWhatsApp();
        bool IsAppAndroid();
        bool IsAppWindows();
        bool IsWIFI(); 
        bool IsProduto();
        bool IsEMAIL();
        bool IsSMS(); 
        bool IsUri();
    }
}
