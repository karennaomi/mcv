using System.Collections.ObjectModel;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using LM.Core.Domain.Servicos;
using LM.Core.RepositorioEF;
using Moq;
using NUnit.Framework;
using System;
using System.Transactions;

namespace LM.Core.Tests
{
    [TestFixture]
    public class CompraAtivaAplicacaoTests
    {
        private Fakes _fakes;
        private MockCompraAtivaRepo _mockRepo;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
            _mockRepo = new MockCompraAtivaRepo();
        }

        [Test]
        public void CriaUmaCompraAtiva()
        {
            using (new TransactionScope())
            {
                var compraAtiva = ObterAppCompraAtiva(new CompraAtivaEF(), new Mock<IServicoRest>().Object).AtivarCompra(3, 1);
                Assert.IsTrue(compraAtiva.Id > 0);
            }
        }

        [Test]
        public void FinalizaUmaCompraAtiva()
        {
            _mockRepo.CompraAtiva = _fakes.CompraAtiva();
            var compraAtiva = ObterAppCompraAtiva(_mockRepo.GetMockedRepo(), new Mock<IServicoRest>().Object).DesativarCompra(1, 100);
            Assert.IsNotNull(compraAtiva.FimCompra);
        }

        [Test]
        public void NaoPodeCriarUmaCompraAtivaSeJaExistirUmaNoPontoDeDemanda()
        {
            _mockRepo.CompraAtiva = _fakes.CompraAtiva();
            var ex = Assert.Throws<ApplicationException>(() => ObterAppCompraAtiva(_mockRepo.GetMockedRepo(), new Mock<IServicoRest>().Object).AtivarCompra(1, 100));
            Assert.AreEqual("Já existe uma compra ativa neste ponto de demanda.", ex.Message);
        }

        [Test]
        public void AtivarCompraNotificaUsuarios()
        {
            var servicoRestMock = new Mock<IServicoRest>();
            var compraAtiva = _fakes.CompraAtiva();

            compraAtiva.PontoDemanda.GruposDeIntegrantes = new Collection<GrupoDeIntegrantes>();
            
            var integrante0 = _fakes.Integrante();
            integrante0.Id = 200;
            integrante0.Usuario = new Usuario { Id = 1 };
            compraAtiva.Usuario.Integrante = integrante0;
            compraAtiva.PontoDemanda.GruposDeIntegrantes.Add(new GrupoDeIntegrantes { Integrante = integrante0});
            
            var integrante1 = _fakes.Integrante();
            integrante1.Id = 201;
            integrante1.Usuario = new Usuario { Id = 2 };
            compraAtiva.PontoDemanda.GruposDeIntegrantes.Add(new GrupoDeIntegrantes { Integrante = integrante1 });
            
            var integrante2 = _fakes.Integrante();
            integrante2.Id = 202;
            integrante2.Usuario = new Usuario { Id = 3 };
            compraAtiva.PontoDemanda.GruposDeIntegrantes.Add(new GrupoDeIntegrantes { Integrante = integrante2 });
            
            _mockRepo.CompraAtiva = compraAtiva;
            ObterAppCompraAtiva(_mockRepo.GetMockedRepo(), servicoRestMock.Object).AtivarCompra(1, 101);
            servicoRestMock.Verify(v => v.Post(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(2));
        }

        private static ICompraAtivaAplicacao ObterAppCompraAtiva(IRepositorioCompraAtiva compraAtivaRepo, IServicoRest servicoRest)
        {
            var mockTemplateMensagemRepo = new MockTemplateMessageRepo {Tipo = "push"};
            var notificacaoApp = new NotificacaoAplicacao(servicoRest, new TemplateMensagemAplicacao(mockTemplateMensagemRepo.GetMockedRepo()), new Mock<IFilaItemAplicacao>().Object);
            return new CompraAtivaAplicacao(compraAtivaRepo, notificacaoApp);
        }
    }
}
