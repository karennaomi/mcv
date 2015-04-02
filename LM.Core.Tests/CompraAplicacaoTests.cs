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
                var app = new CompraAplicacao(new CompraEF());
                compra = app.Criar(compra);
                Assert.IsTrue(compra.Id > 0);
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
                var compraItemOriginal = new ListaCompraItem { Item = new ListaItem { Id = itemOriginal.Id }, Quantidade = 2, Valor = 2.5M };
                
                var itemSubstituto = lista.Itens.OrderBy(i => i.Id).Skip(2).First();
                var compraItemSubstituto = new ListaCompraItem { Item = new ListaItem {Id = itemSubstituto.Id}, Quantidade = 1, Valor = 1, };

                compra.AdicionarItemSubstituto(compraItemOriginal, compraItemSubstituto, "teste");
                var app = new CompraAplicacao(new CompraEF());
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
                    Info = new ProdutoInfo {Nome = "Macarrão Tabajara"},
                    Ean = "ajsh278aska"
                };
                compra.Itens.Add(new ProdutoCompraItem
                {
                    Produto = produto,
                    Quantidade = 3,
                    Valor = 4.5M,
                    Status = StatusCompra.Comprado
                });

                var app = new CompraAplicacao(new CompraEF());
                compra = app.Criar(compra);
                Assert.IsTrue(compra.Id > 0);
            }
        }

        [Test]
        public void CriarCompraDevePossuirItens()
        {
            var compra = new Compra();
            var app = new CompraAplicacao(new CompraEF());
            var ex = Assert.Throws<ApplicationException>(() => app.Criar(compra));
            Assert.AreEqual("A compra deve possuir itens.", ex.Message);
        }

        private Compra GetCompra()
        {
            var agora = DateTime.Now;
            var pontoDemanda = GetPontoDemanda();
            var lista = pontoDemanda.Listas.First();
            var pedidoItens = _contexto.PedidoItens.Where(p => p.PontoDemanda.Id == pontoDemanda.Id);
            return new Compra
            {
                PontoDemanda = new PontoDemanda { Id = pontoDemanda.Id },
                Integrante = new Integrante{ Id = pontoDemanda.GrupoDeIntegrantes.Integrantes.First().Id },
                Itens = new Collection<CompraItem>
                {
                    new ListaCompraItem { Item = new ListaItem { Id = lista.Itens.First().Id }, Quantidade = 2, Valor = 2.5M, Status = StatusCompra.Comprado },
                    new PedidoCompraItem { Item = new PedidoItem{ Id = pedidoItens.First().Id }, Quantidade = 1, Valor = 1.25M, Status = StatusCompra.NaoEncontrado},
                    new PedidoCompraItem { Item = new PedidoItem{ Id = pedidoItens.OrderBy(i => i.Id).Skip(1).First().Id }, Quantidade = 3, Valor = 2.75M, Status = StatusCompra.Comprado}
                }, 
                DataInicioCompra = agora.AddHours(-1.5),
                DataCapturaPrimeiroItemCompra = agora.AddHours(-1.5),
                DataFimCompra = agora,
                LojaId = 2
            };
        }

        private PontoDemanda GetPontoDemanda()
        {
            return _contexto.PontosDemanda.First();
        }
    }
}
