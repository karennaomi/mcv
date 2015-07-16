using LM.Core.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LM.Core.Tests
{
    [TestFixture]
    public class UsuarioTests
    {
        private Fakes _fakes;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
        }

        [Test]
        public void NaoValidaLoginNulo()
        {
            var usuario = _fakes.Usuario();
            usuario.Login = null;
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(usuario, new ValidationContext(usuario), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("Ops! Parece que você esqueceu de digitar o campo Login.", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("Login", error.MemberNames.ElementAt(0));
        }

        [Test]
        public void NaoValidaSenhaNula()
        {
            var usuario = _fakes.Usuario();
            usuario.Senha = null;
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(usuario, new ValidationContext(usuario), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("Ops! Parece que você esqueceu de digitar o campo Senha.", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("Senha", error.MemberNames.ElementAt(0));
        }

        [Test]
        public void NaoValidaSenhaComMenosDe6Caracteres()
        {
            var usuario = _fakes.Usuario();
            usuario.Senha = "12345";
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(usuario, new ValidationContext(usuario), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("O campo Senha deve possuir no mínimo 6 caracteres.", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("Senha", error.MemberNames.ElementAt(0));
        }

        [Test]
        public void NaoValidaUsuarioComMenosDe0Anos()
        {
            var usuario = _fakes.Usuario();
            usuario.Integrante.DataNascimento = DateTime.Now.AddYears(1);
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(usuario, new ValidationContext(usuario), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("O usuário deve ter 0 anos ou mais.", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("Integrante.DataNascimento", error.MemberNames.ElementAt(0));
        }

        [Test]
        public void RetornaStatusAtualCorreto()
        {
            var usuario = _fakes.Usuario();
            usuario.StatusUsuarioPontoDemanda = new Collection<StatusUsuarioPontoDemanda>
            {
                new StatusUsuarioPontoDemanda
                {
                    Id = 1,
                    DataInclusao = DateTime.Now.AddHours(-2),
                    StatusCadastro = StatusCadastro.EtapaDeInformacoesPessoaisCompleta
                },
                new StatusUsuarioPontoDemanda
                {
                    Id = 2,
                    DataInclusao = DateTime.Now.AddHours(-1),
                    StatusCadastro = StatusCadastro.EtapaDeEnderecoDoPontoDeDemandaCompleta
                },
                new StatusUsuarioPontoDemanda
                {
                    Id = 3,
                    DataInclusao = DateTime.Now,
                    StatusCadastro = StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta
                }
            };
            Assert.AreEqual(StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta, usuario.StatusAtual());
        }
    }
}
