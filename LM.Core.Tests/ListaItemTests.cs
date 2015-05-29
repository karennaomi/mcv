using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class ListaItemTests
    {
        private Fakes _fakes;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
        }

        [Test]
        public void NaoValidaItemComCosumoIgualAZero()
        {
            var item = _fakes.ListaItem();
            item.QuantidadeDeConsumo = 0;
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(item, new ValidationContext(item), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("Quantidade consumida deve ser maior que zero.", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("QuantidadeDeConsumo", error.MemberNames.ElementAt(0));
        }

        [Test]
        public void NaoValidaItemComCosumoMenorQueZero()
        {
            var item = _fakes.ListaItem();
            item.QuantidadeDeConsumo = -1;
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(item, new ValidationContext(item), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("Quantidade consumida deve ser maior que zero.", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("QuantidadeDeConsumo", error.MemberNames.ElementAt(0));
        }

        [Test]
        public void NaoValidaItemComCosumoIgualANull()
        {
            var item = _fakes.ListaItem();
            item.QuantidadeDeConsumo = null;
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(item, new ValidationContext(item), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("Quantidade consumida deve ser maior que zero.", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("QuantidadeDeConsumo", error.MemberNames.ElementAt(0));
        }
    }
}
