using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Transactions;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using LM.Core.RepositorioEF;
using Moq;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class PontoDemandaAplicacaoTests
    {
        private Fakes _fakes;
        private MockPontoDemandaRepo _mockRepo;
        private MockUsuarioRepo _mockUsuarioRepo;
        private MockContratoRepo _mockContratoRepo;
        private ContextoEF _contexto;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
            _mockRepo = new MockPontoDemandaRepo();
            _mockUsuarioRepo = new MockUsuarioRepo();
            _mockContratoRepo = new MockContratoRepo
            {
                Contrato = _fakes.Contrato()
            };
            _contexto = new ContextoEF();
        }

        [Test]
        public void CriarPontoDemanda()
        {
            using (new TransactionScope())
            {
                var appUsuario = ObterAppUsuario(new UsuarioEF(), new ContratoEF());
                var usuario = appUsuario.Criar(_fakes.Usuario("usuario_w@mail.com"));
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), ObterAppUsuario(new UsuarioEF(), new ContratoEF()));
                var pontoDemanda = _fakes.PontoDemanda();
                pontoDemanda.Id = 0;
                app.Criar(usuario.Id, pontoDemanda);
                var pontoDemandaNovo = app.Obter(usuario.Id, pontoDemanda.Id);
                Assert.IsTrue(pontoDemandaNovo.Id > 0);
                Assert.IsTrue(pontoDemandaNovo.Listas.Any());
            }
        }

        [Test]
        public void CriarPontoDemandaComLojasFavoritas()
        {
            using (new TransactionScope())
            {
                var appUsuario = ObterAppUsuario(new UsuarioEF(), new ContratoEF());
                var usuario = appUsuario.Criar(_fakes.Usuario("usuario_y@mail.com"));
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), ObterAppUsuario(new UsuarioEF(), new ContratoEF()));
                var pontoDemanda = _fakes.PontoDemanda();
                pontoDemanda.Id = 0;
                pontoDemanda.LojasFavoritas = new List<Loja> { _fakes.Loja(), _fakes.Loja(), _fakes.Loja() };
                app.Criar(usuario.Id, pontoDemanda);
                var pontoDemandaNovo = app.Obter(usuario.Id, pontoDemanda.Id);
                Assert.IsTrue(pontoDemandaNovo.Id > 0);
                Assert.AreEqual(3, pontoDemanda.LojasFavoritas.Count);
                Assert.IsTrue(pontoDemanda.LojasFavoritas.All(l => l.Id > 0));
            }
        }

        [Test]
        public void AtualizarPontoDemanda()
        {
            using (new TransactionScope())
            {
                var pontoDemanda0 = new ContextoEF().PontosDemanda.First();
                var usuarioId = pontoDemanda0.GruposDeIntegrantes.First().Integrante.Usuario.Id;

                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), ObterAppUsuario(new UsuarioEF(), new ContratoEF()));
                var pontoDemanda = new PontoDemanda{ Id = pontoDemanda0.Id, Nome = "Nome Alterado", Endereco = _fakes.Endereco()};
                var pontoDemandaAtualizado = app.Atualizar(usuarioId, pontoDemanda);
                Assert.AreEqual("Nome Alterado", pontoDemandaAtualizado.Nome);
                Assert.AreEqual(_fakes.Endereco().Logradouro, pontoDemandaAtualizado.Endereco.Logradouro);
            }
        }

        [Test]
        public void AdicionarLojaFavoritaNoPontoDemanda()
        {
            var appUsuario = ObterAppUsuario(new UsuarioEF(), new ContratoEF());
            var usuario = appUsuario.Criar(_fakes.Usuario("teste@lojafavorita.com"));
            var app = new PontoDemandaAplicacao(new PontoDemandaEF(), ObterAppUsuario(new UsuarioEF(), new ContratoEF()));
            var pontoDemanda = _fakes.PontoDemanda();
            pontoDemanda.Id = 0;
            pontoDemanda = app.Criar(usuario.Id, pontoDemanda);
            var loja = app.AdicionarLojaFavorita(usuario.Id, pontoDemanda.Id, _fakes.Loja());

            var pontoDemandaComLoja = app.Obter(usuario.Id, pontoDemanda.Id);
            Assert.IsTrue(loja.Id > 0);
            Assert.IsTrue(pontoDemandaComLoja.LojasFavoritas.All(l => l.Id > 0));
        }

        [Test]
        public void RemoverLojaFavoritaDoPontoDemanda()
        {
            using (new TransactionScope())
            {
                var appUsuario = ObterAppUsuario(new UsuarioEF(), new ContratoEF());
                var usuario = appUsuario.Criar(_fakes.Usuario("usuario_remover_loja@mail.com"));
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), ObterAppUsuario(new UsuarioEF(), new ContratoEF()));
                var pontoDemanda = _fakes.PontoDemanda();
                pontoDemanda.Id = 0;
                pontoDemanda = app.Criar(usuario.Id, pontoDemanda);
                app.AdicionarLojaFavorita(usuario.Id, pontoDemanda.Id, _fakes.Loja());

                var pontoDemandaComLoja = app.Obter(usuario.Id, pontoDemanda.Id);
                app.RemoverLojaFavorita(usuario.Id, pontoDemandaComLoja.Id, pontoDemandaComLoja.LojasFavoritas.First().LocalizadorId);

                var pontoDemandaRemovida = app.Obter(usuario.Id, pontoDemanda.Id);
                Assert.IsFalse(pontoDemandaRemovida.LojasFavoritas.Any());
            }
        }

        [Test]
        public void RemoverIntegranteQueNaoPertenceAoPontoDemandaLancaException()
        {
            var pontoDemanda = _fakes.PontoDemanda();
            pontoDemanda.GruposDeIntegrantes=new Collection<GrupoDeIntegrantes>{new GrupoDeIntegrantes{ Integrante = _fakes.Integrante()}};
            _mockRepo.PontoDemanda = pontoDemanda;
            var app = ObterAppPontoDemanda(_mockRepo.GetMockedRepo(), ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo()));
            Assert.Throws<ObjetoNaoEncontradoException>(() => app.RemoverIntegrante(1, 100, 255));
        }

        [Test]
        public void RemoverIntegranteCriadorDoPontoDemandaLancaException()
        {
            var pontoDemanda = _fakes.PontoDemanda();
            var integrante = _fakes.Integrante();
            integrante.Id = 200;
            var usuario = _fakes.Usuario();
            usuario.Id = 1;
            usuario.Integrante = integrante;
            integrante.Usuario = usuario;
            pontoDemanda.UsuarioCriador = usuario;
            pontoDemanda.GruposDeIntegrantes = new Collection<GrupoDeIntegrantes> { new GrupoDeIntegrantes { Integrante = integrante } };
            _mockRepo.PontoDemanda = pontoDemanda;
            var app = ObterAppPontoDemanda(_mockRepo.GetMockedRepo(), ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo()));
            Assert.Throws<ApplicationException>(() => app.RemoverIntegrante(1, 100, 200));
        }

        [Test]
        public void RemoverIntegranteDoPontoDemanda()
        {
            var pontoDemanda = _fakes.PontoDemanda();
            
            var integrante = _fakes.Integrante();
            integrante.Id = 200;
            var usuario = _fakes.Usuario();
            usuario.Id = 1;
            usuario.Integrante = integrante;
            integrante.Usuario = usuario;

            var integrante1 = _fakes.Integrante();
            integrante1.Id = 201;
            var usuario1 = _fakes.Usuario();
            usuario1.Id = 2;
            usuario1.Integrante = integrante1;
            integrante1.Usuario = usuario1;

            pontoDemanda.UsuarioCriador = usuario;
            pontoDemanda.GruposDeIntegrantes = new Collection<GrupoDeIntegrantes> { new GrupoDeIntegrantes { Integrante = integrante }, new GrupoDeIntegrantes { Integrante = integrante1 } };
            _mockRepo.PontoDemanda = pontoDemanda;
            var app = ObterAppPontoDemanda(_mockRepo.GetMockedRepo(), ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo()));
            Assert.AreEqual(2, pontoDemanda.GruposDeIntegrantes.Count);
            app.RemoverIntegrante(1, 100, 201);
            Assert.AreEqual(1, pontoDemanda.GruposDeIntegrantes.Count);
        }

        [Test]
        public void PontoDemandaQueNaoPertenceAoUsuarioLancaException()
        {
            var app = ObterAppPontoDemanda(_mockRepo.GetMockedRepo(), ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo()));
            Assert.Throws<PontoDemandaInvalidoException>(() => app.VerificarPontoDemanda(1, 666));
        }

        [Test]
        public void PontoDemandaQuePertenceAoUsuarioRetornaIdCorreto()
        {
            var app = ObterAppPontoDemanda(_mockRepo.GetMockedRepo(), ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo()));
            var pontoDemandaId = app.VerificarPontoDemanda(1, 101);
            Assert.AreEqual(101, pontoDemandaId);
        }

        [Test]
        public void NaoPodeDesativarSeExistirSomenteUmPontoDeDemanda()
        {
            _mockRepo.PontoDemanda = _fakes.PontoDemanda();
            _mockRepo.PontoDemandaLista = new List<PontoDemanda> {_fakes.PontoDemanda()};
            var app = ObterAppPontoDemanda(_mockRepo.GetMockedRepo(), ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo()));
            Assert.Throws<ApplicationException>(() => app.Desativar(2, 100));
        }

        [Test]
        public void PodeDesativarSeExistirMaisDeUmPontoDeDemanda()
        {
            var pontoDemanda1 = _fakes.PontoDemanda();
            pontoDemanda1.Id = 100;
            var pontoDemanda2 = _fakes.PontoDemanda();
            pontoDemanda2.Id = 101;
            _mockRepo.PontoDemanda = pontoDemanda1;
            _mockRepo.PontoDemandaLista = new List<PontoDemanda> { pontoDemanda1, pontoDemanda2 };

            var app = ObterAppPontoDemanda(_mockRepo.GetMockedRepo(), ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo(), _mockContratoRepo.GetMockedRepo()));
            Assert.IsTrue(app.Listar(2).Single(p => p.Id == 100).Ativo);
            app.Desativar(2, 100);
            Assert.IsFalse(app.Listar(2).Single(p => p.Id == 100).Ativo);
        }

        private static IUsuarioAplicacao ObterAppUsuario(IRepositorioUsuario usuarioRepo, IRepositorioContrato contratoRepo)
        {
            return new UsuarioAplicacao(usuarioRepo, new ContratoAplicacao(contratoRepo), new MockNotificacaoApp().GetMockedApp());
        }

        private IPontoDemandaAplicacao ObterAppPontoDemanda(IRepositorioPontoDemanda pontoDemandaRepo, IUsuarioAplicacao appUsuario)
        {
            return new PontoDemandaAplicacao(pontoDemandaRepo, appUsuario);
        }
    }
}
