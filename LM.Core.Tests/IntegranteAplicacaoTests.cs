using System;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using LM.Core.RepositorioEF;
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
            var app = ObterAppIntegrante(ObterIntegranteRepo().Object);
            Assert.Throws<IntegranteNaoPertenceAPontoDemandaException>(() => app.Apagar(9999, 123));
        }

        [Test]
        public void PodeApagarUmIntegranteQuePertenceAoPonteDemandaEspecificado()
        {
            var repoMock = ObterIntegranteRepo();
            var app = ObterAppIntegrante(repoMock.Object);
            app.Apagar(1234, 123);
            repoMock.Verify(r => r.Apagar(1234), Times.Once);
        }

        private static IIntegranteAplicacao ObterAppIntegrante(IRepositorioIntegrante repo)
        {
            return new IntegranteAplicacao(repo, ObterAppPontoDemanda());
        }

        private static Mock<IRepositorioIntegrante> ObterIntegranteRepo()
        {
            var repoMock = new Mock<IRepositorioIntegrante>();
            repoMock.Setup(r => r.Criar(It.IsAny<Integrante>())).Returns<Integrante>(x => x);
            return repoMock;
        }

        private static IPontoDemandaAplicacao ObterAppPontoDemanda()
        {
            var appMock = new Mock<IPontoDemandaAplicacao>();
            appMock.Setup(d => d.Obter(It.IsAny<long>())).Returns(Fakes.PontoDemanda);
            return appMock.Object;
        }
    }
}
