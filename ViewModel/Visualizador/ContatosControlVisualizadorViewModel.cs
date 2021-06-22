using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Perfect_Scan.Manager.ResultScanner;
using Perfect_Scan.Models;

namespace Perfect_Scan.ViewModel.Visualizador
{
    public class ContatosControlVisualizadorViewModel : ViewModelBase
    {
        internal void PuContato(ContatoResult contato)
        {
            try
            {
                ContatoViewItem item = new ContatoViewItem();
                List<ContatoViewItem> viewItems = new List<ContatoViewItem>();
                var phones = contato.Phones;
                foreach(var phone in phones)
                {
                    item.Tipo = TipoContato.celular;
                    item.Home = phone.phoneType.ToString();
                    item.SubHome = phone.number;
                    item.Content = phone.number;
                    viewItems.Add(item);
                }

            }
            catch
            {

            }
        }
    }
}
