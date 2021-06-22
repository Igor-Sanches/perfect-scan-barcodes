using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Perfect_Scan.Database.Sql
{
    public class ConexaoSql : IDisposable
    {
        public SQLiteConnection _conexao;

        public ConexaoSql()
        {
            //Criar local onde ficara salvo o banco de dados
            _conexao = new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "Vitória2020.oic"));

            //Criar a tabela Salvos
            _conexao.CreateTable<Gerados>();
            //Criar a tabela Escaneados
            _conexao.CreateTable<Escaneados>();

        }

        public void Dispose()
        {
            _conexao.Dispose();
        }
    }
}
  