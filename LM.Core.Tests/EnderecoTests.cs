using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class EnderecoTests
    {
        private Fakes _fakes;
        [TestFixtureSetUp]
        public void Initialize()
        {
            _fakes = new Fakes();
        }

        [Test]
        public void MontaEnderecoCompletoComTodasAsInformacoes()
        {
            Assert.AreEqual("Rua dos bobos, 0 - Vl Olimpia São Paulo - SP", _fakes.Endereco().ToString());
        }

        [Test]
        public void MontaEnderecoCompletoSemLogradouroESemNumero()
        {
            var endereco = _fakes.Endereco();
            endereco.Logradouro = null;
            Assert.AreEqual("Vl Olimpia São Paulo - SP", endereco.ToString());
        }

        [Test]
        public void MontaEnderecoCompletoSemLogradouroESemNumeroESemBairro()
        {
            var endereco = _fakes.Endereco();
            endereco.Logradouro = null;
            endereco.Bairro = null;
            Assert.AreEqual("São Paulo - SP", endereco.ToString());
        }

        [Test]
        public void MontaEnderecoCompletoSemLogradouroESemNumeroESemBairroESemCidade()
        {
            var endereco = _fakes.Endereco();
            endereco.Logradouro = null;
            endereco.Bairro = null;
            endereco.Cidade = null;
            Assert.AreEqual("SP", endereco.ToString());
        }

        [Test]
        public void MontaEnderecoCompletoSemLogradouroESemNumeroESemBairroESemCidadeESemEstado()
        {
            var endereco = _fakes.Endereco();
            endereco.Logradouro = null;
            endereco.Bairro = null;
            endereco.Cidade = null;
            endereco.Uf = null;
            Assert.AreEqual("", endereco.ToString());
        }
    }
}
