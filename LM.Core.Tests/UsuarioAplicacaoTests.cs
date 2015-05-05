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
                Assert.AreEqual(StatusCadastro.EtapaDeInformacoesPessoaisCompleta, usuario.StatusUsuarioPontoDemanda.First().StatusCadastro);
            }
        }

        [Test]
        public void CriacaoDoUsuarioConvidadoDeveDefinirStatusCadastroComoUsuarioConvidado()
        {
            var usuario = Fakes.Usuario();
            usuario.Integrante.Email = "esposa@lista.com";
            var app = ObterAppUsuario();
            using (new TransactionScope())
            {
                usuario = app.Criar(usuario);
                Assert.AreEqual(StatusCadastro.UsuarioConvidado, usuario.StatusAtual());
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
                Assert.IsTrue(usuario.Integrante.GrupoDeIntegrantes.Id > 0);
            }
        }

        [Test]
        public void CriaUmUsuarioEValidaLogin()
        {
            var app = ObterAppUsuario();
            using (new TransactionScope())
            {
                var usuario = Fakes.Usuario();
                usuario = app.Criar(usuario);
                var usuarioValidado = app.ValidarLogin(usuario.Login, "123456");
                Assert.IsTrue(usuario.Id > 0);
                Assert.IsNotNull(usuarioValidado);
            }
        }

        [Test]
        public void AtualizaStatusCadastro()
        {
            var app = ObterAppUsuario();
            using (new TransactionScope())
            {
                var usuario = app.Criar(Fakes.Usuario());
                app.AtualizarStatusCadastro(usuario.Id, StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta);
                usuario = app.Obter(usuario.Id);
                Assert.AreEqual(StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta, usuario.StatusAtual());
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
                app.AtualizarStatusCadastro(usuario.Id, StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta, pontoDemandaId);
                usuario = app.Obter(usuario.Id);
                Assert.AreEqual(StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta, usuario.StatusAtual());
                Assert.AreEqual(pontoDemandaId, usuario.StatusUsuarioPontoDemanda.First(s => s.PontoDemandaId.HasValue).PontoDemandaId);
            }
        }

        [Test]
        public void AtualizarDeviceInfo()
        {
            ObterAppUsuario().AtualizarDeviceInfo(2, "google", "nsuiahfui2u2h2u9hf42");
        }

        private static IUsuarioAplicacao ObterAppUsuario()
        {
            return new UsuarioAplicacao(new UsuarioEF());
        }
    }
}
