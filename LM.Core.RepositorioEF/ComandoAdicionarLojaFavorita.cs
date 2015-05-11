﻿using LM.Core.Domain;
using System.Collections.ObjectModel;

namespace LM.Core.RepositorioEF
{
    class ComandoAdicionarLojaFavorita
    {
        private readonly ContextoEF _contexto;
        private readonly PontoDemanda _pontoDemanda;
        private readonly Loja _novaLoja;
        private readonly LojaFavoritaEF _lojaFavoritaRepo;
        private readonly CidadeEF _cidadeRepo;
        public ComandoAdicionarLojaFavorita(ContextoEF contexto, PontoDemanda pontoDemanda, Loja loja)
        {
            _contexto = contexto;
            _pontoDemanda = pontoDemanda;
            _novaLoja = loja;
            _lojaFavoritaRepo= new LojaFavoritaEF(_contexto);
            _cidadeRepo = new CidadeEF(_contexto);
        }

        internal void Executar()
        {
            if (_pontoDemanda.LojasFavoritas == null) _pontoDemanda.LojasFavoritas = new Collection<Loja>();
            _novaLoja.Info.Endereco.Cidade = _cidadeRepo.Buscar(_novaLoja.Info.Endereco.Cidade.Nome);
            _pontoDemanda.LojasFavoritas.Add(_lojaFavoritaRepo.VerificarLojaExistente(_novaLoja));
            _contexto.SaveChanges();
        }
    }
}
