﻿using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using LM.Core.RepositorioEF;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Transactions;

namespace LM.Core.Tests
{
    [TestFixture]
    public class PedidoAplicacaoTests
    {
        private static long _pontoDemandaId;
        public PedidoAplicacaoTests()
        {
            _pontoDemandaId = new ContextoEF().PontosDemanda.First().Id;
        }

        [Test]
        public void AdiconarUmItemEmUmPedido()
        {
            var pedidoApp = ObterPedidoApp();

            var item = new PedidoItem
            {
                Quantidade = 5,
                Produto = new Produto { Id = 23271 },
                Integrante = new Integrante { Usuario = new Usuario { Id = 2 }}
            };

            using (new TransactionScope())
            {
                item = pedidoApp.AdicionarItem(_pontoDemandaId, item);
                Assert.IsTrue(item.Id > 0);
            }
        }

        [Test]
        public void RemoverUmItemDeUmPedido()
        {
            var pedidoApp = ObterPedidoApp();

            var itens = pedidoApp.ListarItensPorCategoria(_pontoDemandaId, 2000);
            var idItem = itens.First().Id;
            var totalDeItems = itens.Count();
            
            using (new TransactionScope())
            {
                pedidoApp.RemoverItem(_pontoDemandaId, idItem);
                Assert.IsTrue(pedidoApp.ListarItensPorCategoria(_pontoDemandaId, 2000).Count() == totalDeItems - 1);
            }
        }

        [Test]
        public void ListarSecoesDeUmPedido()
        {
            var pedidoApp = ObterPedidoApp();

            var secoes = pedidoApp.ListarSecoes(_pontoDemandaId, StatusPedido.Pendente);
            Assert.IsTrue(secoes.All(s => s.SubCategorias.Count > 0));
        }

        [Test]
        public void AtualizarQuantidadeDeUmItemDeUmPedido()
        {
            var pedidoApp = ObterPedidoApp();

            var item = pedidoApp.ListarItensPorCategoria(_pontoDemandaId, 2000).First();
            using (new TransactionScope())
            {
                pedidoApp.AtualizarQuantidadeDoItem(_pontoDemandaId, item.Id, 12);
                Assert.AreEqual(12, pedidoApp.ListarItensPorCategoria(_pontoDemandaId, 2000).First().Quantidade);
            }
        }

        private static IPedidoAplicacao ObterPedidoApp()
        {
            return new PedidoAplicacao(new PedidoEF(), GetCompraAtivaApp(), GetAppNotificacao(GetMockRestService().Object));
        }

        private static ICompraAtivaAplicacao GetCompraAtivaApp()
        {
            var repoMock = new Mock<IRepositorioCompraAtiva>();
            repoMock.Setup(r => r.Obter(It.IsAny<long>())).Returns(new CompraAtiva
            {
                Usuario = new Usuario { Id = 1, Nome = "John" }
            });
            return new CompraAtivaAplicacao(repoMock.Object, null);
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

        private static Mock<IRestService> GetMockRestService()
        {
            return new Mock<IRestService>();
        }
    }
}
