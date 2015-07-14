using System;
using System.Data.Entity.Validation;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using LM.Core.RepositorioEF;
using NUnit.Framework;
using System.Linq;
using System.Transactions;

namespace LM.Core.Tests
{
    [TestFixture]
    public class UsuarioAplicacaoTests
    {
        private Fakes _fakes;
        private MockUsuarioRepo _mockUsuarioRepo;
        private MockContratoRepo _mockContratoRepo;

        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
            _mockUsuarioRepo = new MockUsuarioRepo();
            _mockContratoRepo = new MockContratoRepo();
        }

        [Test]
        public void CriaUsuario()
        {
            var usuario = _fakes.Usuario();
            usuario.Id = 0;
            var app = ObterAppUsuario(new UsuarioEF(), new ContratoEF());
            using (new TransactionScope())
            {
                try
                {
                    usuario = app.Criar(usuario);
                    Assert.IsTrue(usuario.Id > 0);
                    Assert.IsTrue(usuario.Integrante.Id > 0);
                }
                catch (DbEntityValidationException ex)
                {
                    throw ex;
                }
            }
        }

        [Test]
        public void CriacaoDoUsuarioDeveDefinirStatusCadastroComoEtapaDeInformacoesPessoaisCompleta()
        {
            var usuario = _fakes.Usuario();
            var app = ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo());
            usuario = app.Criar(usuario);
            Assert.AreEqual(StatusCadastro.EtapaDeInformacoesPessoaisCompleta, usuario.StatusUsuarioPontoDemanda.First().StatusCadastro);
        }

        [Test]
        public void CriacaoDoUsuarioConvidadoDeveDefinirStatusCadastroComoOk()
        {
            var usuario = _fakes.Usuario();
            usuario.Integrante.Email = "integrante@convidado.com";

            var integranteConvidado = _fakes.Integrante();
            _mockUsuarioRepo.Integrante = integranteConvidado;
            var app = ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo());
            usuario = app.Criar(usuario);
            Assert.IsTrue(usuario.StatusUsuarioPontoDemanda.Any(s => s.StatusCadastro == StatusCadastro.UsuarioOk));
            Assert.IsFalse(usuario.Integrante.EhUsuarioConvidado);
            Assert.AreSame(integranteConvidado, usuario.Integrante);
        }

        [Test]
        public void AtualizaUsuario()
        {
            var usuarioParaAtualizar = _fakes.Usuario();
            usuarioParaAtualizar.Id = 1;
            _mockUsuarioRepo.Usuario = usuarioParaAtualizar;
            var app = ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo());
            
            var usuario = _fakes.Usuario();
            usuario.Id = 1;
            usuario.Integrante.Nome = "Nomo Atualizado";
            usuario.Integrante.Email = "email@atualizado.com";
            var dataNascimento = DateTime.Now.AddYears(-27);
            usuario.Integrante.DataNascimento = dataNascimento;
            usuario.Integrante.Sexo = "f";

            app.Atualizar(usuario);
            Assert.AreEqual("Nomo Atualizado", usuarioParaAtualizar.Integrante.Nome);
            Assert.AreEqual("email@atualizado.com", usuarioParaAtualizar.Integrante.Email);
            Assert.AreEqual(dataNascimento, usuarioParaAtualizar.Integrante.DataNascimento);
            Assert.AreEqual("f", usuarioParaAtualizar.Integrante.Sexo);
        }

        [Test]
        public void NaoPodeAtualizarUmUsuarioComEmailJaExistente()
        {
            var usuarioParaAtualizar = _fakes.Usuario();
            usuarioParaAtualizar.Id = 1;
            _mockUsuarioRepo.Usuario = usuarioParaAtualizar;
            var app = ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo());

            var usuario = _fakes.Usuario();
            usuario.Id = 1;
            usuario.Integrante.Email = "email@existente.com";

            Assert.Throws<IntegranteExistenteException>(() => app.Atualizar(usuario));
        }

        [Test]
        public void ValidaUmUsuarioComLoginValido()
        {
            var usuarioLogin = _fakes.Usuario();
            usuarioLogin.Login = "usuario@login.com";
            usuarioLogin.Senha = "1000:tQGg+TbIlRzwKkuiAH0EvzMCYmY1Y6V2:ZRp/QGdEktD35jvoPTHWEnAom7btjmCV"; //hash de '789456'
            _mockUsuarioRepo.Usuario = usuarioLogin;
            var app = ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo());
            var usuarioValidado = app.ValidarLogin("usuario@login.com", "789456");
            Assert.IsNotNull(usuarioValidado);
        }

        [Test]
        public void NaoValidaUmUsuarioComLoginInvalido()
        {
            var usuarioLogin = _fakes.Usuario();
            usuarioLogin.Login = "usuario@login.com";
            usuarioLogin.Senha = "1000:tQGg+TbIlRzwKkuiAH0EvzMCYmY1Y6V2:ZRp/QGdEktD35jvoPTHWEnAom7btjmCV"; //hash de '789456'
            _mockUsuarioRepo.Usuario = usuarioLogin;
            var app = ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo());
            Assert.Throws<LoginInvalidoException>(() => app.ValidarLogin("usuario@login.com", "123456"));
        }

        [Test]
        public void NaoValidaUmUsuarioDesativado()
        {
            var usuarioLogin = _fakes.Usuario();
            usuarioLogin.Login = "usuario@login.com";
            usuarioLogin.Senha = "1000:tQGg+TbIlRzwKkuiAH0EvzMCYmY1Y6V2:ZRp/QGdEktD35jvoPTHWEnAom7btjmCV"; //hash de '789456'
            usuarioLogin.Ativo = false;
            _mockUsuarioRepo.Usuario = usuarioLogin;
            var app = ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo());
            var ex = Assert.Throws<LoginInvalidoException>(() => app.ValidarLogin("usuario@login.com", "789456"));
            Assert.AreEqual("Este e-mail esta desativado.", ex.Message);
        }

        [Test]
        public void AtualizaStatusCadastro()
        {
            var usuario = _fakes.Usuario();
            usuario.Id = 1;
            _mockUsuarioRepo.Usuario = usuario;
            var app = ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo());
            
            app.AtualizarStatusCadastro(1, StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta);
            usuario = app.Obter(1);
            Assert.AreEqual(StatusCadastro.EtapaDoGrupoDeIntegrantesCompleta, usuario.StatusAtual());
        }

        [Test]
        public void AtualizaStatusCadastroComPontoDemanda()
        {
            var usuario = _fakes.Usuario();
            usuario.Id = 1;
            _mockUsuarioRepo.Usuario = usuario;
            var app = ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo());

            app.AtualizarStatusCadastro(1, StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta, 2);
            usuario = app.Obter(1);
            Assert.AreEqual(StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta, usuario.StatusAtual());
            Assert.AreEqual(2, usuario.StatusUsuarioPontoDemanda.First(s => s.StatusCadastro == StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta).PontoDemandaId);
        }

        [Test]
        public void AtualizarDeviceInfo()
        {
            var usuario = _fakes.Usuario();
            usuario.Id = 1;
            _mockUsuarioRepo.Usuario = usuario;
            ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo()).AtualizarDeviceInfo(1, "google", "nsuiahfui2u2h2u9hf42");
            Assert.AreEqual("google", usuario.DeviceType);
            Assert.AreEqual("nsuiahfui2u2h2u9hf42", usuario.DeviceId);
        }

        private static IUsuarioAplicacao ObterAppUsuario(IRepositorioUsuario usuarioRepo, IRepositorioContrato contratoRepo)
        {
            return new UsuarioAplicacao(usuarioRepo, new ContratoAplicacao(contratoRepo), new MockNotificacaoApp().GetMockedApp());
        }
    }
}
