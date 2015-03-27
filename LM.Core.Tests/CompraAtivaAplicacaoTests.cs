using System;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using LM.Core.RepositorioEF;
using Moq;
using NUnit.Framework;
using System.Transactions;

namespace LM.Core.Tests
{
    [TestFixture]
    public class CompraAtivaAplicacaoTests
    {
        [Test]
        public void CriaUmaCompraAtiva()
        {
            using (new TransactionScope())
            {
                var compraAtiva = GetApp().AtivarCompra();
                Assert.IsTrue(compraAtiva.Id > 0);
            }
        }

        [Test]
        public void FinalizaUmaCompraAtiva()
        {
            using (new TransactionScope())
            {
                GetApp().AtivarCompra();
                var compraAtiva = GetApp().FinalizarCompra();
                Assert.IsNotNull(compraAtiva.FimCompra);
            }
        }

        [Test]
        public void LancaExcecaoQuandoNaoExisteCompraAtivaETentaFinalizar()
        {
            Assert.Throws<ApplicationException>(() => GetMockApp().FinalizarCompra());
        }

        private static ICompraAtivaAplicacao GetApp()
        {
            return new CompraAtivaAplicacao(new CompraAtivaEF(), 10124, 20114);
        }

        private static ICompraAtivaAplicacao GetMockApp()
        {
            var mockRepo = new Mock<IRepositorioCompraAtiva>();
            return new CompraAtivaAplicacao(mockRepo.Object, 10124, 20114);
        }
    }
}
