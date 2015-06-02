using System.Linq;
using LM.Core.Domain;
using System;
using System.Data.Entity;

namespace LM.Core.RepositorioEF
{
    public class ComandoCriarItemNaLista
    {
        private readonly ContextoEF _contexto;
        private readonly Lista _lista;
        private readonly ListaItem _novoItem;

        public ComandoCriarItemNaLista(ContextoEF contexto, Lista lista, ListaItem novoItem)
        {
            _contexto = contexto;
            _lista = lista;
            _novoItem = novoItem;
        }

        public ListaItem Executar()
        {
            _novoItem.Produto = ChecarProduto(_novoItem.Produto);
            _novoItem.Periodo = _contexto.Set<Periodo>().Single(p => p.Id == _novoItem.Periodo.Id);
            _novoItem.DataInclusao = DateTime.Now;
            _novoItem.DataAlteracao = DateTime.Now;
            _lista.Itens.Add(_novoItem);
            _contexto.SaveChanges();
            return _novoItem;
        }

        private Produto ChecarProduto(Produto produto)
        {
            if (produto.Id == 0)
            {
                var produtoRepo = new ProdutoEF(_contexto);
                return produtoRepo.Criar(produto);
            }
            else
            {
                var produtoLocal = _contexto.Produtos.Local.SingleOrDefault(p => p.Id == produto.Id);
                if (produtoLocal != null) return produtoLocal;
                var produtoToAdd = _contexto.Produtos.Create();
                produtoToAdd.Id = produto.Id;
                _contexto.Entry(produtoToAdd).State = EntityState.Unchanged;
                return produtoToAdd;
            }
        }
    }
}
