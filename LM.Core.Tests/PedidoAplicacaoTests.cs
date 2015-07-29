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
        private readonly Fakes _fakes;
        private readonly MockPedidoRepo _mockRepo;
        private readonly ContextoEF _contexto;
        public PedidoAplicacaoTests()
        {
            _pontoDemandaId = new ContextoEF().PontosDemanda.First().Id;
            _fakes = new Fakes();
            _mockRepo = new MockPedidoRepo();
            _contexto = new ContextoEF();
        }

        [Test]
        public void AdiconarUmItemEmUmPedido()
        {
            var pedidoApp = ObterPedidoApp(new PedidoEF());

            var item = new PedidoItem
            {
                QuantidadeSugestaoCompra = 5,
                Produto = _contexto.Produtos.First(),
                Integrante = _contexto.Integrantes.First()
            };

            item = pedidoApp.AdicionarItem(_pontoDemandaId, item);
            Assert.IsTrue(item.Id > 0);
        }
        
        [Test]
        public void NaoPodeAdiconarUmItemRepetidoEmUmPedido()
        {
            var pedidoApp = ObterPedidoApp(new PedidoEF());
            var produto = _contexto.Produtos.OrderByDescending(p => p.Id).First(); //O teste de cima adiciona um produto, então pegar outro pra testar
            var integrante = _contexto.Integrantes.First();
            var item1 = new PedidoItem
            {
                QuantidadeSugestaoCompra = 5,
                Produto = produto,
                Integrante = integrante
            };

            var item2 = new PedidoItem
            {
                QuantidadeSugestaoCompra = 2,
                Produto = produto,
                Integrante = integrante
            };

            item1 = pedidoApp.AdicionarItem(_pontoDemandaId, item1);
            Assert.IsTrue(item1.Id > 0);
            var app2 = ObterPedidoApp(new PedidoEF());
            Assert.Throws<ApplicationException>(() => app2.AdicionarItem(_pontoDemandaId, item2));
        }

        [Test]
        public void RemoverUmItemDeUmPedido()
        {
            var pedidoApp = ObterPedidoApp(new PedidoEF());

            var itens = pedidoApp.ListarItens(_pontoDemandaId);
            var item = itens.First();
            var totalDeItems = itens.Count();
            
            pedidoApp.RemoverItem(_pontoDemandaId, item.Integrante.Usuario.Id, item.Id);
            Assert.IsTrue(pedidoApp.ListarItens(_pontoDemandaId).Count() == totalDeItems - 1);
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

            var item = pedidoApp.ListarItens(_pontoDemandaId).First();
            using (new TransactionScope())
            {
                pedidoApp.AtualizarQuantidadeDoItem(_pontoDemandaId, item.Integrante.Usuario.Id, item.Id, 12);
                Assert.AreEqual(12, pedidoApp.ListarItens(_pontoDemandaId).First(i => i.Id == item.Id).QuantidadeSugestaoCompra);
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
