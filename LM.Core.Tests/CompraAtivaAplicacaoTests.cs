using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
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
        private const int UsuarioId = 3;
        private const int PontoDemandaId = 1;
        [Test]
        public void CriaUmaCompraAtiva()
        {
            using (new TransactionScope())
            {
                var mockRestService = GetMockRestService();
                var appNotificacao = GetNotificacaoApp(mockRestService.Object);
                var compraAtiva = GetApp(appNotificacao).AtivarCompra(UsuarioId, PontoDemandaId);
                Assert.IsTrue(compraAtiva.Id > 0);
            }
        }

        [Test]
        public void FinalizaUmaCompraAtiva()
        {
            using (new TransactionScope())
            {
                var mockRestService = GetMockRestService();
                var appNotificacao = GetNotificacaoApp(mockRestService.Object);
                var compraAtiva = GetApp(appNotificacao).AtivarCompra(UsuarioId, PontoDemandaId);
                compraAtiva = GetApp(appNotificacao).FinalizarCompra(UsuarioId, PontoDemandaId);
                Assert.IsNotNull(compraAtiva.FimCompra);
            }
        }

        [Test]
        public void AtivarCompraNotificaUsuarios()
        {
            using (new TransactionScope())
            {
                var mockRestService = GetMockRestService();
                var appNotificacao = GetNotificacaoApp(mockRestService.Object);
                var compraAtiva = GetApp(appNotificacao).AtivarCompra(UsuarioId, PontoDemandaId);
                mockRestService.Verify(v => v.Post(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(2));
            }
        }

        [Test]
        public void LancaExcecaoQuandoNaoExisteCompraAtivaETentaFinalizar()
        {
            Assert.Throws<ApplicationException>(() => GetMockedApp().FinalizarCompra(UsuarioId, 666));
        }

        private static ICompraAtivaAplicacao GetApp(INotificacaoAplicacao appNotificacao)
        {
            return new CompraAtivaAplicacao(new CompraAtivaEF(), appNotificacao);
        }

        private static ICompraAtivaAplicacao GetMockedApp()
        {
            var mockRepo = new Mock<IRepositorioCompraAtiva>();
            mockRepo.Setup(r => r.FinalizarCompra(It.IsAny<long>(), It.IsAny<long>())).Throws<ApplicationException>();
            return new CompraAtivaAplicacao(mockRepo.Object, GetNotificacaoApp(GetMockRestService().Object));
        }
        
        private static INotificacaoAplicacao GetNotificacaoApp(IRestService restService)
        {
            return new NotificacaoAplicacao(restService, GetPontoDemandaApp(), new TemplateMensagemAplicacao(new TemplateMensagemEF()));
        }

        private static Mock<IRestService> GetMockRestService()
        {
            return new Mock<IRestService>();
        }

        private static IPontoDemandaAplicacao GetPontoDemandaApp()
        {
            var pontoDemanda = Fakes.PontoDemanda();
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Clear();
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(new Integrante { Usuario = new Usuario { Id = 1, Nome = "Joe Doe", DeviceId = "hb7b328723u", DeviceType = "google" } });
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(new Integrante { Usuario = new Usuario { Id = 2, Nome = "Billy the Kid", DeviceId = "sk98023ndds", DeviceType = "apple" } });
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(new Integrante { Usuario = new Usuario { Id = UsuarioId, Nome = "Tripa Seca", DeviceId = "sfnjk823jkj", DeviceType = "google" } });
            var mockApp = new Mock<IPontoDemandaAplicacao>();
            mockApp.Setup(a => a.Obter(It.IsAny<long>(), It.IsAny<long>())).Returns(pontoDemanda);
            return mockApp.Object;
        }
    }
}
