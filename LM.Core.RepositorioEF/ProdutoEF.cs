using System.Data.Entity;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class ProdutoEF : IRepositorioProduto
    {
        private readonly ContextoEF _contexto;
        public ProdutoEF()
        {
            _contexto = new ContextoEF();
        }

        public ProdutoEF(ContextoEF contexto)
        {
            _contexto = contexto;
        }

        public Produto Criar(Produto produto)
        {
            foreach (var categoria in produto.Categorias)
            {
                _contexto.Entry(categoria).State = EntityState.Unchanged;
            }
            produto = _contexto.Produtos.Add(produto);
            return produto;
        }

        public IEnumerable<Produto> ListarPorCategoria(long usuarioId, long pontoDemandaId, int categoriaId)
        {
            return _contexto.Produtos.AsNoTracking().Where(p => (p.UsuarioId == null || p.UsuarioId == usuarioId) && ((p.PontoDemandaId == null || p.PontoDemandaId == pontoDemandaId)) && p.Categorias.Any(c => c.Id == categoriaId) && p.Ativo);
        }

        public IEnumerable<Produto> Buscar(long usuarioId, long pontoDemandaId, string termo)
        {
            var searchFts = FtsInterceptor.Fts(termo);
            return _contexto.Produtos.AsNoTracking().Where(p => (p.UsuarioId == null || p.UsuarioId == usuarioId) && ((p.PontoDemandaId == null || p.PontoDemandaId == pontoDemandaId)) && p.Ean.Contains(searchFts) || p.Info.Nome.Contains(searchFts) || p.Info.Marca.Contains(searchFts) && p.Ativo);
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
