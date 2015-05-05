using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.RepositorioEF;
using NUnit.Framework;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Transactions;

namespace LM.Core.Tests
{
    [TestFixture]
    public class CompraAplicacaoTests
    {
        private readonly ContextoEF _contexto = new ContextoEF();

        [Test]
        public void CriarCompra()
        {
            using (new TransactionScope())
            {
                var compra = GetCompra();
                var app = GetCompraApp();
                compra = app.Criar(compra);
                Assert.IsTrue(compra.Id > 0);
            }
        }

        [Test]
        public void NaoPodeCriarCompraComItemDuplicado()
        {
            using (new TransactionScope())
            {
                var compra = GetCompra();
                var item = compra.Itens.OfType<ListaCompraItem>().First();
                compra.Itens.Add(new ListaCompraItem { Item = new ListaItem { Id = item.Id }, ProdutoId = item.ProdutoId, Quantidade = 1, Valor = 1.5M, Status = StatusCompra.NaoEncontrado });
                var app = GetCompraApp();
                Assert.Throws<ApplicationException>(() => app.Criar(compra));
            }
        }

        [Test]
        public void CriarCompraComItemSubstituto()
        {
            using (new TransactionScope())
            {
                var compra = GetCompra();

                var pontoDemanda = GetPontoDemanda();
                var lista = pontoDemanda.Listas.First();

                var itemOriginal = lista.Itens.OrderBy(i => i.Id).Skip(1).First();
                var compraItemOriginal = new ListaCompraItem { Item = new ListaItem { Id = itemOriginal.Id }, ProdutoId = itemOriginal.Produto.Id, Quantidade = 2, Valor = 2.5M };
                
                var itemSubstituto = lista.Itens.OrderBy(i => i.Id).Skip(2).First();
                var compraItemSubstituto = new ListaCompraItem { Item = new ListaItem { Id = itemSubstituto.Id }, ProdutoId = itemSubstituto.Produto.Id, Quantidade = 1, Valor = 1, };

                compra.AdicionarItemSubstituto(compraItemOriginal, compraItemSubstituto, "teste");
                var app = GetCompraApp();
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
                var compra = GetCompra();
                var produto = new Produto(2)
                {
                    Info = new ProdutoInfo { Nome = "Macarrão Tabajara" },
                    Ean = "ajsh278aska"
                };
                compra.Itens.Add(new ListaCompraItem
                {
                    Item = new ListaItem { Produto = produto },
                    Quantidade = 3,
                    Valor = 4.5M,
                    Status = StatusCompra.Comprado
                });

                var app = GetCompraApp();
                compra = app.Criar(compra);
                
                Assert.IsTrue(compra.Id > 0);
                Assert.IsTrue(compra.Itens.OfType<ListaCompraItem>().Any(i => i.Item.Periodo.Id == 12));
                Assert.IsTrue(compra.Itens.OfType<ListaCompraItem>().Count() == 2);
            }
        }

        [Test]
        public void CompraDevePossuirItens()
        {
            var compra = new Compra();
            var app = GetCompraApp();
            var ex = Assert.Throws<ApplicationException>(() => app.Criar(compra));
            Assert.AreEqual("A compra deve possuir itens.", ex.Message);
        }

        [Test]
        public void CompraDevePossuirPontoDemanda()
        {
            var compra = GetCompra();
            compra.PontoDemanda = new PontoDemanda();
            var app = GetCompraApp();
            var ex = Assert.Throws<ApplicationException>(() => app.Criar(compra));
            Assert.AreEqual("A compra deve possuir ponto de demanda.", ex.Message);
        }

        [Test]
        public void CompraDevePossuirIntegrante()
        {
            var compra = GetCompra();
            compra.Integrante = new Integrante();
            var app = GetCompraApp();
            var ex = Assert.Throws<ApplicationException>(() => app.Criar(compra));
            Assert.AreEqual("A compra deve possuir integrante.", ex.Message);
        }

        [Test]
        public void CompraDevePossuirUsuario()
        {
            var compra = GetCompra();
            compra.Integrante = new Integrante { Id = 1234, Usuario = new Usuario()};
            var app = GetCompraApp();
            var ex = Assert.Throws<ApplicationException>(() => app.Criar(compra));
            Assert.AreEqual("O integrante da compra deve possuir um usuário.", ex.Message);
        }

        private static CompraAplicacao GetCompraApp()
        {
            return new CompraAplicacao(new CompraEF());
        }

        private Compra GetCompra()
        {
            var agora = DateTime.Now;
            var pontoDemanda = GetPontoDemanda();
            var lista = pontoDemanda.Listas.First();
            var pedidoItens = _contexto.PedidoItens.Where(p => p.PontoDemanda.Id == pontoDemanda.Id);

            var listaItem = lista.Itens.First();
            var pedidoItem1 = pedidoItens.First();
            var pedidoItem2 = pedidoItens.OrderBy(i => i.Id).Skip(1).First();

            var integrante = pontoDemanda.GrupoDeIntegrantes.Integrantes.First();
            return new Compra
            {
                PontoDemanda = new PontoDemanda { Id = pontoDemanda.Id },
                Integrante = new Integrante { Id = integrante.Id, Usuario = new Usuario { Id = integrante.Usuario.Id} },
                Itens = new Collection<CompraItem>
                {
                    new ListaCompraItem { Item = new ListaItem { Id = listaItem.Id }, ProdutoId = listaItem.Produto.Id, Quantidade = 2, Valor = 2.5M, Status = StatusCompra.Comprado },
                    new PedidoCompraItem { Item = new PedidoItem{ Id = pedidoItem1.Id }, ProdutoId = pedidoItem1.Produto.Id, Quantidade = 1, Valor = 1.25M, Status = StatusCompra.NaoEncontrado},
                    new PedidoCompraItem { Item = new PedidoItem{ Id = pedidoItem2.Id }, ProdutoId = pedidoItem2.Produto.Id, Quantidade = 3, Valor = 2.75M, Status = StatusCompra.Comprado}
                }, 
                DataInicioCompra = agora.AddHours(-1.5),
                DataCapturaPrimeiroItemCompra = agora.AddHours(-1.5),
                DataFimCompra = agora,
                LojaId = 2236
            };
        }

        private PontoDemanda GetPontoDemanda()
        {
            return _contexto.PontosDemanda.First(p => p.GrupoDeIntegrantes.Integrantes.FirstOrDefault().Usuario != null);
        }
    }
}
