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
            _contexto.Entry(_novoItem.Periodo).State = EntityState.Unchanged;
            _novoItem.DataInclusao = DateTime.Now;
            _novoItem.DataAlteracao = DateTime.Now;
            _novoItem.Status = "I";
            _lista.Itens.Add(_novoItem);
            _contexto.SaveChanges();
            return _novoItem;
        }

        private Produto ChecarProduto(Produto produto)
        {
            if (produto.Id == 0)
            {
                var produtoRepo = new ProdutoEF(_contexto);
                produto = produtoRepo.Criar(produto);
            }
            else
            {
                _contexto.Entry(produto).State = EntityState.Unchanged;
            }
            return produto;
        }
    }
}
