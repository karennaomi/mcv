using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioCorreios
    {
        EnderecoCorreios BuscarPorCep(string cep);
    }
}
