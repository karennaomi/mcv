using LM.Core.Domain;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class UsuarioTests
    {
        [Test]
        public void StringPersonaMeninoDefineSexoMasculino()
        {
            var usuario = Fakes.Usuario();
            usuario.DefinirSexo("menino-14-17");
            Assert.AreEqual("M", usuario.Sexo);
        }

        [Test]
        public void StringPersonaMeninaDefineSexoFeminino()
        {
            var usuario = Fakes.Usuario();
            usuario.DefinirSexo("menina-14-17");
            Assert.AreEqual("F", usuario.Sexo);
        }

        [Test]
        public void StringPersonaHomemDefineSexoMasculino()
        {
            var usuario = Fakes.Usuario();
            usuario.DefinirSexo("homem-18-27");
            Assert.AreEqual("M", usuario.Sexo);
        }

        [Test]
        public void StringPersonaMulherDefineSexoFeminino()
        {
            var usuario = Fakes.Usuario();
            usuario.DefinirSexo("mulher-18-27");
            Assert.AreEqual("F", usuario.Sexo);
        }

        [Test]
        public void StringPersonaEmpregadoDefineSexoMasculino()
        {
            var usuario = Fakes.Usuario();
            usuario.DefinirSexo("empregado-18-27");
            Assert.AreEqual("M", usuario.Sexo);
        }

        [Test]
        public void StringPersonaEmpregadaDefineSexoFeminino()
        {
            var usuario = Fakes.Usuario();
            usuario.DefinirSexo("empregada-18-27");
            Assert.AreEqual("F", usuario.Sexo);
        }

        [Test]
        public void StringPersonaIdosoDefineSexoMasculino()
        {
            var usuario = Fakes.Usuario();
            usuario.DefinirSexo("idoso-18-27");
            Assert.AreEqual("M", usuario.Sexo);
        }

        [Test]
        public void StringPersonaIdosaDefineSexoFeminino()
        {
            var usuario = Fakes.Usuario();
            usuario.DefinirSexo("idosa-18-27");
            Assert.AreEqual("F", usuario.Sexo);
        }
    }
}
