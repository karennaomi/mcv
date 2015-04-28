using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LM.Core.RepositorioEF;
using NUnit.Framework;
using LM.Core.Application;

namespace LM.Core.Tests
{
    [TestFixture]
    public class PersonaAplicacaoTests
    {
        [Test]
        public void ObterPet()
        {
            var persona = ObterApp().Obter(0, null, "pet");
            Assert.IsTrue(persona.Perfil.StartsWith("PET"));
        }

        [Test]
        public void ObterEmpregadoMsculino()
        {
            var persona = ObterApp().Obter(0, "M", "empregado");
            Assert.IsTrue(persona.Perfil.StartsWith("EMPREGADO"));
            Assert.AreEqual("M", persona.Sexo);
        }

        [Test]
        public void ObterAdulto()
        {
            var persona = ObterApp().Obter(25, "M", "integrante_comum");
            Assert.IsTrue(persona.Perfil.StartsWith("ADULTO"));
            Assert.AreEqual("M", persona.Sexo);
        }

        private IPersonaAplicacao ObterApp()
        {
            return new PersonaAplicacao(new PersonaEF());
        }
    }
}
