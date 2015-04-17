using LM.Core.Domain;
using System.Collections.ObjectModel;

namespace LM.Core.RepositorioEF
{
    class ComandoAdicionarLojaFavorita
    {
        private readonly ContextoEF _contexto;
        private readonly CidadeEF _cidadeRepo;
        private readonly PontoDemanda _pontoDemanda;
        private readonly Loja _novaLoja;
        public ComandoAdicionarLojaFavorita(ContextoEF contexto, PontoDemanda pontoDemanda, Loja loja)
        {
            _contexto = contexto;
            _cidadeRepo = new CidadeEF(_contexto);
            _pontoDemanda = pontoDemanda;
            _novaLoja = loja;
        }

        internal void Executar()
        {
            if (_pontoDemanda.LojasFavoritas == null) _pontoDemanda.LojasFavoritas = new Collection<Loja>();
            _pontoDemanda.LojasFavoritas.Add(_novaLoja);
            _contexto.SaveChanges();
        }
    }
}
