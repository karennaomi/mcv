using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly PontoDemanda _newPontoDemanda = Fakes.PontoDemanda();

        [Test]
        public void CriarPontoDemanda()
        {
            using (new TransactionScope())
            {
                var unitOfWork = new UnitOfWorkEF();
                var appUsuario = new UsuarioAplicacao(new UsuarioEF(unitOfWork), new PersonaAplicacao(new PersonaEF()));
                var usuario = appUsuario.Criar(Fakes.Usuario());
                var app = new PontoDemandaAplicacao(new PontoDemandaEF(unitOfWork), appUsuario, new CidadeAplicacao(new CidadeEF()), usuario.Id);
                app.Criar(_newPontoDemanda);
            }
        }

        [Test]
        public void CriacaoDoPontoDemandaDeveDefinirStatusCadastroComoEtapaDeInformacoesDoPontoDeDemandaCompleta()
        {
            var mockAppUsuario = ObterMockAppUsuario();
            var app = ObterPontoDemandaAplicacao(mockAppUsuario.Object);
            var pontoDemanda = app.Criar(_newPontoDemanda);
            mockAppUsuario.Verify(m => m.AtualizarStatusCadastro(9999, StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta, pontoDemanda.Id));
        }

        [Test]
        public void PontoDemandaQueNaoPertenceAoUsuarioLancaException()
        {
            var app = ObterPontoDemandaAplicacao(ObterMockAppUsuario().Object);
            Assert.Throws<PontoDemandaInvalidoException>(() => app.VerificarPontoDemanda(3));
        }

        [Test]
        public void PontoDemandaQuePertenceAoUsuarioRetornaIdCorreto()
        {
            var app = ObterPontoDemandaAplicacao(ObterMockAppUsuario().Object);
            var pontoDemandaId = app.VerificarPontoDemanda(2);
            Assert.AreEqual(2, pontoDemandaId);
        }

        [Test]
        public void Frequencia1DefineReposicao7Estoque3()
        {
            var app = ObterPontoDemandaAplicacao(ObterMockAppUsuario().Object);
            var pontoDemanda = app.DefinirFrequenciaDeConsumo(999, 1);
            Assert.AreEqual(3, pontoDemanda.QuantidadeDiasCoberturaEstoque);
            Assert.AreEqual(7, pontoDemanda.QuantidadeDiasAlertaReposicao);
        }

        [Test]
        public void Frequencia2DefineReposicao14Estoque3()
        {
            var app = ObterPontoDemandaAplicacao(ObterMockAppUsuario().Object);
            var pontoDemanda = app.DefinirFrequenciaDeConsumo(999, 2);
            Assert.AreEqual(3, pontoDemanda.QuantidadeDiasCoberturaEstoque);
            Assert.AreEqual(14, pontoDemanda.QuantidadeDiasAlertaReposicao);
        }

        [Test]
        public void Frequencia3DefineReposicao28Estoque3()
        {
            var app = ObterPontoDemandaAplicacao(ObterMockAppUsuario().Object);
            var pontoDemanda = app.DefinirFrequenciaDeConsumo(999, 3);
            Assert.AreEqual(3, pontoDemanda.QuantidadeDiasCoberturaEstoque);
            Assert.AreEqual(28, pontoDemanda.QuantidadeDiasAlertaReposicao);
        }

        private IPontoDemandaAplicacao ObterPontoDemandaAplicacao(IUsuarioAplicacao appUsuario)
        {
            return new PontoDemandaAplicacao(ObterPontoDemandaRepo(), appUsuario, new CidadeAplicacao(new CidadeEF()), 9999);
        }

        private IRepositorioPontoDemanda ObterPontoDemandaRepo()
        {
            var repoMock = new Mock<IRepositorioPontoDemanda>();
            repoMock.Setup(r => r.Obter(It.IsAny<long>(), It.IsAny<long>())).Returns(Fakes.PontoDemanda());
            repoMock.Setup(r => r.Criar(_newPontoDemanda)).Returns<PontoDemanda>(x => x);
            repoMock.Setup(r => r.Listar(It.IsAny<long>())).Returns(new List<PontoDemanda>
            {
                new PontoDemanda { Id = 1 }, new PontoDemanda { Id = 2 }
            
            });
            return repoMock.Object;
        }

        private static Mock<IUsuarioAplicacao> ObterMockAppUsuario()
        {
            var integrante = Fakes.Integrante(30, "M");
            integrante.GrupoDeIntegrantes = new GrupoDeIntegrantes();
            integrante.Usuario.MapIntegrantes = new Collection<Integrante> { integrante };
            var repoMock = new Mock<IUsuarioAplicacao>();
            repoMock.Setup(r => r.Obter(It.IsAny<long>())).Returns(integrante.Usuario);
            return repoMock;
        }
    }
}
