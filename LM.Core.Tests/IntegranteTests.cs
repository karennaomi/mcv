using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public void ValidarNomeObrigatorio()
        {
            var integrante = _fakes.Integrante();
            integrante.Nome = null;
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(integrante, new ValidationContext(integrante), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("O nome é de preenchimento obrigatório!", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("Nome", error.MemberNames.ElementAt(0));
        }

        [Test]
        public void ValidarSexoObrigatorioEmNaoPet()
        {
            var integrante = _fakes.Integrante();
            integrante.Sexo = null;
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(integrante, new ValidationContext(integrante), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("O sexo é de preenchimento obrigatório!", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("Sexo", error.MemberNames.ElementAt(0));
        }

        [Test]
        public void ValidarSexoCharEmNaoPet()
        {
            var integrante = _fakes.Integrante();
            integrante.Sexo = "g";
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(integrante, new ValidationContext(integrante), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("O sexo selecionado é inválido: g", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("Sexo", error.MemberNames.ElementAt(0));
        }

        [Test]
        public void ValidarEmail()
        {
            var integrante = _fakes.Integrante();
            integrante.Email = "g@g";
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(integrante, new ValidationContext(integrante), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("O e-mail informado é inválido: g@g", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("Email", error.MemberNames.ElementAt(0));
        }

        [Test]
        public void IntegranteComUsuarioEhUsuarioDoSistema()
        {
            var integrante = _fakes.Integrante();
            integrante.Usuario = _fakes.Usuario();
            Assert.IsTrue(integrante.EhUsuarioDoSistema());
        }

        [Test]
        public void IntegranteSemsuarioNaoEhUsuarioDoSistema()
        {
            var integrante = _fakes.Integrante();
            integrante.Usuario = null;
            Assert.IsFalse(integrante.EhUsuarioDoSistema());
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
