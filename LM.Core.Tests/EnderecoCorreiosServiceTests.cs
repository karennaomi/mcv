using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.RepositorioEF;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class EnderecoCorreiosServiceTests
    {
        [Test]
        public void CepNaoEncontradoLancaExcecao()
        {
            var app = new EnderecoCorreiosService(new CorreiosEF());
            Assert.Throws<ObjetoNaoEncontradoException>(() => app.BuscarPorCep("00000000"));
        }

        [Test]
        public void CepValidoRetornaEndereco()
        {
            var app = new EnderecoCorreiosService(new CorreiosEF());
            Assert.IsInstanceOf<Endereco>(app.BuscarPorCep("14090-495"));
        }
    }
}
