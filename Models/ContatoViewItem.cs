using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Perfect_Scan.Models
{
    public enum TipoContato
    {
        celular, email, link, aniversario, nota, endereco
    }

    public class ContatoViewItem
    {
        private ResourceLoader loader = new ResourceLoader();
        public Visibility LayoutNumber
        {
            get
            {
                return Tipo == TipoContato.celular ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        public Visibility LayoutOuters
        {
            get
            {
                return Tipo != TipoContato.celular && Tipo != TipoContato.aniversario ? Visibility.Visible : Visibility.Collapsed;
            }
        } 

        public string ButtonContent
        {
            get
            {
                String retorno = "";
                switch (Tipo)
                {
                    case TipoContato.email: retorno = loader.GetString("emailContent"); break;
                    case TipoContato.endereco: retorno = loader.GetString("mapContent"); break;
                    case TipoContato.link: retorno = loader.GetString("linkContent"); break;
                    case TipoContato.nota: retorno = loader.GetString("notaContent"); break;
                }
                return retorno;
            }
        }
            
        public TipoContato Tipo { get; set; }
        public string Home { get; set; }
        public string SubHome { get; set; }
        public string Content { get; set; }
}
}
