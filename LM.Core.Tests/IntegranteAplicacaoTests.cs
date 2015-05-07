using System.Data.Entity.Validation;
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
    public class IntegranteAplicacaoTests
    {
        [Test]
        public void CriarIntegrante()
        {
            using (new TransactionScope())
            {
                var integrante = Fakes.Integrante(15, "M", 1);
                var app = new IntegranteAplicacao(new IntegranteEF());
                integrante = app.Criar(integrante);
                Assert.IsTrue(integrante.Id > 0);
            }
        }

        [Test]
        public void CriarIntegrantePet()
        {
            using (new TransactionScope())
            {
                var integrante = new Integrante { Nome = "Bidu", Tipo = TipoIntegrante.Pet, GrupoDeIntegrantes = new GrupoDeIntegrantes { Id = 1 }, };
                var app = new IntegranteAplicacao(new IntegranteEF());
                try
                {
                    integrante = app.Criar(integrante);
                }
                catch (DbEntityValidationException ex)
                {
                    throw ex;
                }
                
                Assert.IsTrue(integrante.Id > 0);
            }
        }

        [Test]
        public void AtualizarIntegrante()
        {
            using (new TransactionScope())
            {
                var integrante = Fakes.Integrante(15, "M", 1);
                integrante.Id = 9;
                var app = new IntegranteAplicacao(new IntegranteEF());
                integrante = app.Atualizar(integrante);
                Assert.IsTrue(integrante.Id > 0);
            }
        }

        [Test]
        public void NãoPodeAtualizarIntegranteComEmailJaExistente()
        {
            using (new TransactionScope())
            {
                var integrante = Fakes.Integrante(15, "M", 1);
                integrante.Id = 9;
                integrante.Email = "johndoe@mail.com";
                var app = new IntegranteAplicacao(new IntegranteEF());
                Assert.Throws<IntegranteExistenteException>(() => app.Atualizar(integrante));
            }
        }

        [Test]
        public void NaoPodeApagarUmIntegranteQueNaoPertenceAoPonteDemandaEspecificado()
        {
            var app = ObterAppIntegrante(ObterIntegranteRepo().Object);
            Assert.Throws<IntegranteNaoPertenceAPontoDemandaException>(() => app.Apagar(9999, 123));
        }

        [Test]
        public void PodeApagarUmIntegranteQuePertenceAoPonteDemandaEspecificado()
        {
            var repoMock = ObterIntegranteRepo();
            var app = ObterAppIntegrante(repoMock.Object);
            app.Apagar(666, 1234);
            repoMock.Verify(r => r.Apagar(1234), Times.Once);
        }

        private static IIntegranteAplicacao ObterAppIntegrante(IRepositorioIntegrante repo)
        {
            return new IntegranteAplicacao(repo);
        }

        private static Mock<IRepositorioIntegrante> ObterIntegranteRepo()
        {
            var repoMock = new Mock<IRepositorioIntegrante>();
            repoMock.Setup(r => r.Obter(It.IsAny<long>())).Returns(Fakes.Integrante(15, "M", 1));
            repoMock.Setup(r => r.Criar(It.IsAny<Integrante>())).Returns<Integrante>(x => x);
            return repoMock;
        }
    }
}
