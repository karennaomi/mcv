using System.Linq;
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
        void DefinirFrequenciaDeConsumo(long pontoDemandaId, long usuarioId, int frequencia);
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
            pontoDemanda.Endereco.Cidade = _appCidade.Buscar(pontoDemanda.Endereco.Cidade.Nome);
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

        public void DefinirFrequenciaDeConsumo(long pontoDemandaId, long usuarioId, int frequencia)
        {
            _repositorio.DefinirFrequenciaDeConsumo(pontoDemandaId, usuarioId, frequencia);
        }

        public long VerificarPontoDemanda(long id)
        {
            var pontosDemanda = Listar();
            if (pontosDemanda.All(p => p.Id != id)) throw new PontoDemandaInvalidoException("Ponto de demanda não pertence ao usuário atual.");
            return id;
        }
    }
}
