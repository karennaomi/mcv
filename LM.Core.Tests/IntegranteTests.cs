using System;
using LM.Core.Domain;
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

        [Test]
        public void IntegranteSemDataDeConvitePodeSerConvidado()
        {
            var integrante = Fakes.Integrante(18, "M", 1);
            integrante.DataConvite = null;
            Assert.IsTrue(integrante.PodeSerConvidado());
        }

        [Test]
        public void IntegranteComDataDeConviteMenorQue1DiaNaoPodeSerConvidado()
        {
            var integrante = Fakes.Integrante(18, "M", 1);
            integrante.DataConvite = DateTime.Now.AddMinutes(-60);
            Assert.IsFalse(integrante.PodeSerConvidado());
        }

        [Test]
        public void IntegranteComDataDeConviteMaiorQue1DiaPodeSerConvidado()
        {
            var integrante = Fakes.Integrante(18, "M", 1);
            integrante.DataConvite = DateTime.Now.AddHours(-25);
            Assert.IsTrue(integrante.PodeSerConvidado());
        }

        [Test]
        public void IntegranteComUsuarioNaoPodeSerConvidado()
        {
            var integrante = Fakes.Integrante(18, "M", 1);
            integrante.Usuario = new Usuario {Id = 1};
            Assert.IsFalse(integrante.PodeSerConvidado());
        }
    }
}
