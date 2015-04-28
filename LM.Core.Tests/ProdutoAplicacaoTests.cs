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
            var produtos = app.Buscar("arroz");
            Assert.IsTrue(produtos.Any());
        }
    }
}
