using LM.Core.Domain;
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
    }

    public class PontoDemandaAplicacao : RelacionaUsuario, IPontoDemandaAplicacao
    {
        private readonly IRepositorioPontoDemanda _repositorio;
        private readonly IUsuarioAplicacao _appUsuario;

        public PontoDemandaAplicacao(IRepositorioPontoDemanda repositorio, IUsuarioAplicacao appUsuario) : this(repositorio, appUsuario, 0)
        {
        }

        public PontoDemandaAplicacao(IRepositorioPontoDemanda repositorio, IUsuarioAplicacao appUsuario, long usuarioId) : base(usuarioId)
        {
            _repositorio = repositorio;
            _appUsuario = appUsuario;
        }

        public PontoDemanda Criar(PontoDemanda pontoDemanda)
        {
            pontoDemanda.GrupoDeIntegrantes = new GrupoDeIntegrantes
            {
                Integrantes = new[] {new Integrante {EhUsuarioSistema = true, Usuario = new Usuario {Id = UsuarioId}}}
            };
            pontoDemanda = _repositorio.Salvar(pontoDemanda);
            _appUsuario.AtualizaStatusCadastro(UsuarioId, StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta, pontoDemanda.Id);
            return pontoDemanda;
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
        //public IEnumerable<PontoDemanda> ListarPontosDemanda()
       //{
       //    return _repositorio.ListarPontoDemanda();
       //}

       // public PontoDemanda BuscaPontoDemandaPorUsuario(int usuarioId)
       // {
       //     return _repositorio.BuscarPontoDemandaPorUsuario(usuarioId);

       // }


        
    }
}
