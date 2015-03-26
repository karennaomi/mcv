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

        [Test]
        public void CriarIntegranteParaUsuario18AnosSexoMasculinoDefinePersonaCorreta()
        {
            IntegrantePersonaTestes(18, "M", 18, 27);
        }

        [Test]
        public void CriarIntegranteParaUsuario18AnosSexoFemininoDefinePersonaCorreta()
        {
            IntegrantePersonaTestes(18, "F", 18, 27);
        }

        [Test]
        public void CriarIntegranteParaUsuario27AnosSexoMasculinoDefinePersonaCorreta()
        {
            IntegrantePersonaTestes(27, "M", 18, 27);
        }

        [Test]
        public void CriarIntegranteParaUsuario27AnosSexoFemininoDefinePersonaCorreta()
        {
            IntegrantePersonaTestes(27, "F", 18, 27);
        }

        [Test]
        public void CriarIntegranteParaUsuario23AnosSexoMasculinoDefinePersonaCorreta()
        {
            IntegrantePersonaTestes(23, "M", 18, 27);
        }

        [Test]
        public void CriarIntegranteParaUsuarioIdadeNegativaLancaException()
        {
            Assert.Throws<ApplicationException>(() => IntegrantePersonaTestes(-1, "F", 18, 27));
        }

        [Test]
        public void CriarIntegranteParaUsuarioIdadeMaior150AnosLancaException()
        {
            Assert.Throws<ApplicationException>(() => IntegrantePersonaTestes(151, "F", 18, 27));
        }

        [Test]
        public void CriarIntegranteParaUsuario23AnosSexoFemininoDefinePersonaCorreta()
        {
            IntegrantePersonaTestes(23, "F", 18, 27);
        }

        private static void IntegrantePersonaTestes(int idade, string sexo, int idadeInicial, int idadeFinal)
        {
            var integrante = Fakes.Integrante(idade, sexo);
            var app = ObterAppIntegrante(ObterIntegranteRepo().Object);
            integrante = app.Criar(integrante);
            Assert.AreEqual(sexo, integrante.Persona.Sexo);
            Assert.AreEqual(idadeInicial, integrante.Persona.IdadeInicial);
            Assert.AreEqual(idadeFinal, integrante.Persona.IdadeFinal);
        }

        private static IIntegranteAplicacao ObterAppIntegrante(IRepositorioIntegrante repo)
        {
            return new IntegranteAplicacao(repo, ObterAppPersona(), ObterAppPontoDemanda());
        }

        private static IPontoDemandaAplicacao ObterAppPontoDemanda()
        {
            var appMock = new Mock<IPontoDemandaAplicacao>();
            appMock.Setup(d => d.Obter(It.IsAny<long>())).Returns(Fakes.PontoDemanda);
            return appMock.Object;
        }

        private static IUsuarioAplicacao ObterAppUsuario(Usuario usuario)
        {
            var repo = new Mock<IRepositorioUsuario>();
            repo.Setup(r => r.Obter(It.IsAny<long>())).Returns(usuario);
            return new UsuarioAplicacao(repo.Object, new Mock<IIntegranteAplicacao>().Object);
        }

        private static IPersonaAplicacao ObterAppPersona()
        {
            return new PersonaAplicacao(new PersonaEF());
        }

        private static Mock<IRepositorioIntegrante> ObterIntegranteRepo()
        {
            var repoMock = new Mock<IRepositorioIntegrante>();
            repoMock.Setup(r => r.Criar(It.IsAny<Integrante>())).Returns<Integrante>(x => x);
            return repoMock;
        }
    }
}
