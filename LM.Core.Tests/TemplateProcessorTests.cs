using LM.Core.Application;
using NUnit.Framework;
using System;
using System.Globalization;

namespace LM.Core.Tests
{
    [TestFixture]
    public class TemplateProcessorTests
    {
        [Test]
        public void ProcessaTemplateComObjeto1Nivel()
        {
            const string template = "Bem vindo {Nome}, você tem {Idade} anos";
            var objeto = new {Nome = "Joe", Idade = 5};
            var resultado = TemplateProcessor.ProcessTemplate(template, objeto);
            Assert.AreEqual("Bem vindo Joe, você tem 5 anos", resultado);
        }

        [Test]
        public void ProcessaTemplateComObjeto2Niveis()
        {
            const string template = "Bem vindo {Nome}, você tem {Idade} anos, voce comprou {Produto.Nome} por {Produto.Preco}";
            var objeto = new { Nome = "Joe", Idade = 5, Produto = new {Nome = "Vassoura", Preco = 5.55.ToString("C", new CultureInfo("pt-BR"))} };
            var resultado = TemplateProcessor.ProcessTemplate(template, objeto);
            Assert.AreEqual("Bem vindo Joe, você tem 5 anos, voce comprou Vassoura por R$ 5,55", resultado);
        }

        [Test]
        public void ProcessaTemplateComPropriedadeInvalidaLancaExcecao()
        {
            const string template = "Ola {Nome}";
            var objeto = new { Idade = 5 };
            var ex = Assert.Throws<ApplicationException>(() => TemplateProcessor.ProcessTemplate(template, objeto));
            Assert.AreEqual("Propriedade inválida no template: Nome", ex.Message);
            Assert.IsInstanceOf<NullReferenceException>(ex.InnerException);
        }
    }
}
