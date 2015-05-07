using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System.Linq;

namespace LM.Core.Application
{
    public interface IIntegranteAplicacao
    {
        Integrante Obter(long id);
        Integrante Criar(Integrante integrante);
        Integrante Atualizar(Integrante integrante);
        void Apagar(long pontoDemandaId, long integranteId);
        void VerificarSeCpfJaExiste(string cpf);
        void VerificarSeEmailJaExiste(string email);
    }

    public class IntegranteAplicacao : IIntegranteAplicacao
    {
        private readonly IRepositorioIntegrante _repositorio;
        public IntegranteAplicacao(IRepositorioIntegrante repositorio)
        {
            _repositorio = repositorio;
        }

        public Integrante Obter(long id)
        {
            return _repositorio.Obter(id);
        }

        public Integrante Criar(Integrante integrante)
        {
            return _repositorio.Criar(integrante);
        }

        public Integrante Atualizar(Integrante integrante)
        {
            var integranteToUpdate = Obter(integrante.Id);
            if (integranteToUpdate.Email != integrante.Email) VerificarSeEmailJaExiste(integrante.Email);
            integranteToUpdate.Atualizar(integrante);
            _repositorio.Salvar();
            return integranteToUpdate;
        }

        public void Apagar(long pontoDemandaId, long integranteId)
        {
            var integrante = Obter(integranteId);
            if (integrante.GrupoDeIntegrantes.PontosDemanda.All(p => p.Id != pontoDemandaId)) throw new IntegranteNaoPertenceAPontoDemandaException();
            _repositorio.Apagar(integranteId);
        }

        public void VerificarSeCpfJaExiste(string cpf)
        {
            _repositorio.VerificarSeCpfJaExiste(cpf);
        }

        public void VerificarSeEmailJaExiste(string email)
        {
            _repositorio.VerificarSeEmailJaExiste(email);
        }
    }
}
