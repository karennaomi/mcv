using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LM.Core.Domain;
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
            Assert.IsFalse(produto.EanValido);
            Assert.IsFalse(result);
            Assert.AreEqual(1, validationResults.Count);
            var error = validationResults[0];
            Assert.AreEqual("Ops! Parece que você esqueceu de digitar o campo Ean.", error.ErrorMessage);
            Assert.AreEqual(1, error.MemberNames.Count());
            Assert.AreEqual("Ean", error.MemberNames.ElementAt(0));
        }

        [Test]
        public void NaoValidaEanComMenosDe8Caracteres()
        {
            var produto = _fakes.Produto();
            produto.Ean = "1234567";
            Assert.IsFalse(produto.EanValido);
        }

        [Test]
        public void ValidaEanCom8Caracteres()
        {
            var produto = _fakes.Produto();
            produto.Ean = "78605831";
            Assert.IsTrue(produto.EanValido);
        }

        [Test]
        public void NaoValidaEanCom9Caracteres()
        {
            var produto = _fakes.Produto();
            produto.Ean = "123456789";
            Assert.IsFalse(produto.EanValido);
        }

        [Test]
        public void NaoValidaEanCom10Caracteres()
        {
            var produto = _fakes.Produto();
            produto.Ean = "1234567890";
            Assert.IsFalse(produto.EanValido);
        }

        [Test]
        public void NaoValidaEanCom11Caracteres()
        {
            var produto = _fakes.Produto();
            produto.Ean = "12345678901";
            Assert.IsFalse(produto.EanValido);
        }

        [Test]
        public void ValidaEanCom12Caracteres()
        {
            var produto = _fakes.Produto();
            produto.Ean = "722358073443";
            Assert.IsTrue(produto.EanValido);
        }

        [Test]
        public void ValidaEanCom13Caracteres()
        {
            var produto = _fakes.Produto();
            produto.Ean = "7891099353640";
            Assert.IsTrue(produto.EanValido);
        }

        [Test]
        public void ValidaEanCom14Caracteres()
        {
            var produto = _fakes.Produto();
            produto.Ean = "12345678901231";
            Assert.IsTrue(produto.EanValido);
        }

        [Test]
        public void NaoValidaEanComMaisDe14Caracteres()
        {
            var produto = _fakes.Produto();
            produto.Ean = "123456789012345";
            Assert.IsFalse(produto.EanValido);
        }

        [Test]
        public void NaoValidaEanComDigitoVerificadorErrado()
        {
            var produto = _fakes.Produto();
            produto.Ean = "1234567890123";
            Assert.IsFalse(produto.EanValido);
        }

        [Test]
        public void PrecoMedioIgualAZeroSeNaoTiverPreco()
        {
            var produto = _fakes.Produto();
            Assert.AreEqual(0, produto.PrecoMedio());
        }

        [Test]
        public void PrecoMedioIgualAZeroSeNaoTiverPrecoAtivo()
        {
            var produto = _fakes.Produto();
            produto.Precos = new List<ProdutoPreco>
            {
                new ProdutoPreco{ PrecoMax = 10, PrecoMin = 8, Ativo = false },
                new ProdutoPreco{ PrecoMax = 11, PrecoMin = 10, Ativo = false},
                new ProdutoPreco{ PrecoMax = 10.5M, PrecoMin = 7, Ativo = false }
            };
            Assert.AreEqual(0, produto.PrecoMedio());
        }

        [Test]
        public void PrecoMedioIgualAZeroSeTiverListaDePrecoVazia()
        {
            var produto = _fakes.Produto();
            produto.Precos = new List<ProdutoPreco>();
            Assert.AreEqual(0, produto.PrecoMedio());
        }

        [Test]
        public void PrecoMedioIgualAZeroSeTiverPrecoMaxMinNulos()
        {
            var produto = _fakes.Produto();
            produto.Precos = new List<ProdutoPreco>
            {
                new ProdutoPreco(),
                new ProdutoPreco{ PrecoMax = 11, PrecoMin = 10, Ativo = false},
                new ProdutoPreco{ PrecoMax = 10.5M, PrecoMin = 7, Ativo = false }
            };
            Assert.AreEqual(0, produto.PrecoMedio());
        }

        [Test]
        public void CalculaPrecoMedioComMinMaxDefinidos()
        {
            var produto = _fakes.Produto();
            produto.Precos = new List<ProdutoPreco>
            {
                new ProdutoPreco{ PrecoMax = 10, PrecoMin = 8 },
                new ProdutoPreco{ PrecoMax = 11, PrecoMin = 10, Ativo = false},
                new ProdutoPreco{ PrecoMax = 10.5M, PrecoMin = 7, Ativo = false }
            };
            Assert.AreEqual(9, produto.PrecoMedio());
        }

        [Test]
        public void CalculaPrecoMedioComMinMaxDefinidosValorQuebrado()
        {
            var produto = _fakes.Produto();
            produto.Precos = new List<ProdutoPreco>
            {
                new ProdutoPreco{ PrecoMax = 10, PrecoMin = 8, Ativo = false },
                new ProdutoPreco{ PrecoMax = 11, PrecoMin = 10, Ativo = false},
                new ProdutoPreco{ PrecoMax = 10.5M, PrecoMin = 7 }
            };
            Assert.AreEqual(8.75M, produto.PrecoMedio());
        }

        [Test]
        public void CalculaPrecoMedioSomenteComMinDefinido()
        {
            var produto = _fakes.Produto();
            produto.Precos = new List<ProdutoPreco>
            {
                new ProdutoPreco{ PrecoMin = 8 },
                new ProdutoPreco{ PrecoMax = 11, PrecoMin = 10, Ativo = false},
                new ProdutoPreco{ PrecoMax = 10.5M, PrecoMin = 7, Ativo = false }
            };
            Assert.AreEqual(8, produto.PrecoMedio());
        }

        [Test]
        public void CalculaPrecoMedioSomenteComMaxDefinido()
        {
            var produto = _fakes.Produto();
            produto.Precos = new List<ProdutoPreco>
            {
                new ProdutoPreco{ PrecoMax = 10 },
                new ProdutoPreco{ PrecoMax = 11, PrecoMin = 10, Ativo = false},
                new ProdutoPreco{ PrecoMax = 10.5M, PrecoMin = 7, Ativo = false }
            };
            Assert.AreEqual(10, produto.PrecoMedio());
        }
    }
}
