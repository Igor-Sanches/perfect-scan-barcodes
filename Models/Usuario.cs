using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Perfect_Scan.Models
{
    public class Usuario
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string KeyPass { get; set; }
        public string Data { get; set; }
        public string SistemaOperacional { get; set; }
        public string LoginSessão { get; set; }
    }
}
