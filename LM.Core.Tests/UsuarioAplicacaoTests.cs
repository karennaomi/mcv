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
                Assert.AreEqual(StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta, usuario.StatusUsuarioPontoDemanda.StatusCadastro);
                Assert.AreEqual(pontoDemandaId, usuario.StatusUsuarioPontoDemanda.PontoDemandaId);
            }
        }

        private static IUsuarioAplicacao ObterAppUsuario()
        {
            return new UsuarioAplicacao(new UsuarioEF(), new IntegranteAplicacao(new IntegranteEF(), new PersonaAplicacao(new PersonaEF()), null));
        }
    }
}
