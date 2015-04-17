using System.Linq;
using System.Runtime.Serialization.Formatters;
using LM.Core.Domain;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace LM.Core.RepositorioEF
{
    public class ComandoCriarPontoDemanda
    {
        private readonly ContextoEF _contexto;
        private readonly UsuarioEF _usuarioRepo;
        private readonly CidadeEF _cidadeRepo;
        private readonly LojaFavoritaEF _lojaFavoritaRepo;
        private readonly long _usuarioId;
        private PontoDemanda _novoPontoDemanda;
        public ComandoCriarPontoDemanda(long usuarioId, PontoDemanda novoPontoDemanda)
        {
            _contexto = new ContextoEF();
            _usuarioRepo = new UsuarioEF(_contexto);
            _cidadeRepo = new CidadeEF(_contexto);
            _lojaFavoritaRepo = new LojaFavoritaEF(_contexto);
            _usuarioId = usuarioId;
            _novoPontoDemanda = novoPontoDemanda;
        }

        public PontoDemanda Executar()
        {
            _novoPontoDemanda.GrupoDeIntegrantes = ObterGrupoDeIntegrantesDoUsuario();
            _novoPontoDemanda.Endereco.Cidade = _cidadeRepo.Buscar(_novoPontoDemanda.Endereco.Cidade.Nome);
            var lojas = new Collection<Loja>();
            foreach (var lojaFavorita in _novoPontoDemanda.LojasFavoritas)
            {
                lojas.Add(_lojaFavoritaRepo.VerificarLojaExistente(lojaFavorita));
            }
            _novoPontoDemanda.LojasFavoritas = lojas;
            if (_novoPontoDemanda.Listas == null) _novoPontoDemanda.Listas = new Collection<Lista> { new Lista() };
            
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
    }
}
