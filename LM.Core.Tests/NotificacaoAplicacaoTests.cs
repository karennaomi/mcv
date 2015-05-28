using System.Collections.ObjectModel;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.Servicos;
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
        public void EnviaNotificacaoParaIntegrantes()
        {
            var mockRestService = GetMockRestService();
            var appNotificacao = GetAppNotificacao(mockRestService.Object);
            var pontoDemanda = _fakes.PontoDemanda();
            var integrante0 = _fakes.Integrante();
            integrante0.Id = 106;
            integrante0.Usuario = new Usuario { Id = 6, Integrante = integrante0 };
            pontoDemanda.GruposDeIntegrantes = new Collection<GrupoDeIntegrantes>();
            pontoDemanda.GruposDeIntegrantes.Add(new GrupoDeIntegrantes{ Integrante = integrante0, PontoDemanda = pontoDemanda});

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
            pontoDemanda.GruposDeIntegrantes = new Collection<GrupoDeIntegrantes>();
            pontoDemanda.GruposDeIntegrantes.Add(new GrupoDeIntegrantes { Integrante = integrante0});

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
