using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_Scan.Models
{
    public class ContaUsuario
    {
        public string DisplayName { get; set; }
        public string Data { get; set; }
        public bool IsIlimited { get; set; }
        public string Email { get; set; }
        public string UUID { get; set; }
        public string KeyPass { get; set; }
    }
}
