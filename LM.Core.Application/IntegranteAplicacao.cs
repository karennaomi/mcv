using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System.Linq;

namespace LM.Core.Application
{
    public interface IIntegranteAplicacao
    {
        Integrante Criar(Integrante integrante, string tipoPersona);
        void Apagar(long usuarioId, long pontoDemandaId, long integranteId);
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

        public Integrante Criar(Integrante integrante, string tipoPersona)
        {
            return _repositorio.Criar(integrante);
        }

        public void Apagar(long usuarioId, long pontoDemandaId, long integranteId)
        {
            var pontoDemanda = _appPontoDemanda.Obter(usuarioId, pontoDemandaId);
            if (pontoDemanda.GrupoDeIntegrantes.Integrantes.All(i => i.Id != integranteId)) throw new IntegranteNaoPertenceAPontoDemandaException();
            _repositorio.Apagar(integranteId);
        }
    }
}
