using LM.Core.Domain;
using System.Collections.ObjectModel;

namespace LM.Core.RepositorioEF
{
    class ComandoAdicionarLojaFavorita
    {
        private readonly ContextoEF _contexto;
        private readonly PontoDemanda _pontoDemanda;
        private readonly LojaFavoritaEF _lojaFavoritaRepo;
        private Loja _novaLoja;
        public ComandoAdicionarLojaFavorita(ContextoEF contexto, PontoDemanda pontoDemanda, Loja loja)
        {
            _contexto = contexto;
            _pontoDemanda = pontoDemanda;
            _novaLoja = loja;
            _lojaFavoritaRepo= new LojaFavoritaEF(_contexto);
        }

        internal Loja Executar()
        {
            if (_pontoDemanda.LojasFavoritas == null) _pontoDemanda.LojasFavoritas = new Collection<Loja>();
            _novaLoja = _lojaFavoritaRepo.VerificarLojaExistente(_novaLoja);
            _pontoDemanda.LojasFavoritas.Add(_novaLoja);
            _contexto.SaveChanges();
            return _novaLoja;
        }
    }
}
