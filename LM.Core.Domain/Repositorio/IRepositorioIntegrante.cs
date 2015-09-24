
using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioIntegrante
    {
        Integrante Obter(long id);
        Integrante Obter(string email);
        Integrante Criar(Integrante integrante);
        void VerificarSeCpfJaExiste(string cpf);
        void VerificarSeEmailJaExiste(string email);
        void Remover(Integrante integrante);
        void RemoverGrupo(Integrante  integrante, long pontoDemandaId);
        IEnumerable<Animal> Animais();
    }
}
