using System;
using LM.Core.Domain;

namespace LM.Core.RepositorioEF
{
    public class ComandoAtualizarPontoDemanda
    {
        private readonly ContextoEF _contexto;
        private readonly PontoDemanda _pontoDemandaToUpdate;
        private readonly PontoDemanda _pontoDemanda;
        public ComandoAtualizarPontoDemanda(ContextoEF contexto, PontoDemanda pontoDemandaToUpdate, PontoDemanda pontoDemanda)
        {
            _contexto = contexto;
            _pontoDemandaToUpdate = pontoDemandaToUpdate;
            _pontoDemanda = pontoDemanda;
        }

        public PontoDemanda Executar()
        {
            _pontoDemandaToUpdate.Nome = _pontoDemanda.Nome;
            _pontoDemandaToUpdate.DataAlteracao = DateTime.Now;
            _pontoDemandaToUpdate.Endereco = _pontoDemanda.Endereco;
            _pontoDemandaToUpdate.Endereco.Alias = _pontoDemanda.Nome;
            _contexto.SaveChanges();
            return _pontoDemandaToUpdate;
        }
    }
}
