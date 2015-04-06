using System;
using System.Collections.Generic;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface ICompraAplicacao
    {
        Compra Criar(Compra compra);
    }

    public class CompraAplicacao : ICompraAplicacao
    {
        private readonly IRepositorioCompra _compraRepo;
        private readonly IListaAplicacao _appLista;
        public CompraAplicacao(IRepositorioCompra compraRepo, IListaAplicacao appLista)
        {
            _compraRepo = compraRepo;
            _appLista = appLista;
        }

        public Compra Criar(Compra compra)
        {
            if(compra.Itens == null || !compra.Itens.Any()) throw new ApplicationException("A compra deve possuir itens.");
            ChecarItensNovos(compra);
            return _compraRepo.Criar(compra);
        }

        private void ChecarItensNovos(Compra compra)
        {
            var compraItensNovos = compra.Itens.OfType<ListaCompraItem>().Where(i => i.Item.Id == 0).ToList();
            foreach (var compraItemNovo in compraItensNovos)
            {
                compraItemNovo.Item.Periodo = new Periodo {Id = 12 /* Eventual */};
                compraItemNovo.Item.QuantidadeDeConsumo = compraItemNovo.Quantidade;
                compraItemNovo.Item.QuantidadeEmEstoque = compraItemNovo.Quantidade;
                compraItemNovo.Item.Status = "A";
                _appLista.AdicionarItem(compra.PontoDemanda.Id, compraItemNovo.Item);
                
                //compra.Itens.Add(new ListaCompraItem
                //{
                //    Quantidade = compraItemNovo.Quantidade,
                //    Valor = compraItemNovo.Valor,
                //    Status = compraItemNovo.Status,
                //    ItemSubstituto = compraItemNovo.ItemSubstituto,
                //    Item = listaItem
                //});
            }
        }
    }
}
