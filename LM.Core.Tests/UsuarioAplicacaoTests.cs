using System;
using System.Collections.ObjectModel;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using LM.Core.RepositorioEF;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Transactions;

namespace LM.Core.Tests
{
    [TestFixture]
    public class UsuarioAplicacaoTests
    {
        [Test]
        public void CriacaoDoUsuarioDeveDefinirStatusCadastroComoEtapaDeInformacoesPessoaisCompleta()
        {
            var usuario = Fakes.Usuario();
            var app = ObterAppUsuario();
            using (new TransactionScope())
            {
                usuario = app.Criar(usuario);
                Assert.AreEqual(StatusCadastro.EtapaDeInformacoesPessoaisCompleta, usuario.StatusUsuarioPontoDemanda.StatusCadastro);
            }
        }

        [Test]
        public void CriaUmUsuario()
        {
            var app = ObterAppUsuario();
            using (new TransactionScope())
            {
                var usuario = app.Criar(Fakes.Usuario());
                Assert.IsTrue(usuario.Id > 0);
                Assert.IsTrue(usuario.Integrante.Id > 0);
                Assert.IsTrue(usuario.Integrante.Persona.Id > 0);
                Assert.IsTrue(usuario.Integrante.GrupoDeIntegrantes.Id > 0);
            }
        }

        [Test]
        public void AtualizaStatusCadastro()
        {
            var app = ObterAppUsuario();
            using (new TransactionScope())
            {
                var usuario = app.Criar(Fakes.Usuario());
                app.AtualizaStatusCadastro(usuario.Id, StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta);
                usuario = app.Obter(usuario.Id);
                Assert.AreEqual(StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta, usuario.StatusUsuarioPontoDemanda.StatusCadastro);
            }
        }

        [Test]
        public void AtualizaStatusCadastroComPontoDemanda()
        {
            var app = ObterAppUsuario();
            var pontoDemandaId = new ContextoEF().PontosDemanda.First().Id;
            using (new TransactionScope())
            {
                var usuario = app.Criar(Fakes.Usuario());
                app.AtualizaStatusCadastro(usuario.Id, StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta, pontoDemandaId);
                usuario = app.Obter(usuario.Id);
                Assert.AreEqual(StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta, usuario.StatusUsuarioPontoDemanda.StatusCadastro);
                Assert.AreEqual(pontoDemandaId, usuario.StatusUsuarioPontoDemanda.PontoDemandaId);
            }
        }

        [Test]
        public void CriarUsuario18AnosSexoMasculinoDefinePersonaCorreta()
        {
            IntegrantePersonaTestes(18, "M", 18, 27);
        }

        [Test]
        public void CriarUsuario18AnosSexoFemininoDefinePersonaCorreta()
        {
            IntegrantePersonaTestes(18, "F", 18, 27);
        }

        [Test]
        public void CriarUsuario27AnosSexoMasculinoDefinePersonaCorreta()
        {
            IntegrantePersonaTestes(27, "M", 18, 27);
        }

        [Test]
        public void CriarUsuario27AnosSexoFemininoDefinePersonaCorreta()
        {
            IntegrantePersonaTestes(27, "F", 18, 27);
        }

        [Test]
        public void CriarUsuario23AnosSexoMasculinoDefinePersonaCorreta()
        {
            IntegrantePersonaTestes(23, "M", 18, 27);
        }

        [Test]
        public void CriarUsuarioIdadeNegativaLancaException()
        {
            Assert.Throws<ApplicationException>(() => IntegrantePersonaTestes(-1, "F", 18, 27));
        }

        [Test]
        public void CriarUsuarioIdadeMaior150AnosLancaException()
        {
            Assert.Throws<ApplicationException>(() => IntegrantePersonaTestes(151, "F", 18, 27));
        }

        [Test]
        public void CriarUsuario23AnosSexoFemininoDefinePersonaCorreta()
        {
            IntegrantePersonaTestes(23, "F", 18, 27);
        }

        private static void IntegrantePersonaTestes(int idade, string sexo, int idadeInicial, int idadeFinal)
        {
            var integrante = Fakes.Integrante(idade, sexo);
            var usuario = integrante.Usuario;
            usuario.MapIntegrantes = new Collection<Integrante>{ integrante };
            integrante.Usuario = usuario;
            var app = ObterAppUsuario();
            usuario = app.Criar(usuario);
            Assert.AreEqual(sexo, usuario.Integrante.Persona.Sexo);
            Assert.AreEqual(idadeInicial, usuario.Integrante.Persona.IdadeInicial);
            Assert.AreEqual(idadeFinal, usuario.Integrante.Persona.IdadeFinal);
        }

        private static IUsuarioAplicacao ObterAppUsuario()
        {
            return new UsuarioAplicacao(new UsuarioEF(), new PersonaAplicacao(new PersonaEF()));
        }
    }
}
