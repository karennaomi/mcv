using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class IntegranteTests
    {
        [Test]
        public void IntegranteMaiorDe13AnosComEmailPodeSerConvidado()
        {
            var integrante = Fakes.Integrante(18, "M", 1);
            Assert.IsTrue(integrante.PodeSerConvidado());
        }

        [Test]
        public void IntegranteCom13AnosComEmailPodeSerConvidado()
        {
            var integrante = Fakes.Integrante(13, "M", 1);
            Assert.IsTrue(integrante.PodeSerConvidado());
        }

        [Test]
        public void IntegranteComMenosDe13AnosComEmailNaoPodeSerConvidado()
        {
            var integrante = Fakes.Integrante(12, "M", 1);
            Assert.IsFalse(integrante.PodeSerConvidado());
        }

        [Test]
        public void IntegranteSemEmailNaoPodeSerConvidado()
        {
            var integrante = Fakes.Integrante(18, "M", 1);
            integrante.Email = null;
            Assert.IsFalse(integrante.PodeSerConvidado());
        }
    }
}
