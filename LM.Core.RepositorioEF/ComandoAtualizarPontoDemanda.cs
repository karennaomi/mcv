using System;
using LM.Core.Domain;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace LM.Core.RepositorioEF
{
    public class ComandoAtualizarPontoDemanda
    {
        private readonly ContextoEF _contexto;
        private readonly CidadeEF _cidadeRepo;
        private readonly PontoDemanda _pontoDemandaToUpdate;
        private readonly PontoDemanda _pontoDemanda;
        public ComandoAtualizarPontoDemanda(ContextoEF contexto, PontoDemanda pontoDemandaToUpdate, PontoDemanda pontoDemanda)
        {
            _contexto = contexto;
            _cidadeRepo = new CidadeEF(_contexto);
            _pontoDemandaToUpdate = pontoDemandaToUpdate;
            _pontoDemanda = pontoDemanda;
        }

        public PontoDemanda Executar()
        {
            _pontoDemandaToUpdate.Nome = _pontoDemanda.Nome;
            _pontoDemandaToUpdate.DataAlteracao = DateTime.Now;

            if (_pontoDemanda.Endereco.Cidade.Id > 0)
            {
                _contexto.Entry(_pontoDemanda.Endereco.Cidade).State = EntityState.Unchanged;
            }
            else
            {
                _pontoDemanda.Endereco.Cidade = _cidadeRepo.Buscar(_pontoDemanda.Endereco.Cidade.Nome);
            }
            _pontoDemandaToUpdate.Endereco = _pontoDemanda.Endereco;
            _pontoDemandaToUpdate.Endereco.Alias = _pontoDemanda.Nome;
            _contexto.SaveChanges();
            return _pontoDemandaToUpdate;
        }
    }
}
