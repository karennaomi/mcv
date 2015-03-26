using System.Collections.Generic;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using Moq;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class PontoDemandaAplicacaoTests
    {
        private readonly PontoDemanda _newPontoDemanda = Fakes.PontoDemanda();
        [Test]
        public void CriacaoDoUsuarioDeveDefinirStatusCadastroComoEtapaDeInformacoesPessoaisCompleta()
        {
            var mockAppUsuario = ObterMockAppUsuario();
            var app = ObterPontoDemandaAplicacao(mockAppUsuario.Object);
            var pontoDemanda = app.Criar(_newPontoDemanda);
            mockAppUsuario.Verify(m => m.AtualizaStatusCadastro(9999, StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta, pontoDemanda.Id));
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

        private IPontoDemandaAplicacao ObterPontoDemandaAplicacao(IUsuarioAplicacao appUsuario)
        {
            return new PontoDemandaAplicacao(ObterPontoDemandaRepo(), appUsuario, 9999);
        }

        private IRepositorioPontoDemanda ObterPontoDemandaRepo()
        {
            var repoMock = new Mock<IRepositorioPontoDemanda>();
            repoMock.Setup(r => r.Salvar(_newPontoDemanda)).Returns<PontoDemanda>(x => x);
            repoMock.Setup(r => r.Listar(It.IsAny<long>())).Returns(new List<PontoDemanda>
            {
                new PontoDemanda { Id = 1 }, new PontoDemanda { Id = 2 }
            
            });
            return repoMock.Object;
        }

        private static Mock<IUsuarioAplicacao> ObterMockAppUsuario()
        {
            var repoMock = new Mock<IUsuarioAplicacao>();
            repoMock.Setup(r => r.Obter(It.IsAny<int>())).Returns(Fakes.Usuario());
            return repoMock;
        }
    }
}
