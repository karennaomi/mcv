using System.Collections;
using System.Collections.Generic;
using LM.Core.Application;
using LM.Core.Domain.Repositorio;
using LM.Core.RepositorioEF;
using NUnit.Framework;
using System.Linq;
using LM.Core.Domain;

namespace LM.Core.Tests
{
    [TestFixture]
    public class ProdutoAplicacaoTests
    {
        private Fakes _fakes;
        private MockProdutoRepo _mockRepo;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
            _mockRepo = new MockProdutoRepo();
        }

        [Test]
        public void BuscaUmProduto()
        {
            var app = ObterAppProduto(new ProdutoEF());
            var produtos = app.Buscar(1, 100, "arroz");
            Assert.IsTrue(produtos.Any());
        }

        [Test]
        public void BuscaUmProdutoPorParteDoNome()
        {
            var app = ObterAppProduto(new ProdutoEF());
            var produtos = app.Buscar(1, 100, "arro");
            Assert.IsTrue(produtos.Any());
        }

        [Test]
        public void BuscaUmProdutoComEspaco()
        {
            var app = ObterAppProduto(new ProdutoEF());
            var produtos = app.Buscar(1, 100, "carne de ra");
            Assert.IsTrue(produtos.Any());
        }

        [Test]
        public void BuscaUmProdutoPorEan()
        {
            var app = ObterAppProduto(new ProdutoEF());
            var produtos = app.Buscar(1, 100, "3700123302360");
            Assert.IsTrue(produtos.Any());
        }

        [Test]
        public void ListarProdutosQueNaoPertencemAoUsuarioOuPontoDemandaSempreVoltaResultado()
        {
            var produto1 = _fakes.Produto();
            var produto2 = _fakes.Produto();
            var produto3 = _fakes.Produto();
            _mockRepo.Produtos = new List<Produto> { produto1, produto2, produto3 };
            Assert.AreEqual(3, ObterAppProduto(_mockRepo.GetMockedRepo()).ListarPorCategoria(1, 100, 3).Count());
        }

        [Test]
        public void ListarProdutosQuePertencemAoUsuarioVoltaResultadoCorreto()
        {
            var produto1 = _fakes.Produto();
            produto1.UsuarioId = 1;
            produto1.PontoDemandaId = 100;
            var produto2 = _fakes.Produto();
            produto2.UsuarioId = 2;
            produto2.PontoDemandaId = 101;
            var produto3 = _fakes.Produto();
            _mockRepo.Produtos = new List<Produto> { produto1, produto2, produto3 };
            Assert.AreEqual(2, ObterAppProduto(_mockRepo.GetMockedRepo()).ListarPorCategoria(1, 100, 2).Count());
        }

        [Test]
        public void ListarProdutosQuePertencemAoPontoDemandaVoltaResultadoCorreto()
        {
            var produto1 = _fakes.Produto();
            produto1.UsuarioId = 1;
            produto1.PontoDemandaId = 100;
            var produto2 = _fakes.Produto();
            produto2.UsuarioId = 2;
            produto2.PontoDemandaId = 101;
            var produto3 = _fakes.Produto();
            _mockRepo.Produtos = new List<Produto> { produto1, produto2, produto3 };
            Assert.AreEqual(2, ObterAppProduto(_mockRepo.GetMockedRepo()).ListarPorCategoria(3, 100, 2).Count());
        }

        private static IProdutoAplicacao ObterAppProduto(IRepositorioProduto repo)
        {
            return new ProdutoAplicacao(repo);
        }
    }
}
