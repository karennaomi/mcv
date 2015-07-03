using LM.Core.Domain;
using LM.Core.Domain.CustomException;
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
            return _contexto.PontosDemanda.AsNoTracking().Where(p => p.GruposDeIntegrantes.Any(g => g.Integrante.Usuario.Id == usuarioId) && p.Ativo).ToList();
        }

        public PontoDemanda Obter(long usuarioId, long pontoDemandaId)
        {
            var pontoDemanda = _contexto.PontosDemanda.SingleOrDefault(p => p.GruposDeIntegrantes.Any(g => g.Integrante.Usuario.Id == usuarioId) && p.Id == pontoDemandaId && p.Ativo);
            if (pontoDemanda == null) throw new ObjetoNaoEncontradoException("Ponto de demanda não encontrado.");
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

        public PontoDemanda Atualizar(long usuarioId, PontoDemanda pontoDemanda)
        {
            var pontoDemandaToUpdate = Obter(usuarioId, pontoDemanda.Id);
            return new ComandoAtualizarPontoDemanda(_contexto, pontoDemandaToUpdate, pontoDemanda).Executar();
        }

        public Loja AdicionarLojaFavorita(long usuarioId, PontoDemanda pontoDemanda, Loja loja)
        {
            loja = new ComandoAdicionarLojaFavorita(_contexto, pontoDemanda, loja).Executar();
            return loja;
        }
    }
}
