using LM.Core.Domain;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class PersonaTests
    {
        [Test]
        public void ObterImagemBebeFemino()
        {
            var persona = new Persona { Perfil = "BEBÊ", IdadeInicial = 0, IdadeFinal = 3, Sexo = "F" };
            Assert.AreEqual("galeria-bebe-f-0-3.png", persona.ObterImage());
        }

        [Test]
        public void ObterImagemBebeMasculino()
        {
            var persona = new Persona { Perfil = "BEBÊ", IdadeInicial = 0, IdadeFinal = 3, Sexo = "M" };
            Assert.AreEqual("galeria-bebe-m-0-3.png", persona.ObterImage());
        }

        [Test]
        public void ObterImagemCriancaFemino3_8()
        {
            var persona = new Persona { Perfil = "CRIANÇA", IdadeInicial = 3, IdadeFinal = 8, Sexo = "F" };
            Assert.AreEqual("galeria-crianca-f-3-8.png", persona.ObterImage());

        }

        [Test]
        public void ObterImagemCriancamsculino3_8()
        {
            var persona = new Persona { Perfil = "CRIANÇA", IdadeInicial = 3, IdadeFinal = 8, Sexo = "M" };
            Assert.AreEqual("galeria-crianca-m-3-8.png", persona.ObterImage());
        }

        [Test]
        public void ObterImagemCat()
        {
            var persona = new Persona { Perfil = "PET CAT"};
            Assert.AreEqual("galeria-pet_cat.png", persona.ObterImage());
        }

        [Test]
        public void ObterImagemDog()
        {
            var persona = new Persona { Perfil = "PET DOG" };
            Assert.AreEqual("galeria-pet_dog.png", persona.ObterImage());
        }

        [Test]
        public void ObterDescricaoBebeMasculino()
        {
            var persona = new Persona { Perfil = "BEBÊ", IdadeInicial = 0, IdadeFinal = 3, Sexo = "M" };
            Assert.AreEqual("BEBÊ M 0-3 ANOS", persona.Descricao());
        }

        [Test]
        public void ObterDescricaoBebeFeminino()
        {
            var persona = new Persona { Perfil = "BEBÊ", IdadeInicial = 0, IdadeFinal = 3, Sexo = "F" };
            Assert.AreEqual("BEBÊ F 0-3 ANOS", persona.Descricao());
        }

        [Test]
        public void ObterDescricaoCriancaMasculino()
        {
            var persona = new Persona { Perfil = "CRIANÇA", IdadeInicial = 3, IdadeFinal = 8, Sexo = "M" };
            Assert.AreEqual("MENINO 3-8 ANOS", persona.Descricao());
        }

        [Test]
        public void ObterDescricaoCriancaFeminino()
        {
            var persona = new Persona { Perfil = "CRIANÇA", IdadeInicial = 3, IdadeFinal = 8, Sexo = "F" };
            Assert.AreEqual("MENINA 3-8 ANOS", persona.Descricao());
        }

        [Test]
        public void ObterDescricaoAdolescenteMasculino()
        {
            var persona = new Persona { Perfil = "ADOLESCENTE", IdadeInicial = 14, IdadeFinal = 17, Sexo = "M" };
            Assert.AreEqual("MENINO 14-17 ANOS", persona.Descricao());
        }

        [Test]
        public void ObterDescricaoAdolescenteFeminino()
        {
            var persona = new Persona { Perfil = "ADOLESCENTE", IdadeInicial = 14, IdadeFinal = 17, Sexo = "F" };
            Assert.AreEqual("MENINA 14-17 ANOS", persona.Descricao());
        }

        [Test]
        public void ObterDescricaoAdultoMasculino()
        {
            var persona = new Persona { Perfil = "ADULTO", IdadeInicial = 18, IdadeFinal = 27, Sexo = "M" };
            Assert.AreEqual("HOMEM 18-27 ANOS", persona.Descricao());
        }

        [Test]
        public void ObterDescricaoAdultoFeminino()
        {
            var persona = new Persona { Perfil = "ADULTO", IdadeInicial = 18, IdadeFinal = 27, Sexo = "F" };
            Assert.AreEqual("MULHER 18-27 ANOS", persona.Descricao());
        }

        [Test]
        public void ObterDescricaoIdoso()
        {
            var persona = new Persona { Perfil = "IDOSO", IdadeInicial = 66, IdadeFinal = 150, Sexo = "M" };
            Assert.AreEqual("IDOSO +65", persona.Descricao());
        }

        [Test]
        public void ObterDescricaoIdosa()
        {
            var persona = new Persona { Perfil = "IDOSO", IdadeInicial = 66, IdadeFinal = 150, Sexo = "F" };
            Assert.AreEqual("IDOSA +65", persona.Descricao());
        }

        [Test]
        public void ObterDescricaoEmpregado()
        {
            var persona = new Persona { Perfil = "EMPREGADO", IdadeInicial = 18, IdadeFinal = 65, Sexo = "M" };
            Assert.AreEqual("EMPREGADO", persona.Descricao());
        }

        [Test]
        public void ObterDescricaoEmpregada()
        {
            var persona = new Persona { Perfil = "EMPREGADO", IdadeInicial = 18, IdadeFinal = 65, Sexo = "F" };
            Assert.AreEqual("EMPREGADA", persona.Descricao());
        }

        [Test]
        public void ObterDescricaoCachorro()
        {
            var persona = new Persona { Perfil = "PET DOG" };
            Assert.AreEqual("CACHORRO", persona.Descricao());
        }

        [Test]
        public void ObterDescricaoGato()
        {
            var persona = new Persona { Perfil = "PET CAT" };
            Assert.AreEqual("GATO", persona.Descricao());
        }
    }
}
