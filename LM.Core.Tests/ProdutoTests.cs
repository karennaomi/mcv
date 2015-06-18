using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class ProdutoTests
    {
        private Fakes _fakes;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
        }

        [Test]
        public void NaoValidaEanNulo()
        {
            var produto = _fakes.Produto();
            produto.Ean = null;
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(produto, new ValidationContext(produto), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("O codigo EAN do produto é obrigatório.", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("Ean", error.MemberNames.ElementAt(0));
        }

        [Test]
        public void NaoValidaEanComMaisDe13Caracteres()
        {
            var produto = _fakes.Produto();
            produto.Ean = "12345678901234";
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(produto, new ValidationContext(produto), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("O codigo EAN deve ter no máximo 13 caracteres.", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("Ean", error.MemberNames.ElementAt(0));
        }
    }
}
