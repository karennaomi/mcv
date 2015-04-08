using System;
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
            var uow = new UnitOfWorkEF();
            var appPontoDemanda = new PontoDemandaAplicacao(new PontoDemandaEF(uow),
                new UsuarioAplicacao(new UsuarioEF(uow), new PersonaAplicacao(new PersonaEF())),
                new CidadeAplicacao(new CidadeEF()));
            var appNotificacao = new NotificacaoAplicacao(restService, appPontoDemanda, new TemplateMensagemAplicacao(new TemplateMensagemEF()));
            appNotificacao.NotificarIntegrantesDoPontoDamanda(6, 17, TipoTemplateMensagem.AtivarCompra, "compras");
        }

        [Test]
        public void EnviaNotificacaoParaIntegrantes()
        {
            var mockRestService = GetMockRestService();
            var appNotificacao = GetAppNotificacao(mockRestService.Object);
            appNotificacao.NotificarIntegrantesDoPontoDamanda(6, 17, TipoTemplateMensagem.AtivarCompra, "compras");
            mockRestService.Verify(r => r.Post("sendpushmessage", It.IsAny<object>()), Times.Once);
        }

        private static INotificacaoAplicacao GetAppNotificacao(IRestService restService)
        {
            return new NotificacaoAplicacao(restService, GetPontoDemandaApp(), GetTemplateMensagemApp());
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
            var mockUsuarioApp = new Mock<IUsuarioAplicacao>();
            var mockCidadeApp= new Mock<ICidadeAplicacao>();

            return new PontoDemandaAplicacao(mockPontoDemandaRepo.Object, mockUsuarioApp.Object, mockCidadeApp.Object);
        }

        private static Mock<IRestService> GetMockRestService()
        {
            return new Mock<IRestService>();
        }
    }
}
