using LM.Core.Domain;
using System.Collections.ObjectModel;

namespace LM.Core.RepositorioEF
{
    public class ComandoCriarPontoDemanda
    {
        private readonly ContextoEF _contexto;
        private readonly UsuarioEF _usuarioRepo;
        private readonly LojaFavoritaEF _lojaFavoritaRepo;
        private readonly long _usuarioId;
        private PontoDemanda _novoPontoDemanda;
        public ComandoCriarPontoDemanda(long usuarioId, PontoDemanda novoPontoDemanda)
        {
            _contexto = new ContextoEF();
            _usuarioRepo = new UsuarioEF(_contexto);
            _lojaFavoritaRepo = new LojaFavoritaEF(_contexto);
            _usuarioId = usuarioId;
            _novoPontoDemanda = novoPontoDemanda;
        }

        public PontoDemanda Executar()
        {
            var usuario = _usuarioRepo.Obter(_usuarioId);
            _novoPontoDemanda.UsuarioCriador = usuario;
            _novoPontoDemanda.GruposDeIntegrantes = new Collection<GrupoDeIntegrantes> { 
                new GrupoDeIntegrantes { Integrante = usuario.Integrante, Papel = PapelIntegrante.Administrador }
            };
            LojasFavoritas();
            if (_novoPontoDemanda.Listas == null) _novoPontoDemanda.Listas = new Collection<Lista> { new Lista() };
            _novoPontoDemanda = _contexto.PontosDemanda.Add(_novoPontoDemanda);
            _contexto.SaveChanges();
            return _novoPontoDemanda;
        }

        private void LojasFavoritas()
        {
            if (_novoPontoDemanda.LojasFavoritas == null) return;
            var lojas = new Collection<Loja>();
            foreach (var lojaFavorita in _novoPontoDemanda.LojasFavoritas)
            {
                lojas.Add(_lojaFavoritaRepo.VerificarLojaExistente(lojaFavorita));
            }
            _novoPontoDemanda.LojasFavoritas = lojas;
        }
    }
}
