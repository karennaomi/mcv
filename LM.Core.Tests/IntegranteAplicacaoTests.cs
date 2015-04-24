using System.Transactions;
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
        public void CriarIntegrante()
        {
            using (new TransactionScope())
            {
                var integrante = Fakes.Integrante(15, "M", 1);
                var app = new IntegranteAplicacao(new IntegranteEF(), null, new PersonaAplicacao(new PersonaEF()));
                integrante = app.Criar(integrante);
                Assert.IsTrue(integrante.Id > 0);
            }
        }

        [Test]
        public void NaoPodeApagarUmIntegranteQueNaoPertenceAoPonteDemandaEspecificado()
        {
            var app = ObterAppIntegrante(ObterIntegranteRepo().Object);
            Assert.Throws<IntegranteNaoPertenceAPontoDemandaException>(() => app.Apagar(9999, 9999, 123));
        }

        [Test]
        public void PodeApagarUmIntegranteQuePertenceAoPonteDemandaEspecificado()
        {
            var repoMock = ObterIntegranteRepo();
            var app = ObterAppIntegrante(repoMock.Object);
            app.Apagar(9999, 1234, 1234);
            repoMock.Verify(r => r.Apagar(1234), Times.Once);
        }

        private static IIntegranteAplicacao ObterAppIntegrante(IRepositorioIntegrante repo)
        {
            return new IntegranteAplicacao(repo, ObterAppPontoDemanda(), new PersonaAplicacao(new PersonaEF()));
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
            appMock.Setup(d => d.Obter(It.IsAny<long>(), It.IsAny<long>())).Returns(Fakes.PontoDemanda);
            return appMock.Object;
        }
    }
}
