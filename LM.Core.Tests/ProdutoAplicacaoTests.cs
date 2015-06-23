using LM.Core.Application;
using LM.Core.RepositorioEF;
using NUnit.Framework;
using System.Linq;

namespace LM.Core.Tests
{
    [TestFixture]
    public class ProdutoAplicacaoTests
    {
        [Test]
        public void BuscaUmProduto()
        {
            var app = new ProdutoAplicacao(new ProdutoEF());
            var produtos = app.Buscar(1, "arroz");
            Assert.IsTrue(produtos.Any());
        }

        [Test]
        public void BuscaUmProdutoPorParteDoNome()
        {
            var app = new ProdutoAplicacao(new ProdutoEF());
            var produtos = app.Buscar(1, "arro");
            Assert.IsTrue(produtos.Any());
        }

        [Test]
        public void BuscaUmProdutoComEspaco()
        {
            var app = new ProdutoAplicacao(new ProdutoEF());
            var produtos = app.Buscar(1, "carne de ra");
            Assert.IsTrue(produtos.Any());
        }

        [Test]
        public void BuscaUmProdutoPorEan()
        {
            var app = new ProdutoAplicacao(new ProdutoEF());
            var produtos = app.Buscar(1, "3700123302360");
            Assert.IsTrue(produtos.Any());
        }
    }
}
