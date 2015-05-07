using System.Collections.Generic;
using System.Linq;
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
        [Test]
        public void CriarPontoDemanda()
        {
            using (new TransactionScope())
            {
                var appUsuario = GetAppUsuario();
                var usuario = appUsuario.Criar(Fakes.Usuario());
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), GetAppUsuario());
                var pontoDemanda = Fakes.PontoDemanda();
                pontoDemanda.Id = 0;
                app.Criar(usuario.Id, pontoDemanda);
                var pontoDemandaNovo = app.Obter(usuario.Id, pontoDemanda.Id);
                Assert.AreEqual(StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta, pontoDemandaNovo.GrupoDeIntegrantes.Integrantes.Single(i => i.Usuario.Id == usuario.Id).Usuario.StatusAtual());
                Assert.IsTrue(pontoDemandaNovo.Listas.Any());
            }
        }

        [Test]
        public void CriarPontoDemandaComIdDeCidade()
        {
            using (new TransactionScope())
            {
                var appUsuario = GetAppUsuario();
                var usuario = appUsuario.Criar(Fakes.Usuario());
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), GetAppUsuario());
                var pontoDemanda = Fakes.PontoDemanda();
                pontoDemanda.Id = 0;
                pontoDemanda.Endereco.Cidade = new Cidade { Id = 159 };
                app.Criar(usuario.Id, pontoDemanda);
                var pontoDemandaNovo = app.Obter(usuario.Id, pontoDemanda.Id);
                Assert.AreEqual(StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta, pontoDemandaNovo.GrupoDeIntegrantes.Integrantes.Single(i => i.Usuario.Id == usuario.Id).Usuario.StatusAtual());
                Assert.IsTrue(pontoDemandaNovo.Listas.Any());
            }
        }

        [Test]
        public void CriarPontoDemandaComLojasFavoritas()
        {
            using (new TransactionScope())
            {
                var appUsuario = GetAppUsuario();
                var usuario = appUsuario.Criar(Fakes.Usuario());
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), GetAppUsuario());
                var pontoDemanda = Fakes.PontoDemanda();
                pontoDemanda.LojasFavoritas = Fakes.Lojas();
                pontoDemanda.Id = 0;
                app.Criar(usuario.Id, pontoDemanda);
                Assert.AreEqual(StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta, pontoDemanda.GrupoDeIntegrantes.Integrantes.Single(i => i.Usuario.Id == usuario.Id).Usuario.StatusAtual());
                Assert.IsTrue(pontoDemanda.LojasFavoritas.All(l => l.Id > 0));
            }
        }

        [Test]
        public void AdicionarLojaFavoritaNoPontoDemanda()
        {
            using (new TransactionScope())
            {
                var appUsuario = GetAppUsuario();
                var usuario = appUsuario.Criar(Fakes.Usuario());
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), GetAppUsuario());
                var pontoDemanda = Fakes.PontoDemanda();
                pontoDemanda.Id = 0;
                pontoDemanda = app.Criar(usuario.Id, pontoDemanda);
                var loja = app.AdicionarLojaFavorita(usuario.Id, pontoDemanda.Id, Fakes.Lojas().First());

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
                var appUsuario = GetAppUsuario();
                var usuario = appUsuario.Criar(Fakes.Usuario());
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(), GetAppUsuario());
                var pontoDemanda = Fakes.PontoDemanda();
                pontoDemanda.Id = 0;
                pontoDemanda = app.Criar(usuario.Id, pontoDemanda);
                app.AdicionarLojaFavorita(usuario.Id, pontoDemanda.Id, Fakes.Lojas().First());
                
                var pontoDemandaComLoja = app.Obter(usuario.Id, pontoDemanda.Id);
                app.RemoverLojaFavorita(usuario.Id, pontoDemandaComLoja.Id, pontoDemandaComLoja.LojasFavoritas.First().Idlocalizador);

                var pontoDemandaRemovida = app.Obter(usuario.Id, pontoDemanda.Id);
                Assert.IsFalse(pontoDemandaRemovida.LojasFavoritas.Any());
            }
        }

        [Test]
        public void PontoDemandaQueNaoPertenceAoUsuarioLancaException()
        {
            var app = ObterPontoDemandaAplicacao();
            Assert.Throws<PontoDemandaInvalidoException>(() => app.VerificarPontoDemanda(9999, 3));
        }

        [Test]
        public void PontoDemandaQuePertenceAoUsuarioRetornaIdCorreto()
        {
            var app = ObterPontoDemandaAplicacao();
            var pontoDemandaId = app.VerificarPontoDemanda(9999, 2);
            Assert.AreEqual(2, pontoDemandaId);
        }

        [Test]
        public void Frequencia1DefineReposicao7Estoque3()
        {
            var app = ObterPontoDemandaAplicacao();
            var pontoDemanda = app.DefinirFrequenciaDeCompra(9999, 999, 1);
            Assert.AreEqual(3, pontoDemanda.QuantidadeDiasCoberturaEstoque);
            Assert.AreEqual(7, pontoDemanda.QuantidadeDiasAlertaReposicao);
        }

        [Test]
        public void Frequencia2DefineReposicao14Estoque3()
        {
            var app = ObterPontoDemandaAplicacao();
            var pontoDemanda = app.DefinirFrequenciaDeCompra(9999, 999, 2);
            Assert.AreEqual(3, pontoDemanda.QuantidadeDiasCoberturaEstoque);
            Assert.AreEqual(14, pontoDemanda.QuantidadeDiasAlertaReposicao);
        }

        [Test]
        public void Frequencia3DefineReposicao28Estoque3()
        {
            var app = ObterPontoDemandaAplicacao();
            var pontoDemanda = app.DefinirFrequenciaDeCompra(9999, 999, 3);
            Assert.AreEqual(3, pontoDemanda.QuantidadeDiasCoberturaEstoque);
            Assert.AreEqual(28, pontoDemanda.QuantidadeDiasAlertaReposicao);
        }

        private static IUsuarioAplicacao GetAppUsuario()
        {
            return new UsuarioAplicacao(new UsuarioEF());
            
        }

        private static IPontoDemandaAplicacao ObterPontoDemandaAplicacao(PontoDemanda pontoDemanda = null)
        {
            return new PontoDemandaAplicacao(ObterPontoDemandaRepo(pontoDemanda), new Mock<IUsuarioAplicacao>().Object);
        }

        private static IRepositorioPontoDemanda ObterPontoDemandaRepo(PontoDemanda pontoDemanda)
        {
            var repoMock = new Mock<IRepositorioPontoDemanda>();
            repoMock.Setup(r => r.Obter(It.IsAny<long>(), It.IsAny<long>())).Returns(Fakes.PontoDemanda());

            repoMock.Setup(r => r.Criar(9999, pontoDemanda)).Returns<PontoDemanda>(x => { x.Id = 1; return x; });
            repoMock.Setup(r => r.Listar(It.IsAny<long>())).Returns(new List<PontoDemanda>
            {
                new PontoDemanda { Id = 1 }, new PontoDemanda { Id = 2 }
            
            });
            return repoMock.Object;
        }
    }
}
