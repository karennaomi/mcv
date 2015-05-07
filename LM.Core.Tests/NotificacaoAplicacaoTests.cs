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
            var appNotificacao = new NotificacaoAplicacao(restService,  new TemplateMensagemAplicacao(new TemplateMensagemEF()));
            appNotificacao.NotificarIntegrantesDoPontoDamanda(new Usuario{ Id = 6 }, new PontoDemanda { Id = 17 } , TipoTemplateMensagem.AtivarCompra, "compras");
        }

        [Test]
        public void EnviaNotificacaoParaIntegrantes()
        {
            var mockRestService = GetMockRestService();
            var appNotificacao = GetAppNotificacao(mockRestService.Object);
            var pontoDemanda = Fakes.PontoDemanda();
            var integrante = new Integrante {Usuario = new Usuario {Id = 7}};
            integrante.Usuario.Integrante = integrante;
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(integrante);
            appNotificacao.NotificarIntegrantesDoPontoDamanda(new Usuario { Id = 6, Integrante = new Integrante{Nome = "John Bililis"}} , pontoDemanda, TipoTemplateMensagem.AtivarCompra, "compras");
            mockRestService.Verify(r => r.Post("sendpushmessage", It.IsAny<object>()), Times.Once);
        }

        [Test]
        public void EnviaNotificacaoParaUsuarioEmCompraAtiva()
        {
            var mockRestService = GetMockRestService();
            var appNotificacao = GetAppNotificacao(mockRestService.Object);
            appNotificacao.Notificar(new Usuario { Id = 6, Integrante = new Integrante { Nome = "John Bililis" } }, new Usuario { Id = 7, Integrante = new Integrante { Nome = "John Bkaasa" } }, new PontoDemanda { Id = 17 }, TipoTemplateMensagem.PedidoItemCriado, "compras");
            mockRestService.Verify(r => r.Post("sendpushmessage", It.IsAny<object>()), Times.Once);
        }

        private static INotificacaoAplicacao GetAppNotificacao(IServicoRest restService)
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
                    new Integrante { Nome = "Joe Doe", Usuario = new Usuario {Id = 6}},
                    new Integrante { Nome = "John Armless", Usuario = new Usuario {Id = 10, DeviceType = "apple", DeviceId = "CE3BA6E02D7F4AEDA33DCB31B3F3A9DE"}},
                } }
            });

            return new PontoDemandaAplicacao(mockPontoDemandaRepo.Object, new Mock<IUsuarioAplicacao>().Object);
        }

        private static Mock<IServicoRest> GetMockRestService()
        {
            return new Mock<IServicoRest>();
        }
    }
}
