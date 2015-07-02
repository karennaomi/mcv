using System.Collections.ObjectModel;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class ProdutoEF : IRepositorioProduto
    {
        private readonly ContextoEF _contexto;
        private readonly FilaItemEF _filaRepo;
        public ProdutoEF()
        {
            _contexto = new ContextoEF();
            _filaRepo = new FilaItemEF(_contexto);
        }

        public ProdutoEF(ContextoEF contexto)
        {
            _contexto = contexto;
            _filaRepo = new FilaItemEF(contexto);
        }

        public Produto Criar(Produto produto, long usuarioId)
        {
            RelacionaCategorias(produto);
            RelacionaPontosDemanda(produto, usuarioId);
            CriarFilaProcessamentoProduto(produto);
            produto = _contexto.Produtos.Add(produto);
            return produto;
        }

        private void RelacionaCategorias(Produto produto)
        {
            var categorias = new Categoria[produto.Categorias.Count];
            produto.Categorias.CopyTo(categorias, 0);
            produto.Categorias.Clear();
            foreach (var categoria in categorias)
            {
                produto.Categorias.Add(_contexto.Categorias.Single(c => c.Id == categoria.Id));
            }
        }

        private void RelacionaPontosDemanda(Produto produto, long usuarioId)
        {
            var pontosDemanda = _contexto.PontosDemanda.Where(p => p.UsuarioCriador.Id == usuarioId);
            if (produto.PontosDemanda == null) produto.PontosDemanda = new Collection<PontoDemanda>();
            foreach (var pontoDemanda in pontosDemanda)
            {
                produto.PontosDemanda.Add(pontoDemanda);
            }
        }

        private void CriarFilaProcessamentoProduto(Produto produto)
        {
            _filaRepo.Criar(new FilaItemProduto
            {
                FilaProdutos = new Collection<FilaProduto> { new FilaProduto
                {
                    ProdutoId = produto.Id,
                    Ean = produto.Ean,
                    Descricao = produto.Nome(),
                    Imagem = produto.ImagemPrincipal().Path
                } }
            });
        }

        public IEnumerable<Produto> ListarPorCategoria(int categoriaId)
        {
            return _contexto.Produtos.AsNoTracking().Where(p => p.Categorias.Any(c => c.Id == categoriaId) && p.Ativo);
        }
        
        public IEnumerable<Produto> Buscar(string termo)
        {
            var searchFts = FtsInterceptor.Fts(termo);
            return _contexto.Produtos.AsNoTracking().Where(p => p.Ean.Contains(searchFts) || p.Info.Nome.Contains(searchFts) || p.Info.Marca.Contains(searchFts) && p.Ativo);
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
