using LM.Core.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
            item.QuantidadeConsumo = 0;
            AssertValidation(item, "Quantidade consumida deve ser maior que zero.", "QuantidadeDeConsumo");
        }

        [Test]
        public void NaoValidaItemComCosumoMenorQueZero()
        {
            var item = _fakes.ListaItem();
            item.QuantidadeConsumo = -1;
            AssertValidation(item, "Quantidade consumida deve ser maior que zero.", "QuantidadeDeConsumo");
        }

        [Test]
        public void NaoValidaItemComCosumoIgualANull()
        {
            var item = _fakes.ListaItem();
            item.QuantidadeConsumo = null;
            AssertValidation(item, "Quantidade consumida deve ser maior que zero.", "QuantidadeDeConsumo");
        }

        [Test]
        public void NaoValidaItemComEstoqueMenorQueZero()
        {
            var item = _fakes.ListaItem();
            item.QuantidadeEstoque = -1;
            AssertValidation(item, "Quantidade em estoque deve ser maior ou igual a zero.", "QuantidadeEmEstoque");
        }

        [Test]
        public void NaoValidaItemComEstoqueIgualANull()
        {
            var item = _fakes.ListaItem();
            item.QuantidadeEstoque = null;
            AssertValidation(item, "Quantidade em estoque deve ser maior ou igual a zero.", "QuantidadeEmEstoque");
        }

        [Test]
        public void NaoValidaItemComPeriodoIgualANull()
        {
            var item = _fakes.ListaItem();
            item.Periodo = null;
            AssertValidation(item, "O período de consumo deve ser informado.", "Periodo");
        }

        private static void AssertValidation(ListaItem item, string message, string property)
        {
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(item, new ValidationContext(item), validationResults, true);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual(message, error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual(property, error.MemberNames.ElementAt(0));
        }
    }
}
