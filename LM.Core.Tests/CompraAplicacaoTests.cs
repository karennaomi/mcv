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
    public class CompraAplicacaoTests
    {
        private Fakes _fakes;
        private MockCompraRepo _mockRepo;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
            _mockRepo = new MockCompraRepo();
        }

        [Test]
        public void CriarCompra()
        {
            using (new TransactionScope())
            {
                var compra = _fakes.CompraNotSoFake();
                var app = ObterAppCompra(new CompraEF());
                compra = app.Criar(compra);
                Assert.IsTrue(compra.Id > 0);
            }
        }

        [Test]
        public void CriarCompraComItemSubstituto()
        {
            using (new TransactionScope())
            {
                var compra = _fakes.CompraNotSoFake();

                var listaItemIds = new[] { new[] { 2, 27397 }, new[] { 3, 27399 } };
                var compraItemOriginal = new ListaCompraItem { Item = new ListaItem { Id = listaItemIds[0][0] }, ProdutoId = listaItemIds[0][1], Quantidade = 2, Valor = 2.5M };
                var compraItemSubstituto = new ListaCompraItem { Item = new ListaItem { Id = listaItemIds[1][0] }, ProdutoId = listaItemIds[1][1], Quantidade = 1, Valor = 1, };

                compra.AdicionarItemSubstituto(compraItemOriginal, compraItemSubstituto, "teste");
                var app = ObterAppCompra(new CompraEF());
                compra = app.Criar(compra);
                Assert.IsTrue(compra.Id > 0);
                Assert.AreEqual(5, compra.Itens.Count);
                Assert.IsTrue(compra.Itens.Any(i => i.ItemSubstituto != null));
                Assert.AreEqual(StatusCompra.ItemSubstituido, compraItemOriginal.Status);
                Assert.AreEqual(StatusCompra.Comprado, compraItemSubstituto.Status);
            }
        }

        [Test]
        public void CriarCompraComProdutoNovo()
        {
            using (new TransactionScope())
            {
                var compra = _fakes.CompraNotSoFake();
                compra.Itens.Add(_fakes.ListaCompraItem());

                var app = ObterAppCompra(new CompraEF());
                compra = app.Criar(compra);
                
                Assert.IsTrue(compra.Id > 0);
                Assert.IsTrue(compra.Itens.OfType<ListaCompraItem>().Any(i => i.Item.Periodo.Id == 12));
                Assert.IsTrue(compra.Itens.OfType<ListaCompraItem>().Count() == 2);
            }
        }

        [Test]
        public void ListaSugestaoDeCompra()
        {
            var app = ObterAppCompra(_mockRepo.GetMockedRepo());
            var listaSugestao = app.ListarSugestao(100).ToList();
            Assert.AreEqual(2, listaSugestao.OfType<ListaItem>().Count());
            Assert.AreEqual(1, listaSugestao.OfType<PedidoItem>().Count());
        }

        private CompraAplicacao ObterAppCompra(IRepositorioCompra compraRepo)
        {
            return new CompraAplicacao(compraRepo,  ObterAppPedido(), ObterAppLista());
        }

        private ListaAplicacao ObterAppLista()
        {
            var mockRepo = new MockListaRepo {Lista = _fakes.Lista()};
            return new ListaAplicacao(mockRepo.GetMockedRepo());
        }

        private PedidoAplicacao ObterAppPedido()
        {
            var notificacaoApp = new NotificacaoAplicacao(new Mock<IServicoRest>().Object, new Mock<ITemplateMensagemAplicacao>().Object, new Mock<IFilaItemAplicacao>().Object);
            var mockRepo = new MockPedidoRepo {PedidoItens = _fakes.PedidoItens()};
            return new PedidoAplicacao(mockRepo.GetMockedRepo(), new CompraAtivaAplicacao(new MockCompraAtivaRepo().GetMockedRepo(), notificacaoApp), notificacaoApp);
        }
    }
}
