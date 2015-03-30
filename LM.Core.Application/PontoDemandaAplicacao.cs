using System.Linq;
using System.Runtime.Remoting;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IPontoDemandaAplicacao : IRelacionaUsuario
    {
        PontoDemanda Criar(PontoDemanda pontoDemanda);
        IList<PontoDemanda> Listar();
        PontoDemanda Obter(long id);
        PontoDemanda DefinirFrequenciaDeConsumo(long id, int frequencia);
        long VerificarPontoDemanda(long id);
    }

    public class PontoDemandaAplicacao : RelacionaUsuario, IPontoDemandaAplicacao
    {
        private readonly IRepositorioPontoDemanda _repositorio;
        private readonly IUsuarioAplicacao _appUsuario;
        private readonly ICidadeAplicacao _appCidade;

        public PontoDemandaAplicacao(IRepositorioPontoDemanda repositorio, IUsuarioAplicacao appUsuario, ICidadeAplicacao appCidade)
            : this(repositorio, appUsuario, appCidade, 0)
        {
        }

        public PontoDemandaAplicacao(IRepositorioPontoDemanda repositorio, IUsuarioAplicacao appUsuario, ICidadeAplicacao appCidade, long usuarioId) : base(usuarioId)
        {
            _repositorio = repositorio;
            _appUsuario = appUsuario;
            _appCidade = appCidade;
        }

        public PontoDemanda Criar(PontoDemanda pontoDemanda)
        {
            pontoDemanda.GrupoDeIntegrantes = ObterGrupoDeIntegrantesDoUsuario();
            pontoDemanda.GrupoDeIntegrantes.Nome = "Integrantes: " + pontoDemanda.Nome;
            pontoDemanda.Endereco.Cidade = new Cidade { Id = _appCidade.Buscar(pontoDemanda.Endereco.Cidade.Nome).Id };
            pontoDemanda = _repositorio.Criar(pontoDemanda);
            _appUsuario.AtualizarStatusCadastro(UsuarioId, StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta, pontoDemanda.Id);
            return pontoDemanda;
        }

        private GrupoDeIntegrantes ObterGrupoDeIntegrantesDoUsuario()
        {
            return _appUsuario.Obter(UsuarioId).Integrante.GrupoDeIntegrantes;
        }

        public IList<PontoDemanda> Listar()
        {
            return _repositorio.Listar(UsuarioId);
        }

        public PontoDemanda Obter(long id)
        {
            return _repositorio.Obter(id, UsuarioId);
        }

        public PontoDemanda DefinirFrequenciaDeConsumo(long id, int frequencia)
        {
            var pontoDemanda = Obter(id);
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

        public long VerificarPontoDemanda(long id)
        {
            var pontosDemanda = Listar();
            if (pontosDemanda.All(p => p.Id != id)) throw new PontoDemandaInvalidoException("Ponto de demanda não pertence ao usuário atual.");
            return id;
        }
    }
}
