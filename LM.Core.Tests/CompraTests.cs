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
            var compra = _fakes.CompraNotSoFake();
            var item = compra.Itens.OfType<ListaCompraItem>().First();
            compra.Itens.Add(new ListaCompraItem { Item = new ListaItem { Id = item.Id }, ProdutoId = item.ProdutoId, Quantidade = 1, Valor = 1.5M, Status = StatusCompra.NaoEncontrado });
            var ex = Assert.Throws<ApplicationException>(compra.Validar);
            Assert.AreEqual("Existem itens duplicados na sua compra.", ex.Message);
        }

        [Test]
        public void CompraDevePossuirItens()
        {
            var compra = _fakes.CompraNotSoFake();
            compra.Itens = null;
            var ex = Assert.Throws<ApplicationException>(compra.Validar);
            Assert.AreEqual("A compra deve possuir itens.", ex.Message);
        }

        [Test]
        public void CompraDevePossuirPontoDemanda()
        {
            var compra = _fakes.CompraNotSoFake();
            compra.PontoDemanda = null;
            var ex = Assert.Throws<ApplicationException>(compra.Validar);
            Assert.AreEqual("A compra deve possuir ponto de demanda.", ex.Message);
        }

        [Test]
        public void CompraDevePossuirIntegrante()
        {
            var compra = _fakes.CompraNotSoFake();
            compra.Integrante = null;
            var ex = Assert.Throws<ApplicationException>(compra.Validar);
            Assert.AreEqual("A compra deve possuir integrante.", ex.Message);
        }

        [Test]
        public void CompraDevePossuirUsuario()
        {
            var compra = _fakes.CompraNotSoFake();
            compra.Integrante.Usuario = null;
            var ex = Assert.Throws<ApplicationException>(compra.Validar);
            Assert.AreEqual("O integrante da compra deve possuir um usuário.", ex.Message);
        }
    }
}
