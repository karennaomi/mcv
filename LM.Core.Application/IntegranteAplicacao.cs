using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System.Linq;

namespace LM.Core.Application
{
    public interface IIntegranteAplicacao
    {
        Integrante Criar(Integrante integrante);
        void Apagar(long id, long pontoDemandaId);
    }

    public class IntegranteAplicacao : IIntegranteAplicacao
    {
        private readonly IRepositorioIntegrante _repositorio;
        private readonly IPontoDemandaAplicacao _appPontoDemanda;
        public IntegranteAplicacao(IRepositorioIntegrante repositorio, IPontoDemandaAplicacao appPontoDemanda)
        {
            _repositorio = repositorio;
            _appPontoDemanda = appPontoDemanda;
        }

        public Integrante Criar(Integrante integrante)
        {
            return _repositorio.Criar(integrante);
        }

        public void Apagar(long id, long pontoDemandaId)
        {
            var pontoDemanda = _appPontoDemanda.Obter(pontoDemandaId);
            if (!pontoDemanda.GrupoDeIntegrantes.Integrantes.Any(i => i.Id == id)) throw new IntegranteNaoPertenceAPontoDemandaException();
            _repositorio.Apagar(id);
        }
    }
}
