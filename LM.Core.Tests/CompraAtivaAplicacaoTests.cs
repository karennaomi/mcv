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
        private const int UsuarioId = 6;
        private const int PontoDemandaId = 1;

        private Fakes _fakes;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
        }

        [Test]
        public void CriaUmaCompraAtiva()
        {
            using (new TransactionScope())
            {
                var mockRestService = GetMockRestService();
                var appNotificacao = GetNotificacaoApp(mockRestService.Object);
                var compraAtiva = GetApp(appNotificacao).AtivarCompra(10755, PontoDemandaId);
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
                var compraAtiva = GetApp(appNotificacao).AtivarCompra(10755, PontoDemandaId);
                compraAtiva = GetApp(appNotificacao).FinalizarCompra(10755, PontoDemandaId);
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

        private ICompraAtivaAplicacao GetMockedApp(IServicoRest restService)
        {
            var pontoDemanda = _fakes.PontoDemanda();
            var integrante0 = _fakes.Integrante();
            integrante0.Id = 106;
            integrante0.Usuario = new Usuario { Id = 6, Integrante = integrante0 };
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(integrante0);

            var integrante1 = _fakes.Integrante();
            integrante0.Id = 107;
            integrante1.Nome = "Joe Doe 1";
            integrante1.Usuario = new Usuario {Id = 7, Integrante = integrante1};
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(integrante1);

            var integrante2 = _fakes.Integrante();
            integrante0.Id = 109;
            integrante2.Nome = "Joe Doe 2";
            integrante2.Usuario = new Usuario { Id = 9, Integrante = integrante1 };
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
            return new NotificacaoAplicacao(restService, new TemplateMensagemAplicacao(new TemplateMensagemEF()), null);
        }

        private static Mock<IServicoRest> GetMockRestService()
        {
            return new Mock<IServicoRest>();
        }
    }
}
