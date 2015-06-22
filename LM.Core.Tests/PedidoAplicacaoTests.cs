using System;
using System.Collections.Generic;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using LM.Core.Domain.Servicos;
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
        private Fakes _fakes;
        private readonly MockPedidoRepo _mockRepo;
        public PedidoAplicacaoTests()
        {
            _pontoDemandaId = new ContextoEF().PontosDemanda.First().Id;
            _fakes = new Fakes();
            _mockRepo = new MockPedidoRepo();
        }

        [Test]
        public void AdiconarUmItemEmUmPedido()
        {
            var pedidoApp = ObterPedidoApp(new PedidoEF());

            var item = new PedidoItem
            {
                Quantidade = 5,
                Produto = new Produto { Id = 26270 },
                Integrante = new Integrante { Usuario = new Usuario { Id = 2 }}
            };

            using (new TransactionScope())
            {
                item = pedidoApp.AdicionarItem(_pontoDemandaId, item);
                Assert.IsTrue(item.Id > 0);
            }
        }

        [Test]
        public void NaoPodeAdiconarUmItemRepetidoEmUmPedido()
        {
            var pedidoApp = ObterPedidoApp(new PedidoEF());

            var item1 = new PedidoItem
            {
                Quantidade = 5,
                Produto = new Produto { Id = 25861 },
                Integrante = new Integrante { Usuario = new Usuario { Id = 2 } }
            };

            var item2 = new PedidoItem
            {
                Quantidade = 2,
                Produto = new Produto { Id = 25861 },
                Integrante = new Integrante { Usuario = new Usuario { Id = 2 } }
            };

            using (new TransactionScope())
            {
                item1 = pedidoApp.AdicionarItem(_pontoDemandaId, item1);
                Assert.IsTrue(item1.Id > 0);
                var app2 = ObterPedidoApp(new PedidoEF());
                Assert.Throws<ApplicationException>(() => app2.AdicionarItem(_pontoDemandaId, item2));
            }
        }

        [Test]
        public void RemoverUmItemDeUmPedido()
        {
            var pedidoApp = ObterPedidoApp(new PedidoEF());

            var itens = pedidoApp.ListarItensPorCategoria(_pontoDemandaId, 2000);
            var idItem = itens.First().Id;
            var totalDeItems = itens.Count();
            
            using (new TransactionScope())
            {
                pedidoApp.RemoverItem(_pontoDemandaId, 1, idItem);
                Assert.IsTrue(pedidoApp.ListarItensPorCategoria(_pontoDemandaId, 2000).Count() == totalDeItems - 1);
            }
        }

        [Test]
        public void ListarSecoesDeUmPedido()
        {
            var pedidoApp = ObterPedidoApp(new PedidoEF());

            var secoes = pedidoApp.ListarSecoes(_pontoDemandaId, StatusPedido.Pendente);
            Assert.IsTrue(secoes.All(s => s.SubCategorias.Count > 0));
        }

        [Test]
        public void AtualizarQuantidadeDeUmItemDeUmPedido()
        {
            var pedidoApp = ObterPedidoApp(new PedidoEF());

            var item = pedidoApp.ListarItensPorCategoria(_pontoDemandaId, 2000).First();
            using (new TransactionScope())
            {
                pedidoApp.AtualizarQuantidadeDoItem(_pontoDemandaId, 1, item.Id, 12);
                Assert.AreEqual(12, pedidoApp.ListarItensPorCategoria(_pontoDemandaId, 2000).First().Quantidade);
            }
        }

        [Test]
        public void SomenteUsuarioQueCriouOPedidoPodeAlterarQuantidade()
        {
            var pedidoItem = _fakes.PedidoItem();
            pedidoItem.Id = 400;
            pedidoItem.Integrante = new Integrante{Usuario = new Usuario{ Id = 2 }};
            _mockRepo.PedidoItens = new List<PedidoItem> {pedidoItem};
            
            var appPedido = ObterPedidoApp(_mockRepo.GetMockedRepo());
            Assert.Throws<ApplicationException>(() => appPedido.AtualizarQuantidadeDoItem(100, 1, 400, 3));
        }

        [Test]
        public void SomenteUsuarioQueCriouOPedidoPodeRemover()
        {
            var pedidoItem = _fakes.PedidoItem();
            pedidoItem.Id = 400;
            pedidoItem.Integrante = new Integrante { Usuario = new Usuario { Id = 2 } };
            _mockRepo.PedidoItens = new List<PedidoItem> { pedidoItem };

            var appPedido = ObterPedidoApp(_mockRepo.GetMockedRepo());
            Assert.Throws<ApplicationException>(() => appPedido.RemoverItem(100, 1, 400));
        }

        private static IPedidoAplicacao ObterPedidoApp(IRepositorioPedido repoPedido)
        {
            return new PedidoAplicacao(repoPedido, GetCompraAtivaApp(), GetAppNotificacao(GetMockRestService().Object));
        }

        private static ICompraAtivaAplicacao GetCompraAtivaApp()
        {
            var repoMock = new Mock<IRepositorioCompraAtiva>();
            var usuario = new Usuario {Id = 1};
            var integrante = new Integrante { Nome = "John", Usuario = usuario};
            usuario.Integrante = integrante;
            repoMock.Setup(r => r.Obter(It.IsAny<long>())).Returns(new CompraAtiva
            {
                Usuario = usuario
            });
            return new CompraAtivaAplicacao(repoMock.Object, null);
        }

        private static INotificacaoAplicacao GetAppNotificacao(IServicoRest restService)
        {
            return new NotificacaoAplicacao(restService, GetTemplateMensagemApp(), null);
        }

        private static ITemplateMensagemAplicacao GetTemplateMensagemApp()
        {
            var mockTemplateMensagemApp = new Mock<ITemplateMensagemAplicacao>();
            mockTemplateMensagemApp.Setup(t => t.ObterPorTipoTemplate(It.IsAny<TipoTemplateMensagem>()))
                .Returns(new TemplateMensagemPush
                {
                    Mensagem = "{PontoDemanda.Nome} {Remetente.Nome} {Destinatario.Nome}"
                });
            return mockTemplateMensagemApp.Object;
        }

        private static Mock<IServicoRest> GetMockRestService()
        {
            return new Mock<IServicoRest>();
        }
    }
}
