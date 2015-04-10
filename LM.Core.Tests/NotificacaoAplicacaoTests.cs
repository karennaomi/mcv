using System.Collections.ObjectModel;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using LM.Core.RepositorioEF;
using Moq;
using NUnit.Framework;

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
            var appPontoDemanda = new PontoDemandaAplicacao(new PontoDemandaEF());
            var appNotificacao = new NotificacaoAplicacao(restService,  new TemplateMensagemAplicacao(new TemplateMensagemEF()));
            appNotificacao.NotificarIntegrantesDoPontoDamanda(new Usuario{ Id = 6 }, new PontoDemanda { Id = 17 } , TipoTemplateMensagem.AtivarCompra, "compras");
        }

        [Test]
        public void EnviaNotificacaoParaIntegrantes()
        {
            var mockRestService = GetMockRestService();
            var appNotificacao = GetAppNotificacao(mockRestService.Object);
            var pontoDemanda = Fakes.PontoDemanda();
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(new Integrante{ Usuario = new Usuario { Id = 7 }});
            appNotificacao.NotificarIntegrantesDoPontoDamanda(new Usuario { Id = 6 } , pontoDemanda, TipoTemplateMensagem.AtivarCompra, "compras");
            mockRestService.Verify(r => r.Post("sendpushmessage", It.IsAny<object>()), Times.Once);
        }

        [Test]
        public void EnviaNotificacaoParaUsuarioEmCompraAtiva()
        {
            var mockRestService = GetMockRestService();
            var appNotificacao = GetAppNotificacao(mockRestService.Object);
            appNotificacao.Notificar(new Usuario { Id = 6 }, new Usuario { Id = 7 }, new PontoDemanda { Id = 17 }, TipoTemplateMensagem.PedidoItemCriado, "compras");
            mockRestService.Verify(r => r.Post("sendpushmessage", It.IsAny<object>()), Times.Once);
        }

        private static INotificacaoAplicacao GetAppNotificacao(IRestService restService)
        {
            return new NotificacaoAplicacao(restService, GetTemplateMensagemApp());
        }

        private static ITemplateMensagemAplicacao GetTemplateMensagemApp()
        {
            var mockTemplateMensagemApp = new Mock<ITemplateMensagemAplicacao>();
            mockTemplateMensagemApp.Setup(t => t.ObterPorTipo<TemplateMensagemPush>(It.IsAny<TipoTemplateMensagem>()))
                .Returns(new TemplateMensagemPush
                {
                    Mensagem = "{PontoDemanda.Nome} {Remetente.Nome} {Destinatario.Nome}"
                });
            return mockTemplateMensagemApp.Object;
        }

        private static IPontoDemandaAplicacao GetPontoDemandaApp()
        {
            var mockPontoDemandaRepo = new Mock<IRepositorioPontoDemanda>();
            mockPontoDemandaRepo.Setup(p => p.Obter(6, It.IsAny<long>())).Returns(new PontoDemanda
            {
                Nome = "PontoDemandaTeste",
                GrupoDeIntegrantes = new GrupoDeIntegrantes { Integrantes = new Collection<Integrante>
                {
                    new Integrante { Usuario = new Usuario {Id = 6, Nome = "Joe Doe"}},
                    new Integrante { Usuario = new Usuario {Id = 10, Nome = "John Armless", DeviceType = "apple", DeviceId = "CE3BA6E02D7F4AEDA33DCB31B3F3A9DE"}},
                } }
            });

            return new PontoDemandaAplicacao(mockPontoDemandaRepo.Object);
        }

        private static Mock<IRestService> GetMockRestService()
        {
            return new Mock<IRestService>();
        }
    }
}
