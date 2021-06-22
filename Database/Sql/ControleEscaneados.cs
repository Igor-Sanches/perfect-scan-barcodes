using Data_Base.Interfaces;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_Scan.Database.Sql
{
    public class ControleEscaneados : IRepositorioEscaneados<Escaneados>
    {
        private ConexaoSql conexao;
        private Settings.Settings settings;
        public ControleEscaneados()
        {
            conexao = new ConexaoSql();
            settings = new Settings.Settings();
        }

        public bool Atualizar(Escaneados escaneados)
        {
            try
            {
                conexao._conexao.Update(escaneados);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Excluir(Escaneados escaneados)
        {
            try
            {
                conexao._conexao.Delete(escaneados);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Limpa()
        {
            try
            {
                conexao._conexao.DeleteAll<Escaneados>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Escaneados> ListaEscaneados()
        {
            List<Escaneados> lista = conexao._conexao.Table<Escaneados>().OrderBy(name => name.DisplayName).ToList();
            switch (Data.Data.SortingEscaneados)
            {
                case 0:
                    lista = conexao._conexao.Table<Escaneados>().OrderBy(name => name.DisplayName).ToList();
                    break;
                case 1:
                    lista = conexao._conexao.Table<Escaneados>().OrderByDescending(name => name.DisplayName).ToList();
                    break;
                case 2:
                    lista = conexao._conexao.Table<Escaneados>().OrderBy(date => date.Data).ToList();
                    break;
                case 3:
                    lista = conexao._conexao.Table<Escaneados>().OrderByDescending(date => date.Data).ToList();
                    break;
                case 4:
                    lista = conexao._conexao.Table<Escaneados>().OrderBy(tipo => tipo.Tipo).ToList();
                    break;
                case 5:
                    lista = conexao._conexao.Table<Escaneados>().OrderBy(formato => formato.Formato).ToList();
                    break;
            }
            return lista;
        }

        public Escaneados GetID(long ID)
        {
            if (ID > 0)
            {
                return conexao._conexao.Table<Escaneados>().FirstOrDefault(m => m.Data == DateTime.Now.ToString());
            }
            return null;
        }

        public async Task<bool> SalvarAsync(Escaneados escaneados)
        {
            bool ok = false;
            try
            {
               if(settings.IsSalvarEscaneados)
                {
                    bool existe = false;
                    var lista = ListaEscaneados();
                    if (lista.Count > 0)
                    {
                        System.Collections.IList list = lista;
                        for (int i = 0; i < list.Count; i++)
                        {
                            Gerados gerado = (Gerados)list[i];
                            if (gerado.Codigo.Equals(escaneados.Codigo) && gerado.Formato.Equals(escaneados.Formato))
                            {
                                existe = true;
                            }
                        }
                    }

                    if (!existe)
                    {
                        conexao._conexao.Insert(escaneados);
                        if (Data.Conta.IsNuvemToEscaneados)
                        {
                            try
                            {
                                if (NetworkInterface.GetIsNetworkAvailable())
                                {
                                    HistoricoNuvem nuvem = new HistoricoNuvem();
                                    nuvem.UUID = escaneados.UUID;
                                    nuvem.Titulo = escaneados.DisplayName;
                                    nuvem.Codigo = escaneados.Codigo;
                                    nuvem.Formato = escaneados.Formato;
                                    nuvem.Data = escaneados.Data;
                                    nuvem.Tipo = escaneados.Tipo;
                                    nuvem.DbType = "Escaneado";
                                    var db = App.GetInstance.GetDatabase;
                                    await db.Child($"{Data.Conta.UUID}/Escaneados/{nuvem.UUID}").PutAsync(nuvem).ContinueWith(p => { if (!p.IsCompleted) { Data.Data.IsFaulted = true; } });
                                }
                            }
                            catch
                            {

                            }
                            ok = true;

                        }
                    }
                } 
            }
            catch
            { 
            }
            return ok;
        }
    }
}
