using System;
using LM.Core.Domain;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace LM.Core.Tests
{
    [TestFixture]
    public class IntegranteTests
    {
        private Fakes _fakes; 
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
        }

        [Test]
        public void IntegranteMaiorDe13AnosComEmailPodeSerConvidado()
        {
            var integrante = _fakes.Integrante();
            Assert.IsTrue(integrante.PodeSerConvidado());
        }

        [Test]
        public void IntegranteCom13AnosComEmailPodeSerConvidado()
        {
            var integrante = _fakes.Integrante();
            integrante.DataNascimento = DateTime.Now.AddYears(-13);
            Assert.IsTrue(integrante.PodeSerConvidado());
        }

        [Test]
        public void IntegranteComMenosDe13AnosComEmailNaoPodeSerConvidado()
        {
            var integrante = _fakes.Integrante();
            integrante.DataNascimento = DateTime.Now.AddYears(-12);
            Assert.IsFalse(integrante.PodeSerConvidado());
        }

        [Test]
        public void IntegranteSemEmailNaoPodeSerConvidado()
        {
            var integrante = _fakes.Integrante();
            integrante.Email = null;
            Assert.IsFalse(integrante.PodeSerConvidado());
        }

        [Test]
        public void IntegranteSemDataDeConvitePodeSerConvidado()
        {
            var integrante = _fakes.Integrante();
            Assert.IsTrue(integrante.PodeSerConvidado());
        }

        [Test]
        public void IntegranteComDataDeConviteMenorQue1DiaNaoPodeSerConvidado()
        {
            var integrante = _fakes.Integrante();
            integrante.DataConvite = DateTime.Now.AddHours(-6);
            Assert.IsFalse(integrante.PodeSerConvidado());
        }

        [Test]
        public void IntegranteComDataDeConviteMaiorQue1DiaPodeSerConvidado()
        {
            var integrante = _fakes.Integrante();
            integrante.DataConvite = DateTime.Now.AddHours(-25);
            Assert.IsTrue(integrante.PodeSerConvidado());
        }

        [Test]
        public void IntegranteComUsuarioNaoPodeSerConvidado()
        {
            var integrante = _fakes.Integrante();
            integrante.Usuario = new Usuario {Id = 1};
            Assert.IsFalse(integrante.PodeSerConvidado());
        }
    }
}
