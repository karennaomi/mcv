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
            var app = new UsuarioAplicacao(ObterUsuarioRepo(usuario));
            usuario = app.Criar(usuario);
            Assert.AreEqual(StatusCadastro.EtapaDeInformacoesPessoaisCompleta, usuario.StatusUsuarioPontoDemanda.StatusCadastro);
        }

        [Test]
        public void CriaUmUsuario()
        {
            var app = new UsuarioAplicacao(new UsuarioEF());
            using (new TransactionScope())
            {
                var usuario = app.Criar(Fakes.Usuario());
                Assert.IsTrue(usuario.Id > 0);
            }
        }

        [Test]
        public void AtualizaStatusCadastro()
        {
            var app = new UsuarioAplicacao(new UsuarioEF());
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
            var app = new UsuarioAplicacao(new UsuarioEF());
            var pontoDemandaId = new ContextoEF().PontosDemanda.First().Id;
            using (new TransactionScope())
            {
                var usuario = app.Criar(Fakes.Usuario());
                app.AtualizaStatusCadastro(usuario.Id, StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta, pontoDemandaId);
                Assert.AreEqual(StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta, usuario.StatusUsuarioPontoDemanda.StatusCadastro);
                Assert.AreEqual(pontoDemandaId, usuario.StatusUsuarioPontoDemanda.PontoDemandaId);
            }
        }

        private static IRepositorioUsuario ObterUsuarioRepo(Usuario usuario)
        {
            var repoMock = new Mock<IRepositorioUsuario>();
            repoMock.Setup(r => r.Criar(It.IsAny<Usuario>())).Returns(usuario);
            return repoMock.Object;
        }
    }
}
