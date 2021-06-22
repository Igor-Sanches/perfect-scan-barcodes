using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Base.Interfaces
{
    public interface IRepositorioGerados <T> where T: class
    {
        T GetID(long ID);
         Task<bool> SalvarAsync(T Gerados);
        bool Atualizar(T Gerados);
        bool Excluir(T Gerados);
        bool Limpa();
         List<T> ListaGerados();
    }
}
