﻿using LM.Core.Application;
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
                var compraAtiva = GetApp(GetMockNotificacaoApp().Object).AtivarCompra(UsuarioId, PontoDemandaId);
                Assert.IsTrue(compraAtiva.Id > 0);
            }
        }

        [Test]
        public void FinalizaUmaCompraAtiva()
        {
            using (new TransactionScope())
            {
                var compraAtiva = GetApp(GetMockNotificacaoApp().Object).AtivarCompra(UsuarioId, PontoDemandaId);
                compraAtiva = GetApp(GetMockNotificacaoApp().Object).FinalizarCompra(UsuarioId, PontoDemandaId);
                Assert.IsNotNull(compraAtiva.FimCompra);
            }
        }

        [Test]
        public void AtivarCompraNotificaUsuarios()
        {
            using (new TransactionScope())
            {
                var mockNotificacao = GetMockNotificacaoApp();
                var compraAtiva = GetApp(mockNotificacao.Object).AtivarCompra(UsuarioId, PontoDemandaId);
                mockNotificacao.Verify(v => v.EnviarNotificacao(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            }
        }

        [Test]
        public void LancaExcecaoQuandoNaoExisteCompraAtivaETentaFinalizar()
        {
            Assert.Throws<ApplicationException>(() => GetMockedApp().FinalizarCompra(9999, 9999));
        }

        private static ICompraAtivaAplicacao GetApp(INotificacaoAplicacao appNotificacao)
        {
            return new CompraAtivaAplicacao(new CompraAtivaEF(), GetPontoDemandaApp(), appNotificacao);
        }

        private static ICompraAtivaAplicacao GetMockedApp()
        {
            var mockRepo = new Mock<IRepositorioCompraAtiva>();
            return new CompraAtivaAplicacao(mockRepo.Object, GetPontoDemandaApp(), GetMockNotificacaoApp().Object);
        }
        
        private static IPontoDemandaAplicacao GetPontoDemandaApp()
        {
            var pontoDemanda = Fakes.PontoDemanda();
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Clear();
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(new Integrante{Usuario = new Usuario{Id = 1, DeviceId = "hb7b328723u", DeviceType = "google"}});
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(new Integrante{Usuario = new Usuario{Id = 2, DeviceId = "sk98023ndds", DeviceType = "apple"}});
            pontoDemanda.GrupoDeIntegrantes.Integrantes.Add(new Integrante { Usuario = new Usuario { Id = UsuarioId, DeviceId = "sfnjk823jkj", DeviceType = "google" } });
            var mockApp = new Mock<IPontoDemandaAplicacao>();
            mockApp.Setup(a => a.Obter(It.IsAny<long>(), It.IsAny<long>())).Returns(pontoDemanda);
            return mockApp.Object;
        }

        private static Mock<INotificacaoAplicacao> GetMockNotificacaoApp()
        {
            var mockApp = new Mock<INotificacaoAplicacao>();
            return mockApp;
        }
    }
}
