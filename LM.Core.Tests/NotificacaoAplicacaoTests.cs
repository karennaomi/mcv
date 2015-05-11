using System.Data.Entity.ModelConfiguration.Conventions;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.Servicos;
using LM.Core.RepositorioEF;
using Moq;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class NotificacaoAplicacaoTests
    {
        private Fakes _fakes;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
        }

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
            var pontoDemanda = _fakes.PontoDemanda();
            var integrante0 = _fakes.Integrante();
            integrante0.Id = 106;
            integrante0.Usuario = new Usuario { Id = 6, Integrante = integrante0 };
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(integrante0);

            var integrante1 = _fakes.Integrante();
            integrante1.Id = 107;
            integrante1.Nome = "John Bililis";
            integrante1.Usuario = new Usuario {Id = 7};

            appNotificacao.NotificarIntegrantesDoPontoDamanda(integrante1, pontoDemanda, TipoTemplateMensagem.AtivarCompra, new { Action = "compras" });
            mockRestService.Verify(r => r.Post("sendpushmessage", It.IsAny<object>()), Times.Once);
        }

        [Test]
        public void EnviaNotificacaoParaUsuarioEmCompraAtiva()
        {
            var mockRestService = GetMockRestService();
            var appNotificacao = GetAppNotificacao(mockRestService.Object);

            var pontoDemanda = _fakes.PontoDemanda();
            var integrante0 = _fakes.Integrante();
            integrante0.Id = 105;
            integrante0.Usuario = new Usuario { Id = 5, Integrante = integrante0 };
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(integrante0);

            var integrante1 = _fakes.Integrante();
            integrante1.Id = 106;
            integrante1.Nome = "John Bililis";
            integrante1.Usuario = new Usuario { Id = 6 };

            var integrante2 = _fakes.Integrante();
            integrante2.Id = 107;
            integrante2.Nome = "John Bkaasa";
            integrante2.Usuario = new Usuario { Id = 7 };

            appNotificacao.Notificar(integrante1, integrante2, pontoDemanda, TipoTemplateMensagem.PedidoItemCriado, new { Action = "compras" });
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
