
using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioIntegrante
    {
        Integrante Obter(long id);
        Integrante Criar(Integrante integrante);
        void VerificarSeCpfJaExiste(string cpf);
        void VerificarSeEmailJaExiste(string email);
        void Remover(Integrante integrante);
        void Salvar();
        IEnumerable<Animal> Animais();
    }
}
