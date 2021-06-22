using Firebase.Database.Query;
using Perfect_Scan.Database;
using Perfect_Scan.Database.Sql;
using Perfect_Scan.Paginas.Visualizador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;
using ZXing;

namespace Perfect_Scan.Manager
{
    public class HistoricoManager
    {
        private static ResourceLoader loader = new ResourceLoader();
        public async static void Escaneado(Result result)
        {
            if(result != null)
            {
                if (!result.Text.Trim().Equals(""))
                {
                    var guid = Guid.NewGuid();
                    string uuid = guid.ToString();
                    string content = result.Text;
                    string formato = result.BarcodeFormat.ToString();
                    string tipo = CodigoRetorno.GetResultado(content, formato).ToString();
                    string nome = CodigoRetorno.GetName(formato);
                    Escaneados escaneado = new Escaneados();
                    ControleEscaneados controle = new ControleEscaneados();
                    escaneado.DisplayName = nome; 
                    escaneado.UUID = uuid;
                    escaneado.IsNuvem = Data.Conta.IsNuvemToEscaneados;
                    escaneado.Formato = formato;
                    escaneado.Tipo = tipo;
                    escaneado.Codigo = content;
                    escaneado.Data = GetData();
                    await controle.SalvarAsync(escaneado);
                }
            }
        }

        public async static void Gerado(string codigo, string formato, Frame frame)
        {
            if (!codigo.Trim().Equals(""))
            {
                var guid = Guid.NewGuid();
                string uuid = guid.ToString();
                string tipo = CodigoRetorno.GetResultado(codigo, formato).ToString();
                string nome = CodigoRetorno.GetName(formato);
                Gerados gerado = new Gerados();
                ControleGerados controle = new ControleGerados();
                gerado.DisplayName = nome;
                gerado.UUID = uuid;
                gerado.IsNuvem = Data.Conta.IsNuvemToGerados;
                gerado.Formato = formato;
                gerado.Tipo = tipo;
                gerado.Codigo = codigo;
                gerado.Data = GetData();
                await controle.SalvarAsync(gerado);
                if (Data.Data.IsMulti)
                {
                    Paginas.Root.RootApp.Instance.GetToast(loader.GetString("salvo"), Tools.ModoColor.Succes);
                }
                else
                {
                    frame.Navigate(typeof(Visualizador), gerado);
                }    
            }

        }

        public async static Task<bool> EscaneadoAsync(List<Escaneados> lista_, bool v)
        {
            bool ok = false;
            ControleEscaneados controle = new ControleEscaneados();
            if (lista_ != null && lista_.Count > 0)
            {
                if (v) { controle.Limpa(); }

                foreach(var item in lista_)
                {
                   ok = await controle.SalvarAsync(item);
                }
            }
            return ok;
        }

        private static string GetData()
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }

        public async static Task<bool> GeradoAsync(List<Gerados> lista_, bool v)
        {
            bool ok = false;
            ControleGerados controle = new ControleGerados();
            if (lista_ != null && lista_.Count > 0)
            {
                if (v) { controle.Limpa(); }

                foreach (var item in lista_)
                {
                    ok = await controle.SalvarAsync(item);
                }
            }
            return ok;
        }
    }
}
