using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using LM.Core.RepositorioEF;
using Moq;
using NUnit.Framework;
using System;
using System.Transactions;

namespace LM.Core.Tests
{
    [TestFixture]
    public class RecuperarSenhaAplicacaoTests
    {
        private const string UrlTrocarSenha = "http://teste.com/trocarsenha";

        [Test]
        public void CriaUmaRecuperacaoDeSenha()
        {
            using (new TransactionScope())
            {
                var app = new RecuperarSenhaAplicacao(new RecuperarSenhaEF(),
                new UsuarioAplicacao(new UsuarioEF()), new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF()));
                var recuperarSenha = app.RecuperarSenha("thanos@marvel.com", UrlTrocarSenha);
                Assert.IsTrue(recuperarSenha.Id > 0);
                Assert.IsNotNull(recuperarSenha.Usuario);
            }
        }

        [Test]
        public void TrocarSenha()
        {
            using (new TransactionScope())
            {
                var app = GetApp();
                var recuperarSenha = app.RecuperarSenha("thanos@marvel.com", UrlTrocarSenha);
                var token = recuperarSenha.Token;
                var usuarioId = recuperarSenha.Usuario.Id;
                
                var app2 = GetApp();
                app2.TrocarSenha(token, "abc123def");

                var appUsuario = new UsuarioAplicacao(new UsuarioEF());
                var usuario = appUsuario.Obter(usuarioId);
                Assert.IsTrue(PasswordHash.ValidatePassword("abc123def", usuario.Senha));
            }
        }

        [Test]
        public void TokenInvalidoRetornaFalso()
        {
            var mockedApp = GetMockedApp();
            Assert.IsFalse(mockedApp.ValidarToken(new Guid("176CA494-E477-410C-967D-B059A49003D9")));
        }

        [Test]
        public void TokenValidoPoremExpiradoRetornaFalso()
        {
            var mockedApp = GetMockedApp();
            Assert.IsFalse(mockedApp.ValidarToken(new Guid("176CA494-E477-410C-967D-B059A49003B4")));
        }

        [Test]
        public void TokenValidoDentroDoPrazoRetornaTrue()
        {
            var mockedApp = GetMockedApp();
            Assert.IsTrue(mockedApp.ValidarToken(new Guid("176CA494-E477-410C-967D-B059A49003C3")));
        }

        private static IRecuperarSenhaAplicacao GetApp()
        {
            return new RecuperarSenhaAplicacao(new RecuperarSenhaEF(),
                new UsuarioAplicacao(new UsuarioEF()), new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF()));
        }

        private static IRecuperarSenhaAplicacao GetMockedApp()
        {
            var repoMock = new Mock<IRepositorioRecuperarSenha>();
            repoMock.Setup(r => r.ObterPorToken(new Guid("176CA494-E477-410C-967D-B059A49003D9"))).Returns((RecuperarSenha)null);
            repoMock.Setup(r => r.ObterPorToken(new Guid("176CA494-E477-410C-967D-B059A49003B4"))).Returns(new RecuperarSenha { DataInclusao = DateTime.Now.AddHours(-1), Token = new Guid("176CA494-E477-410C-967D-B059A49003B4") });
            repoMock.Setup(r => r.ObterPorToken(new Guid("176CA494-E477-410C-967D-B059A49003C3"))).Returns(new RecuperarSenha { DataInclusao = DateTime.Now.AddMinutes(-10), Token = new Guid("176CA494-E477-410C-967D-B059A49003C3") });
            return new RecuperarSenhaAplicacao(repoMock.Object, null, null, null);
        }
    }
}
