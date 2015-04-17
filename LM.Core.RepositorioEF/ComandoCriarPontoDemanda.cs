using LM.Core.Domain;
using System.Collections.ObjectModel;

namespace LM.Core.RepositorioEF
{
    public class ComandoCriarPontoDemanda
    {
        private readonly ContextoEF _contexto;
        private readonly UsuarioEF _usuarioRepo;
        private readonly CidadeEF _cidadeRepo;
        private readonly long _usuarioId;
        private PontoDemanda _novoPontoDemanda;
        public ComandoCriarPontoDemanda(long usuarioId, PontoDemanda novoPontoDemanda)
        {
            _contexto = new ContextoEF();
            _usuarioRepo = new UsuarioEF(_contexto);
            _cidadeRepo = new CidadeEF(_contexto);
            _usuarioId = usuarioId;
            _novoPontoDemanda = novoPontoDemanda;
        }

        public PontoDemanda Executar()
        {
            _novoPontoDemanda.GrupoDeIntegrantes = ObterGrupoDeIntegrantesDoUsuario();
            _novoPontoDemanda.Endereco.Cidade = _cidadeRepo.Buscar(_novoPontoDemanda.Endereco.Cidade.Nome);
            _novoPontoDemanda = _contexto.PontosDemanda.Add(_novoPontoDemanda);
            if (_novoPontoDemanda.Listas == null) _novoPontoDemanda.Listas = new Collection<Lista>{ new Lista() };
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
    }
}
