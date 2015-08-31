﻿using System;
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
    public class CompraAplicacaoTests
    {
        private Fakes _fakes;
        private MockCompraRepo _mockRepo;
        private IRepositorioProcedures _repoProcedures;
        private Integrante _integrante;
        private PontoDemanda _pontoDemanda;
        private ContextoEF _contexto;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
            _mockRepo = new MockCompraRepo();
            _contexto = new ContextoEF();
            _integrante = _contexto.Usuarios.First().Integrante;
            _pontoDemanda = _contexto.PontosDemanda.First();
            _repoProcedures = new Mock<IRepositorioProcedures>().Object;
        }

        [Test]
        public void CriarCompra()
        {
            using (new TransactionScope())
            {
                var compra = _fakes.Compra();
                compra.Integrante = _integrante;
                compra.PontoDemanda = _pontoDemanda;
                compra.Itens.Add(_fakes.ListaCompraItem(_pontoDemanda.Listas.First().Itens.First()));
                var compraAtivaRepo = new CompraAtivaEF();
                var compraAtivaApp = ObterAppCompraAtiva(compraAtivaRepo);
                compraAtivaApp.AtivarCompra(compra.Integrante.Usuario.Id, compra.PontoDemanda.Id);
                var app = ObterAppCompra(new CompraEF(_repoProcedures), compraAtivaRepo, compra.PontoDemanda.Listas.First());
                compra = app.Criar(compra);
                Assert.IsTrue(compra.Id > 0);
                Assert.IsFalse(compraAtivaApp.ExisteCompraAtiva(compra.PontoDemanda.Id));
            }
        }

        [Test]
        public void CriarCompraComItemSubstituto()
        {
            using (new TransactionScope())
            {
                var compra = _fakes.Compra();
                compra.Integrante = _integrante;
                compra.PontoDemanda = _pontoDemanda;
                var compraAtivaRepo = new CompraAtivaEF();
                AtivarCompra(compra, compraAtivaRepo);
                var app = ObterAppCompra(new CompraEF(_repoProcedures), compraAtivaRepo, compra.PontoDemanda.Listas.First());
                var item1 = _pontoDemanda.Listas.First().Itens.First();
                var item2 = _pontoDemanda.Listas.First().Itens.Skip(1).First();
                var compraItemOriginal = new ListaCompraItem { Item = item1, ProdutoId = item1.Produto.Id, Quantidade = 2, Valor = 2.5M };
                var compraItemSubstituto = new ListaCompraItem { Item = item2, ProdutoId = item2.Produto.Id, Quantidade = 1, Valor = 1, };

                compra.AdicionarItemSubstituto(compraItemOriginal, compraItemSubstituto, _fakes.MotivosSubstituicao().First());
                compra = app.Criar(compra);
                Assert.IsTrue(compra.Id > 0);
                Assert.AreEqual(2, compra.Itens.Count);
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
                var compra = _fakes.Compra();
                compra.Integrante = _integrante;
                compra.PontoDemanda = _pontoDemanda;
                var compraAtivaRepo = new CompraAtivaEF();
                AtivarCompra(compra, compraAtivaRepo);
                var app = ObterAppCompra(new CompraEF(_repoProcedures), compraAtivaRepo, compra.PontoDemanda.Listas.First());
                
                var item = _fakes.ListaCompraItem();
                item.Item.Produto.Categorias.First().Id = 2;
                compra.Itens.Add(item);

                compra = app.Criar(compra);
                
                Assert.IsTrue(compra.Id > 0);
                Assert.IsTrue(compra.Itens.OfType<ListaCompraItem>().Any(i => i.Item.Periodo.Nome.Equals("eventual", StringComparison.InvariantCultureIgnoreCase)));
                Assert.IsTrue(compra.Itens.OfType<ListaCompraItem>().Count() == 1);
            }
        }

        [Test]
        public void CriarCompraDefinePedidosComStatusCorretos()
        {
            var compra = _fakes.Compra();
            compra.Integrante = _integrante;
            compra.PontoDemanda = _pontoDemanda;
            compra.PontoDemanda.Id = 100;
            compra.Itens.Clear();
            compra.Itens.Add(new PedidoCompraItem { Status = StatusCompra.AgoraNao, Item = new PedidoItem { Id = 1 }, ProdutoId = 1, Quantidade = 1, Valor = 1.25M });
            compra.Itens.Add(new PedidoCompraItem { Status = StatusCompra.Comprado, Item = new PedidoItem { Id = 2 }, ProdutoId = 2, Quantidade = 1, Valor = 1.25M });
            compra.Itens.Add(new PedidoCompraItem { Status = StatusCompra.ItemAComprar, Item = new PedidoItem { Id = 3 }, ProdutoId = 3, Quantidade = 1, Valor = 1.25M });
            compra.Itens.Add(new PedidoCompraItem { Status = StatusCompra.ItemSubstituido, Item = new PedidoItem { Id = 4 }, ProdutoId = 4, Quantidade = 1, Valor = 1.25M });
            compra.Itens.Add(new PedidoCompraItem { Status = StatusCompra.NaoEncontrado, Item = new PedidoItem { Id = 5 }, ProdutoId = 5, Quantidade = 1, Valor = 1.25M });

            var mockCompraAtivaRepo = new MockCompraAtivaRepo {CompraAtiva = _fakes.CompraAtiva()};
            var app = ObterAppCompra(_mockRepo.GetMockedRepo(), mockCompraAtivaRepo.GetMockedRepo());
            compra = app.Criar(compra);
            Assert.AreEqual(StatusPedido.Rejeitado, compra.Itens.OfType<PedidoCompraItem>().Single(i => i.Item.Id == 1).Item.Status);
            Assert.AreEqual(StatusPedido.Comprado, compra.Itens.OfType<PedidoCompraItem>().Single(i => i.Item.Id == 2).Item.Status);
            Assert.AreEqual(StatusPedido.Pendente, compra.Itens.OfType<PedidoCompraItem>().Single(i => i.Item.Id == 3).Item.Status);
            Assert.AreEqual(StatusPedido.Substituido, compra.Itens.OfType<PedidoCompraItem>().Single(i => i.Item.Id == 4).Item.Status);
            Assert.AreEqual(StatusPedido.Pendente, compra.Itens.OfType<PedidoCompraItem>().Single(i => i.Item.Id == 5).Item.Status);
        }

        [Test]
        public void ListaSugestaoDeCompra()
        {
            var app = ObterAppCompra(_mockRepo.GetMockedRepo(), null);
            var listaSugestao = app.ListarSugestao(100).ToList();
            Assert.AreEqual(2, listaSugestao.OfType<ListaItem>().Count());
            Assert.AreEqual(1, listaSugestao.OfType<PedidoItem>().Count());
        }

        [Test]
        public void ListarItensSubstitutosParaListaItens()
        {
            var app = ObterAppCompraParaTestarSubstitutos();
            var substitutos = app.ListarItensSubstitutos(100, 1).ToList();
            Assert.AreEqual(2, substitutos.Count());
            Assert.IsTrue(substitutos.Any(i => i.Id == 200));
            Assert.IsTrue(substitutos.Any(i => i.Id == 202));
        }

        [Test]
        public void ListarItensSubstitutosPedidoSubstituidoPorItemDaLista()
        {
            var app = ObterAppCompraParaTestarSubstitutos();
            var substitutos = app.ListarItensSubstitutos(100, 1).ToList();
            Assert.AreEqual(2, substitutos.Count());
            Assert.IsTrue(substitutos.Any(i => i.Id == 200));
            Assert.IsTrue(substitutos.Any(i => i.Id == 202));
        }

        [Test]
        public void ListarItensSubstitutosParaPedidoItens()
        {
            var app = ObterAppCompraParaTestarSubstitutos();
            var substitutos = app.ListarItensSubstitutos(100, 3).ToList();
            Assert.AreEqual(1, substitutos.Count());
            Assert.IsTrue(substitutos.Any(i => i.Id == 204));
        }

        private CompraAplicacao ObterAppCompraParaTestarSubstitutos()
        {
            var pontoDemanda = _fakes.PontoDemanda();
            pontoDemanda.Id = 100;

            var compra1 = _fakes.Compra();
            compra1.PontoDemanda = pontoDemanda;

            var compra2 = _fakes.Compra();
            compra2.PontoDemanda = pontoDemanda;

            var compra3 = _fakes.Compra();
            compra3.PontoDemanda = pontoDemanda;

            var itemOriginal1 = new ListaCompraItem {Item = new ListaItem {Id = 1}};
            var itemOriginal2 = new PedidoCompraItem {Item = new PedidoItem {Id = 2}};
            var itemOriginal3 = new PedidoCompraItem { Item = new PedidoItem { Id = 3 } };

            compra1.Itens.Add(new ListaCompraItem
            {
                Id = 200,
                ItemSubstituto = new CompraItemSubstituto {Original = itemOriginal1}
            });
            compra1.Itens.Add(new ListaCompraItem {Id = 201});

            compra2.Itens.Add(new ListaCompraItem
            {
                Id = 202,
                ItemSubstituto = new CompraItemSubstituto {Original = itemOriginal1}
            });
            compra2.Itens.Add(new PedidoCompraItem
            {
                Id = 203,
                ItemSubstituto = new CompraItemSubstituto {Original = itemOriginal2}
            });

            compra3.Itens.Add(new ListaCompraItem
            {
                Id = 204,
                ItemSubstituto = new CompraItemSubstituto { Original = itemOriginal3}
            });

            var compras = new List<Compra> {compra1, compra2, compra3};
            _mockRepo.Compras = compras;

            var app = ObterAppCompra(_mockRepo.GetMockedRepo(), null);
            return app;
        }

        private static void AtivarCompra(Compra compra, IRepositorioCompraAtiva repo)
        {
            repo.AtivarCompra(compra.Integrante.Usuario.Id, compra.PontoDemanda.Id);
        }

        private CompraAplicacao ObterAppCompra(IRepositorioCompra compraRepo, IRepositorioCompraAtiva compraAtivaRepo, Lista lista = null)
        {
            return new CompraAplicacao(compraRepo, ObterAppPedido(), ObterAppLista(), ObterAppCompraAtiva(compraAtivaRepo));
        }

        private IListaAplicacao ObterAppLista(Lista lista = null)
        {
            var mockRepo = new MockListaRepo { Lista = lista ?? _fakes.Lista() };
            return new ListaAplicacao(mockRepo.GetMockedRepo());
        }

        private IPedidoAplicacao ObterAppPedido()
        {
            var notificacaoApp = new NotificacaoAplicacao(new Mock<IServicoRest>().Object, new Mock<ITemplateMensagemAplicacao>().Object, new Mock<IFilaItemAplicacao>().Object);
            var mockRepo = new MockPedidoRepo {PedidoItens = _fakes.PedidoItens()};
            return new PedidoAplicacao(mockRepo.GetMockedRepo(), new CompraAtivaAplicacao(new MockCompraAtivaRepo().GetMockedRepo(), notificacaoApp), notificacaoApp);
        }

        private static ICompraAtivaAplicacao ObterAppCompraAtiva(IRepositorioCompraAtiva compraAtivaRepo)
        {
            return new CompraAtivaAplicacao(compraAtivaRepo, new Mock<INotificacaoAplicacao>().Object);
        }
    }
}
