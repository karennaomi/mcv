using System.Collections.ObjectModel;
using LM.Core.Domain;
using NUnit.Framework;
using System;
using System.Linq;

namespace LM.Core.Tests
{
    [TestFixture]
    class CompraTests
    {
        private Fakes _fakes;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
        }

        [Test]
        public void NaoPodeCriarCompraComItemDuplicado()
        {
            var compra = GetCompra();
            compra.Itens.Add(new ListaCompraItem {ProdutoId = 666});
            var ex = Assert.Throws<ApplicationException>(compra.Validar);
            Assert.AreEqual("Existem itens duplicados na sua compra.", ex.Message);
        }

        [Test]
        public void PodeCriarCompraComItemDuplicadoDesdeQueUmSejaListaItemEOutroPedidoItem()
        {
            var compra = GetCompra();
            compra.Itens.Add(new PedidoCompraItem {ProdutoId = 666});
            Assert.DoesNotThrow(compra.Validar);
        }

        [Test]
        public void CompraDevePossuirItens()
        {
            var compra = GetCompra();
            compra.Itens = new Collection<CompraItem>();
            var ex = Assert.Throws<ApplicationException>(compra.Validar);
            Assert.AreEqual("A compra deve possuir itens.", ex.Message);
        }

        [Test]
        public void CompraDevePossuirPontoDemanda()
        {
            var compra = GetCompra();
            compra.PontoDemanda = null;
            var ex = Assert.Throws<ApplicationException>(compra.Validar);
            Assert.AreEqual("A compra deve possuir ponto de demanda.", ex.Message);
        }

        [Test]
        public void CompraDevePossuirIntegrante()
        {
            var compra = GetCompra();
            compra.Integrante = null;
            var ex = Assert.Throws<ApplicationException>(compra.Validar);
            Assert.AreEqual("A compra deve possuir integrante.", ex.Message);
        }

        [Test]
        public void CompraDevePossuirUsuario()
        {
            var compra = GetCompra();
            compra.Integrante.Usuario = null;
            var ex = Assert.Throws<ApplicationException>(compra.Validar);
            Assert.AreEqual("O integrante da compra deve possuir um usuário.", ex.Message);
        }

        private Compra GetCompra()
        {
            var compra = _fakes.Compra();
            compra.PontoDemanda = _fakes.PontoDemanda();
            compra.PontoDemanda.Listas = new Collection<Lista> { new Lista { PontoDemanda = compra.PontoDemanda } };
            compra.Integrante = _fakes.Integrante();
            compra.Integrante.Id = 1;
            compra.Integrante.Usuario = _fakes.Usuario();
            compra.Integrante.Usuario.Id = 1;
            compra.Itens = new Collection<CompraItem> {new ListaCompraItem {ProdutoId = 666, Item = new ListaItem {Id = 20, Produto = new Produto{ Id = 666 }}}};
            return compra;
        }
    }
}
