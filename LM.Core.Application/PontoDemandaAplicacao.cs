using System.Linq;
using System.Runtime.Remoting;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IPontoDemandaAplicacao
    {
        PontoDemanda Criar(long usuarioId, PontoDemanda pontoDemanda);
        IList<PontoDemanda> Listar(long usuarioId);
        PontoDemanda Obter(long usuarioId, long pontoDemandaId);
        PontoDemanda DefinirFrequenciaDeConsumo(long usuarioId, long pontoDemandaId, int frequencia);
        long VerificarPontoDemanda(long usuarioId, long pontoDemandaId);
    }

    public class PontoDemandaAplicacao : IPontoDemandaAplicacao
    {
        private readonly IRepositorioPontoDemanda _repositorio;
        private readonly IUsuarioAplicacao _appUsuario;
        private readonly ICidadeAplicacao _appCidade;

        public PontoDemandaAplicacao(IRepositorioPontoDemanda repositorio, IUsuarioAplicacao appUsuario, ICidadeAplicacao appCidade)
        {
            _repositorio = repositorio;
            _appUsuario = appUsuario;
            _appCidade = appCidade;
        }

        public PontoDemanda Criar(long usuarioId, PontoDemanda pontoDemanda)
        {
            pontoDemanda.GrupoDeIntegrantes = ObterGrupoDeIntegrantesDoUsuario(usuarioId);
            pontoDemanda.GrupoDeIntegrantes.Nome = "Integrantes: " + pontoDemanda.Nome;
            pontoDemanda.Endereco.Cidade = new Cidade { Id = _appCidade.Buscar(pontoDemanda.Endereco.Cidade.Nome).Id };
            pontoDemanda = _repositorio.Criar(pontoDemanda);
            _appUsuario.AtualizarStatusCadastro(usuarioId, StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta, pontoDemanda.Id);
            return pontoDemanda;
        }

        private GrupoDeIntegrantes ObterGrupoDeIntegrantesDoUsuario(long usuarioId)
        {
            return _appUsuario.Obter(usuarioId).Integrante.GrupoDeIntegrantes;
        }

        public IList<PontoDemanda> Listar(long usuarioId)
        {
            return _repositorio.Listar(usuarioId);
        }

        public PontoDemanda Obter(long usuarioId, long pontoDemandaId)
        {
            return _repositorio.Obter(usuarioId, pontoDemandaId);
        }

        public PontoDemanda DefinirFrequenciaDeConsumo(long usuarioId, long pontoDemandaId, int frequencia)
        {
            var pontoDemanda = Obter(usuarioId, pontoDemandaId);
            switch (frequencia)
            {
                case 1:
                    pontoDemanda.QuantidadeDiasAlertaReposicao = 7;
                    break;
                case 2:
                    pontoDemanda.QuantidadeDiasAlertaReposicao = 14;
                    break;
                case 3:
                    pontoDemanda.QuantidadeDiasAlertaReposicao = 28;
                    break;
            }
            pontoDemanda.QuantidadeDiasCoberturaEstoque = 3;
            _repositorio.SalvarAlteracoes();
            return pontoDemanda;
        }

        public long VerificarPontoDemanda(long usuarioId, long pontoDemandaId)
        {
            var pontosDemanda = Listar(usuarioId);
            if (pontosDemanda.All(p => p.Id != pontoDemandaId)) throw new PontoDemandaInvalidoException("Ponto de demanda não pertence ao usuário atual.");
            return pontoDemandaId;
        }
    }
}
