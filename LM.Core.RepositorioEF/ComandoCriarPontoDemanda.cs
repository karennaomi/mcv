using System.Data.Entity;
using LM.Core.Domain;

namespace LM.Core.RepositorioEF
{
    public class ComandoCriarPontoDemanda
    {
        private readonly ContextoEF _contexto;
        private readonly UsuarioEF _usuarioRepo;
        private readonly long _usuarioId;
        private PontoDemanda _novoPontoDemanda;
        public ComandoCriarPontoDemanda(long usuarioId, PontoDemanda novoPontoDemanda)
        {
            _contexto = new ContextoEF();
            _usuarioRepo = new UsuarioEF(_contexto);
            _usuarioId = usuarioId;
            _novoPontoDemanda = novoPontoDemanda;
        }

        public PontoDemanda Executar()
        {
            _novoPontoDemanda.GrupoDeIntegrantes = ObterGrupoDeIntegrantesDoUsuario();
            _novoPontoDemanda.Endereco.Cidade = BuscarCidade();
            _novoPontoDemanda = _contexto.PontosDemanda.Add(_novoPontoDemanda);
            _contexto.SaveChanges();
            
            _usuarioRepo.AtualizarStatusCadastro(_usuarioId, StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta, _novoPontoDemanda.Id);
            _contexto.SaveChanges();

            return _novoPontoDemanda;
        }

        private GrupoDeIntegrantes ObterGrupoDeIntegrantesDoUsuario()
        {
            var usuario = _usuarioRepo.Obter(_usuarioId);
            return usuario.Integrante.GrupoDeIntegrantes;
        }

        private Cidade BuscarCidade()
        {
            var cidadeRepo = new CidadeEF(_contexto);
            var cidade = cidadeRepo.Buscar(_novoPontoDemanda.Endereco.Cidade.Nome);
            _contexto.Entry(cidade).State = EntityState.Unchanged;
            return cidade;
        }
    }
}
