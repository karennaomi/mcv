using NUnit.Framework;
using System;

namespace LM.Core.Tests
{
    [TestFixture]
    public class CompraAtivaTests
    {
        private Fakes _fakes;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
        }

        [Test]
        public void CompraAtivaSemDataFinalIndicaCompraNaoFinalizada()
        {
            var compraAtiva = _fakes.CompraAtiva();
            compraAtiva.FimCompra = null;
            Assert.IsFalse(compraAtiva.CompraFinalizada());
        }

        [Test]
        public void CompraAtivaComDataFinalIndicaCompraFinalizada()
        {
            var compraAtiva = _fakes.CompraAtiva();
            compraAtiva.FimCompra = DateTime.Now;
            Assert.IsTrue(compraAtiva.CompraFinalizada());
        }
    }
}
