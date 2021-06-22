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
    public class ControleGerados : IRepositorioGerados<Gerados>
    {
        private ConexaoSql conexao;
        private Settings.Settings settings;
        public ControleGerados()
        {
            conexao = new ConexaoSql();
            settings = new Settings.Settings();
        }


        public bool Atualizar(Gerados gerados)
        {
            try
            {
                conexao._conexao.Update(gerados);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Excluir(Gerados gerados)
        {
            try
            {
                conexao._conexao.Delete(gerados);
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
                conexao._conexao.DeleteAll<Gerados>();
                return true;
            }
            catch
            {
                return false;
            }
        }
         
        public List<Gerados> ListaGerados()
        { 
            List<Gerados> lista = conexao._conexao.Table<Gerados>().OrderBy(name => name.DisplayName).ToList();
            switch (Data.Data.SortingGerados)
            {
                case 0:
                    lista = conexao._conexao.Table<Gerados>().OrderBy(name => name.DisplayName).ToList();
                    break;
                case 1:
                    lista = conexao._conexao.Table<Gerados>().OrderByDescending(name => name.DisplayName).ToList();
                    break;
                case 2:
                    lista = conexao._conexao.Table<Gerados>().OrderBy(date => date.Data).ToList();
                    break;
                case 3:
                    lista = conexao._conexao.Table<Gerados>().OrderByDescending(date => date.Data).ToList();
                    break;
                case 4:
                    lista = conexao._conexao.Table<Gerados>().OrderBy(tipo => tipo.Tipo).ToList();
                    break;
                case 5:
                    lista = conexao._conexao.Table<Gerados>().OrderBy(formato => formato.Formato).ToList();
                    break;
            }
            return lista;
        }

        public Gerados GetID(long ID)
        {
            if (ID > 0)
            {
                return conexao._conexao.Table<Gerados>().FirstOrDefault(m => m.Data == DateTime.Now.ToString());
            }
            return null;
        }

        public async Task<bool> SalvarAsync(Gerados gerados)
        {
            bool ok = false;
            try
            {
                if (settings.IsSalvarGerados)
                {
                    bool existe = false;
                    var lista = ListaGerados();
                    if (lista.Count > 0)
                    {
                        foreach (Gerados gerado in lista)
                        {
                            if (gerado.Codigo.Equals(gerados.Codigo) && gerado.Formato.Equals(gerados.Formato))
                            {
                                existe = true;
                            }
                        }
                    }

                    if (!existe)
                    {
                        conexao._conexao.Insert(gerados);
                        if (Data.Conta.IsNuvemToGerados)
                        {
                            try
                            {
                                if (NetworkInterface.GetIsNetworkAvailable())
                                {
                                    HistoricoNuvem nuvem = new HistoricoNuvem();
                                    nuvem.UUID = gerados.UUID;
                                    nuvem.Titulo = gerados.DisplayName;
                                    nuvem.Codigo = gerados.Codigo;
                                    nuvem.Formato = gerados.Formato;
                                    nuvem.Data = gerados.Data;
                                    nuvem.Tipo = gerados.Tipo;
                                    nuvem.DbType = "Gerado";
                                    var db = App.GetInstance.GetDatabase;
                                    await db.Child($"{Data.Conta.UUID}/Gerados/{nuvem.UUID}").PutAsync(nuvem).ContinueWith(p => { if (!p.IsCompleted) { Data.Data.IsFaulted = true; } });
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
