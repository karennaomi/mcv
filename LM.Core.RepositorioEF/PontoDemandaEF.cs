using System;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class PontoDemandaEF : IRepositorioPontoDemanda
    {
        private readonly ContextoEF _contexto;
        public PontoDemandaEF()
        {
            _contexto = new ContextoEF();
        }

        public IList<PontoDemanda> Listar(long usuarioId)
        {
            return _contexto.PontosDemanda.AsNoTracking().Where(d => d.GrupoDeIntegrantes.Integrantes.Any(i => i.Usuario.Id == usuarioId)).ToList();
        }

        public PontoDemanda Obter(long usuarioId, long pontoDemandaId)
        {
            var pontoDemanda = _contexto.PontosDemanda.SingleOrDefault(d => d.GrupoDeIntegrantes.Integrantes.Any(i => i.Usuario.Id == usuarioId) && d.Id == pontoDemandaId);
            if(pontoDemanda == null) throw new ApplicationException("Ponto de demanda não encontrado.");
            return pontoDemanda;
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }

        public PontoDemanda Criar(long usuarioId, PontoDemanda novoPontoDemanda)
        {
            return new ComandoCriarPontoDemanda(usuarioId, novoPontoDemanda).Executar();
        }

        public Loja AdicionarLojaFavorita(long usuarioId, PontoDemanda pontoDemanda, Loja loja)
        {
            new ComandoAdicionarLojaFavorita(_contexto, pontoDemanda, loja).Executar();
            return loja;
        }
    }
}
