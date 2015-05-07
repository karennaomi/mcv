using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using LM.Core.Domain.Servicos;
using LM.Core.RepositorioEF;
using Moq;
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace LM.Core.Tests
{
    [TestFixture]
    public class NotificacaoAplicacaoTests
    {
        [Test]
        [Ignore]
        public void EnviaNotificacao()
        {
            var restService = new RestServiceWithRestSharp("http://localhost:45678");
            var appNotificacao = new NotificacaoAplicacao(restService,  new TemplateMensagemAplicacao(new TemplateMensagemEF()), GetFilaItemAplicacao());
            appNotificacao.NotificarIntegrantesDoPontoDamanda(new Integrante { Id = 6, Usuario = new Usuario{Id=6}}, new PontoDemanda { Id = 17 }, TipoTemplateMensagem.AtivarCompra, new { Action = "compras" });
        }

        [Test]
        public void EnviaNotificacaoParaIntegrantes()
        {
            var mockRestService = GetMockRestService();
            var appNotificacao = GetAppNotificacao(mockRestService.Object);
            var pontoDemanda = Fakes.PontoDemanda();
            appNotificacao.NotificarIntegrantesDoPontoDamanda(new Integrante { Id = 6, Nome = "John Bililis", Usuario = new Usuario {Id = 444}}, pontoDemanda, TipoTemplateMensagem.AtivarCompra, new { Action = "compras" });
            mockRestService.Verify(r => r.Post("sendpushmessage", It.IsAny<object>()), Times.Once);
        }

        [Test]
        public void EnviaNotificacaoParaUsuarioEmCompraAtiva()
        {
            var mockRestService = GetMockRestService();
            var appNotificacao = GetAppNotificacao(mockRestService.Object);
            appNotificacao.Notificar(new Integrante { Id = 6, Nome = "John Bililis", Usuario = new Usuario { Id = 6 } }, new Integrante { Nome = "John Bkaasa", Usuario = new Usuario { Id = 7 } }, new PontoDemanda { Id = 17 }, TipoTemplateMensagem.PedidoItemCriado, new { Action = "compras" });
            mockRestService.Verify(r => r.Post("sendpushmessage", It.IsAny<object>()), Times.Once);
        }

        private static INotificacaoAplicacao GetAppNotificacao(IServicoRest restService)
        {
            return new NotificacaoAplicacao(restService, GetTemplateMensagemApp(), GetFilaItemAplicacao());
        }

        private static ITemplateMensagemAplicacao GetTemplateMensagemApp()
        {
            var mockTemplateMensagemApp = new Mock<ITemplateMensagemAplicacao>();
            mockTemplateMensagemApp.Setup(t => t.ObterPorTipoTemplate(It.IsAny<TipoTemplateMensagem>()))
                .Returns(new TemplateMensagemPush
                {
                    Mensagem = "{PontoDemanda.Nome} {Remetente.Integrante.Nome} {Destinatario.Integrante.Nome}"
                });
            return mockTemplateMensagemApp.Object;
        }

        private static IFilaItemAplicacao GetFilaItemAplicacao()
        {
            var mockFilaItemApp = new Mock<IFilaItemAplicacao>();
            return mockFilaItemApp.Object;
        }

        private static Mock<IServicoRest> GetMockRestService()
        {
            return new Mock<IServicoRest>();
        }
    }
}
