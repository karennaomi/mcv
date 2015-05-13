using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using LM.Core.RepositorioEF;
using NUnit.Framework;
using System.Linq;
using System.Transactions;

namespace LM.Core.Tests
{
    [TestFixture]
    public class CompraAplicacaoTests
    {
        private Fakes _fakes;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
        }

        [Test]
        public void CriarCompra()
        {
            using (new TransactionScope())
            {
                var compra = _fakes.CompraNotSoFake();
                var app = ObterAppCompra(new CompraEF());
                compra = app.Criar(compra);
                Assert.IsTrue(compra.Id > 0);
            }
        }

        [Test]
        public void CriarCompraComItemSubstituto()
        {
            using (new TransactionScope())
            {
                var compra = _fakes.CompraNotSoFake();

                var listaItemIds = new[] { new[] { 2, 27397 }, new[] { 3, 27399 } };
                var compraItemOriginal = new ListaCompraItem { Item = new ListaItem { Id = listaItemIds[0][0] }, ProdutoId = listaItemIds[0][1], Quantidade = 2, Valor = 2.5M };
                var compraItemSubstituto = new ListaCompraItem { Item = new ListaItem { Id = listaItemIds[1][0] }, ProdutoId = listaItemIds[1][1], Quantidade = 1, Valor = 1, };

                compra.AdicionarItemSubstituto(compraItemOriginal, compraItemSubstituto, "teste");
                var app = ObterAppCompra(new CompraEF());
                compra = app.Criar(compra);
                Assert.IsTrue(compra.Id > 0);
                Assert.AreEqual(5, compra.Itens.Count);
                Assert.IsTrue(compra.Itens.Any(i => i.ItemSubstituto != null));
                Assert.AreEqual(StatusCompra.ItemSubstituido, compraItemOriginal.Status);
                Assert.AreEqual(StatusCompra.Comprado, compraItemSubstituto.Status);
            }
        }

        [Test]
        public void CriarCompraComProdutoNovo()
        {
            using (new TransactionScope())
            {
                var compra = _fakes.CompraNotSoFake();
                compra.Itens.Add(_fakes.ListaCompraItem());

                var app = ObterAppCompra(new CompraEF());
                compra = app.Criar(compra);
                
                Assert.IsTrue(compra.Id > 0);
                Assert.IsTrue(compra.Itens.OfType<ListaCompraItem>().Any(i => i.Item.Periodo.Id == 12));
                Assert.IsTrue(compra.Itens.OfType<ListaCompraItem>().Count() == 2);
            }
        }

        private static CompraAplicacao ObterAppCompra(IRepositorioCompra compraRepo)
        {
            return new CompraAplicacao(compraRepo);
        }
    }
}
