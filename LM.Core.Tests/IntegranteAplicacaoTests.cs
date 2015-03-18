using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Repository;
using Moq;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class IntegranteAplicacaoTests
    {
        [Test]
        public void NaoPodeApagarUmIntegranteQueNaoPertenceAoPonteDemandaEspecificado()
        {
            var app = new IntegranteAplicacao(ObterIntegranteRepo().Object, ObterAppPontoDemanda());
            Assert.Throws<IntegranteNaoPertenceAPontoDemandaException>(() => app.Apagar(9999, 123));
        }

        [Test]
        public void PodeApagarUmIntegranteQuePertenceAoPonteDemandaEspecificado()
        {
            var repoMock = ObterIntegranteRepo();
            var app = new IntegranteAplicacao(repoMock.Object, ObterAppPontoDemanda());
            app.Apagar(1234, 123);
            repoMock.Verify(r => r.Apagar(1234), Times.Once);
        }

        private static IPontoDemandaAplicacao ObterAppPontoDemanda()
        {
            var appMock = new Mock<IPontoDemandaAplicacao>();
            appMock.Setup(d => d.Obter(It.IsAny<long>())).Returns(Fakes.PontoDemanda);
            return appMock.Object;
        }

        private static Mock<IRepositorioIntegrante> ObterIntegranteRepo()
        {
            var repoMock = new Mock<IRepositorioIntegrante>();
            return repoMock;
        }
    }
}
