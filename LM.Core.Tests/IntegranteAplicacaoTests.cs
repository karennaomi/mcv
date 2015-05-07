using System;
using System.Data.Entity.Validation;
using System.Runtime.InteropServices;
using System.Transactions;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using LM.Core.RepositorioEF;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

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
                integrante.GrupoDeIntegrantes.PontosDemanda = null;
                var app = new IntegranteAplicacao(new IntegranteEF(), new NotificacaoAplicacao(null, new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF())));
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
                var app = new IntegranteAplicacao(new IntegranteEF(), new NotificacaoAplicacao(null, new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF())));
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
                var app = new IntegranteAplicacao(new IntegranteEF(), new NotificacaoAplicacao(null, new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF())));
                integrante = app.Atualizar(17, integrante);
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
                var app = new IntegranteAplicacao(new IntegranteEF(), new NotificacaoAplicacao(null, new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF())));
                Assert.Throws<IntegranteExistenteException>(() => app.Atualizar(17, integrante));
            }
        }

        [Test]
        public void NãoPodeAtualizarIntegranteQueNaoPertenceAoPontoDeDemanda()
        {
            using (new TransactionScope())
            {
                var integrante = Fakes.Integrante(15, "M", 1);
                integrante.Id = 9;
                integrante.Email = "johndoe@mail.com";
                var app = new IntegranteAplicacao(new IntegranteEF(), new NotificacaoAplicacao(null, new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF())));
                Assert.Throws<IntegranteNaoPertenceAPontoDemandaException>(() => app.Atualizar(9876, integrante));
            }
        }

        [Test]
        public void NaoPodeApagarUmIntegranteQueNaoPertenceAoPonteDemandaEspecificado()
        {
            var app = ObterAppIntegrante(ObterIntegranteRepo(Fakes.Integrante(15, "M", 1)).Object);
            Assert.Throws<IntegranteNaoPertenceAPontoDemandaException>(() => app.Apagar(9999, 123));
        }

        [Test]
        public void PodeApagarUmIntegranteQuePertenceAoPonteDemandaEspecificado()
        {
            var integrante = Fakes.Integrante(15, "M", 1);
            var repoMock = ObterIntegranteRepo(integrante);
            var app = ObterAppIntegrante(repoMock.Object);
            app.Apagar(666, 1234);
            repoMock.Verify(r => r.Apagar(integrante), Times.Once);
        }

        [Test]
        public void ConvidarIntegrante()
        {
            using (new TransactionScope())
            {
                var app = ObterAppIntegrante(new IntegranteEF());
                app.Convidar(20, 49, 53);
                var convidado = app.Obter(20, 53);
                Assert.IsTrue(convidado.DataConvite.Value.Date == DateTime.Now.Date);
            }
        }

        private static IIntegranteAplicacao ObterAppIntegrante(IRepositorioIntegrante repo)
        {
            return new IntegranteAplicacao(repo, new NotificacaoAplicacao(null, new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF())));
        }

        private static Mock<IRepositorioIntegrante> ObterIntegranteRepo(Integrante integrante)
        {
            var repoMock = new Mock<IRepositorioIntegrante>();
            repoMock.Setup(r => r.Obter(It.IsAny<long>())).Returns(integrante);
            repoMock.Setup(r => r.Criar(It.IsAny<Integrante>())).Returns<Integrante>(x => x);
            return repoMock;
        }
    }
}
