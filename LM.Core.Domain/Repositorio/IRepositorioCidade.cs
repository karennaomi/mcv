using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioCidade
    {
        Cidade Buscar(string nome);
        IList<Cidade> Listar(int ufId);
    }
}
