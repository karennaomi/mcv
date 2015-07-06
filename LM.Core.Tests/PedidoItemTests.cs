using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LM.Core.Domain;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class PedidoItemTests
    {
        private Fakes _fakes;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
        }

        [Test]
        public void NaoValidaPedidoItemComQuantidadeIgualAZero()
        {
            var item = _fakes.PedidoItem();
            item.Quantidade = 0;
            AssertValidation(item, "Quantidade deve ser maior que zero.", "Quantidade");
        }

        [Test]
        public void NaoValidaPedidoItemComQuantidadeMenorQueZero()
        {
            var item = _fakes.PedidoItem();
            item.Quantidade = -1;
            AssertValidation(item, "Quantidade deve ser maior que zero.", "Quantidade");
        }

        private static void AssertValidation(PedidoItem item, string message, string property)
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
