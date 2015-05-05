using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using LM.Core.Domain.Servicos;
using LM.Core.RepositorioEF;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
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
            var mockRestService = GetMockRestService();
            GetMockedApp(mockRestService.Object).AtivarCompra(UsuarioId, PontoDemandaId);
            mockRestService.Verify(v => v.Post(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(2));
        }

        [Test]
        public void LancaExcecaoQuandoNaoExisteCompraAtivaETentaFinalizar()
        {
            Assert.Throws<ApplicationException>(() => GetMockedApp(GetMockRestService().Object).FinalizarCompra(UsuarioId, 666));
        }

        private static ICompraAtivaAplicacao GetApp(INotificacaoAplicacao appNotificacao)
        {
            return new CompraAtivaAplicacao(new CompraAtivaEF(), appNotificacao);
        }

        private static ICompraAtivaAplicacao GetMockedApp(IServicoRest restService)
        {
            var pontoDemanda = Fakes.PontoDemanda();
            var integrante1 = new Integrante {Nome = "Joe Doe", Usuario = new Usuario {Id = 7}};
            integrante1.Usuario.Integrante = integrante1;
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(integrante1);

            var integrante2 = new Integrante { Nome = "Joe Doe 2", Usuario = new Usuario { Id = 9 } };
            integrante2.Usuario.Integrante = integrante2;
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(integrante2);
            var mockRepo = new Mock<IRepositorioCompraAtiva>();
            mockRepo.Setup(r => r.AtivarCompra(It.IsAny<long>(), It.IsAny<long>())).Returns(new CompraAtiva
            {
                PontoDemanda = pontoDemanda,
                Usuario = pontoDemanda.GrupoDeIntegrantes.Integrantes.Single(i => i.Usuario.Id == 6).Usuario
            });
            mockRepo.Setup(r => r.FinalizarCompra(It.IsAny<long>(), It.IsAny<long>())).Throws<ApplicationException>();
            return new CompraAtivaAplicacao(mockRepo.Object, GetNotificacaoApp(restService));
        }
        
        private static INotificacaoAplicacao GetNotificacaoApp(IServicoRest restService)
        {
            return new NotificacaoAplicacao(restService, new TemplateMensagemAplicacao(new TemplateMensagemEF()));
        }

        private static Mock<IServicoRest> GetMockRestService()
        {
            return new Mock<IServicoRest>();
        }

        private static IPontoDemandaAplicacao GetPontoDemandaApp()
        {
            var pontoDemanda = Fakes.PontoDemanda();
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Clear();
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(new Integrante { Nome = "Joe Doe", Usuario = new Usuario { Id = 1, DeviceId = "hb7b328723u", DeviceType = "google" } });
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(new Integrante { Nome = "Billy the Kid", Usuario = new Usuario { Id = 2, DeviceId = "sk98023ndds", DeviceType = "apple" } });
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(new Integrante { Nome = "Tripa Seca", Usuario = new Usuario { Id = UsuarioId, DeviceId = "sfnjk823jkj", DeviceType = "google" } });
            var mockApp = new Mock<IPontoDemandaAplicacao>();
            mockApp.Setup(a => a.Obter(It.IsAny<long>(), It.IsAny<long>())).Returns(pontoDemanda);
            return mockApp.Object;
        }
    }
}
