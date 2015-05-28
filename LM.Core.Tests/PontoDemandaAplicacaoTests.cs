using System.Collections.Generic;
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
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
            _mockRepo = new MockPontoDemandaRepo();
            _mockUsuarioRepo = new MockUsuarioRepo();
        }

        [Test]
        public void CriarPontoDemanda()
        {
            using (new TransactionScope())
            {
                var appUsuario = ObterAppUsuario(new UsuarioEF());
                var usuario = appUsuario.Criar(_fakes.Usuario());
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), ObterAppUsuario(new UsuarioEF()));
                var pontoDemanda = _fakes.PontoDemanda();
                pontoDemanda.Id = 0;
                app.Criar(usuario.Id, pontoDemanda);
                var pontoDemandaNovo = app.Obter(usuario.Id, pontoDemanda.Id);
                Assert.IsTrue(pontoDemandaNovo.Id > 0);
                Assert.IsTrue(pontoDemandaNovo.Listas.Any());
            }
        }

        [Test]
        public void CriarPontoDemandaComIdDeCidade()
        {
            using (new TransactionScope())
            {
                var appUsuario = ObterAppUsuario(new UsuarioEF());
                var usuario = appUsuario.Criar(_fakes.Usuario());
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), ObterAppUsuario(new UsuarioEF()));
                var pontoDemanda = _fakes.PontoDemanda();
                pontoDemanda.Id = 0;
                pontoDemanda.Endereco.Cidade = new Cidade { Id = 159 };
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
                var appUsuario = ObterAppUsuario(new UsuarioEF());
                var usuario = appUsuario.Criar(_fakes.Usuario());
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), ObterAppUsuario(new UsuarioEF()));
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

                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), ObterAppUsuario(new UsuarioEF()));
                var pontoDemanda = new PontoDemanda{ Id = pontoDemanda0.Id, Nome = "Nome Alterado", Endereco = _fakes.Endereco()};
                var pontoDemandaAtualizado = app.Atualizar(usuarioId, pontoDemanda);
                Assert.AreEqual("Nome Alterado", pontoDemandaAtualizado.Nome);
                Assert.AreEqual(_fakes.Endereco().Logradouro, pontoDemandaAtualizado.Endereco.Logradouro);
            }
        }

        [Test]
        public void Frequencia1DefineReposicao7Estoque3()
        {
            _mockRepo.PontoDemanda = _fakes.PontoDemanda();
            var usuario = _fakes.Usuario();
            usuario.Id = 1;
            _mockUsuarioRepo.Usuario = usuario;
            var app = ObterAppUsuarioPontoDemanda(_mockRepo.GetMockedRepo(), ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo()));
            var pontoDemanda = app.DefinirFrequenciaDeCompra(1, 100, 1);
            Assert.AreEqual(3, pontoDemanda.QuantidadeDiasCoberturaEstoque);
            Assert.AreEqual(7, pontoDemanda.QuantidadeDiasAlertaReposicao);
            Assert.AreEqual(StatusCadastro.FrequenciaDeCompraCompleta, usuario.StatusAtual());
        }

        [Test]
        public void Frequencia2DefineReposicao14Estoque3()
        {
            _mockRepo.PontoDemanda = _fakes.PontoDemanda();
            var usuario = _fakes.Usuario();
            usuario.Id = 1;
            _mockUsuarioRepo.Usuario = usuario;
            var app = ObterAppUsuarioPontoDemanda(_mockRepo.GetMockedRepo(), ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo()));
            var pontoDemanda = app.DefinirFrequenciaDeCompra(1, 100, 2);
            Assert.AreEqual(3, pontoDemanda.QuantidadeDiasCoberturaEstoque);
            Assert.AreEqual(14, pontoDemanda.QuantidadeDiasAlertaReposicao);
            Assert.AreEqual(StatusCadastro.FrequenciaDeCompraCompleta, usuario.StatusAtual());
        }

        [Test]
        public void Frequencia3DefineReposicao28Estoque3()
        {
            _mockRepo.PontoDemanda = _fakes.PontoDemanda();
            var usuario = _fakes.Usuario();
            usuario.Id = 1;
            _mockUsuarioRepo.Usuario = usuario;
            var app = ObterAppUsuarioPontoDemanda(_mockRepo.GetMockedRepo(), ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo()));
            var pontoDemanda = app.DefinirFrequenciaDeCompra(1, 100, 3);
            Assert.AreEqual(3, pontoDemanda.QuantidadeDiasCoberturaEstoque);
            Assert.AreEqual(28, pontoDemanda.QuantidadeDiasAlertaReposicao);
            Assert.AreEqual(StatusCadastro.FrequenciaDeCompraCompleta, usuario.StatusAtual());
        }

        [Test]
        public void AdicionarLojaFavoritaNoPontoDemanda()
        {
            using (new TransactionScope())
            {
                var appUsuario = ObterAppUsuario(new UsuarioEF());
                var usuario = appUsuario.Criar(_fakes.Usuario());
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), ObterAppUsuario(new UsuarioEF()));
                var pontoDemanda = _fakes.PontoDemanda();
                pontoDemanda.Id = 0;
                pontoDemanda = app.Criar(usuario.Id, pontoDemanda);
                var loja = app.AdicionarLojaFavorita(usuario.Id, pontoDemanda.Id, _fakes.Loja());

                var pontoDemandaComLoja = app.Obter(usuario.Id, pontoDemanda.Id);
                Assert.IsTrue(loja.Id > 0);
                Assert.IsTrue(pontoDemandaComLoja.LojasFavoritas.All(l => l.Id > 0));
            }
        }

        [Test]
        public void RemoverLojaFavoritaNoPontoDemanda()
        {
            using (new TransactionScope())
            {
                var appUsuario = ObterAppUsuario(new UsuarioEF());
                var usuario = appUsuario.Criar(_fakes.Usuario());
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), ObterAppUsuario(new UsuarioEF()));
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
        public void PontoDemandaQueNaoPertenceAoUsuarioLancaException()
        {
            var app = ObterAppUsuarioPontoDemanda(_mockRepo.GetMockedRepo(), ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo()));
            Assert.Throws<PontoDemandaInvalidoException>(() => app.VerificarPontoDemanda(1, 666));
        }

        [Test]
        public void PontoDemandaQuePertenceAoUsuarioRetornaIdCorreto()
        {
            var app = ObterAppUsuarioPontoDemanda(_mockRepo.GetMockedRepo(), ObterAppUsuario(_mockUsuarioRepo.GetMockedRepo()));
            var pontoDemandaId = app.VerificarPontoDemanda(1, 101);
            Assert.AreEqual(101, pontoDemandaId);
        }

        private static IUsuarioAplicacao ObterAppUsuario(IRepositorioUsuario usuarioRepo)
        {
            return new UsuarioAplicacao(usuarioRepo);
        }

        private IPontoDemandaAplicacao ObterAppUsuarioPontoDemanda(IRepositorioPontoDemanda pontoDemandaRepo, IUsuarioAplicacao appUsuario)
        {
            return new PontoDemandaAplicacao(pontoDemandaRepo, appUsuario);
        }
    }
}
