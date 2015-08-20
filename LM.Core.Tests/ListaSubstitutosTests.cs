using LM.Core.Application;
using LM.Core.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.Tests
{
    [TestFixture]
    public class ListaSubstitutosTests
    {
        private readonly Fakes _fakes;
        private readonly MockCompraRepo _mockCompraRepos;
        private const long PontoDemandaId = 1;

        public ListaSubstitutosTests()
        {
            _fakes = new Fakes();
            
            var compras = new List<Compra>();
            var compra1 = GetCompra(PontoDemandaId);
            compra1.AdicionarItemSubstituto(GetCompraItem(1, 1, compra1), GetCompraItem(2, 2, compra1), new MotivoSubstituicao{Id = 1});
            compras.Add(compra1);
            var compra2 = GetCompra(PontoDemandaId);
            compra2.AdicionarItemSubstituto(GetCompraItem(1, 1, compra2), GetCompraItem(3, 3, compra2), new MotivoSubstituicao { Id = 1 });
            compra2.AdicionarItemSubstituto(GetCompraItem(4, 4, compra2), GetCompraItem(5, 5, compra2), new MotivoSubstituicao { Id = 1 });
            compras.Add(compra2);
            var compra3 = GetCompra(PontoDemandaId);
            compra3.AdicionarItemSubstituto(GetCompraItem(1, 1, compra3), GetCompraItem(6, 6, compra3), new MotivoSubstituicao { Id = 1 });
            compra3.AdicionarItemSubstituto(GetCompraItem(4, 4, compra3), GetCompraItem(5, 5, compra3), new MotivoSubstituicao { Id = 1 });
            compra3.AdicionarItemSubstituto(GetCompraItem(7, 7, compra3), GetCompraItem(8, 8, compra3), new MotivoSubstituicao { Id = 1 });
            compras.Add(compra3);
            var compra4 = GetCompra(2);
            compra4.AdicionarItemSubstituto(GetCompraItem(9, 9, compra4), GetCompraItem(8, 8, compra4), new MotivoSubstituicao { Id = 1 });
            compra4.AdicionarItemSubstituto(GetCompraItem(11, 11, compra4), GetCompraItem(7, 7, compra4), new MotivoSubstituicao { Id = 1 });
            compras.Add(compra4);

            _mockCompraRepos = new MockCompraRepo {Compras = compras};
        }

        private Compra GetCompra(long pontoDemandaId)
        {
            var compra = _fakes.Compra();
            var pontoDemanda = _fakes.PontoDemanda();
            pontoDemanda.Id = pontoDemandaId;
            compra.PontoDemanda = pontoDemanda;
            return compra;
        }

        private ListaCompraItem GetCompraItem(int produtoId, long itemId, Compra compra)
        {
            var produto = _fakes.Produto();
            produto.Id = produtoId;
            var item = _fakes.ListaItem();
            item.Id = itemId;
            return new ListaCompraItem {Id = 1, Item = item, ProdutoId = produtoId, Compra = compra};
        }

        [Test]
        public void ListaSubstitutosCaso1()
        {
            var compraApp = new CompraAplicacao(_mockCompraRepos.GetMockedRepo(), null, null, null);
            var itensSubstitutos = compraApp.ListarItensSubstitutos(PontoDemandaId, 1L);
            Assert.AreEqual(3, itensSubstitutos.Count());
        }

        [Test]
        public void ListaSubstitutosCaso2()
        {
            var compraApp = new CompraAplicacao(_mockCompraRepos.GetMockedRepo(), null, null, null);
            var itensSubstitutos = compraApp.ListarItensSubstitutos(PontoDemandaId, 4L);
            Assert.AreEqual(2, itensSubstitutos.Count());
        }

        [Test]
        public void ListaSubstitutosCaso3()
        {
            var compraApp = new CompraAplicacao(_mockCompraRepos.GetMockedRepo(), null, null, null);
            var itensSubstitutos = compraApp.ListarItensSubstitutos(PontoDemandaId, 7L);
            Assert.AreEqual(1, itensSubstitutos.Count());
        }
    }
}
