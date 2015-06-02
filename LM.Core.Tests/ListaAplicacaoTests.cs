using System;
using System.Data.Entity.Validation;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.RepositorioEF;
using NUnit.Framework;
using System.Linq;
using System.Transactions;

namespace LM.Core.Tests
{
    [TestFixture]
    public class ListaAplicacaoTests
    {
        private static long _pontoDemandaId;
        public ListaAplicacaoTests()
        {
            _pontoDemandaId = new ContextoEF().PontosDemanda.First().Id;
        }
        
        [Test]
        public void AdiconarUmItemEmUmaLista()
        {
            var listaApp = ObterListaApp();
            const int produtoId = 23271;
            var item1 = new ListaItem
            {
                QuantidadeDeConsumo = 5,
                QuantidadeEmEstoque = 3,
                Periodo = new Periodo { Id = 5 },
                Produto = new Produto { Id = produtoId }
            };

            var item2 = new ListaItem
            {
                QuantidadeDeConsumo = 2,
                QuantidadeEmEstoque = 4,
                Periodo = new Periodo { Id = 2 },
                Produto = new Produto { Id = produtoId }
            };
            
            using (new TransactionScope())
            {
                item1 = listaApp.AdicionarItem(_pontoDemandaId, item1);
                Assert.IsTrue(item1.Id > 0);
                var listaApp2 = ObterListaApp();
                Assert.Throws<ApplicationException>(() => listaApp2.AdicionarItem(_pontoDemandaId, item2));
            }
        }

        [Test]
        public void NaoPodeAdiconarUmItemRepetidoEmUmaLista()
        {
            var listaApp = ObterListaApp();

            var item = new ListaItem
            {
                QuantidadeDeConsumo = 5,
                QuantidadeEmEstoque = 3,
                Periodo = new Periodo { Id = 5 },
                Produto = new Produto { Id = 23271 }
            };

            using (new TransactionScope())
            {
                item = listaApp.AdicionarItem(_pontoDemandaId, item);
                Assert.IsTrue(item.Id > 0);
            }
        }

        [Test]
        public void RemoverUmItemDeUmaLista()
        {
            var listaApp = ObterListaApp();

            var itens = listaApp.ListarItens(_pontoDemandaId).ToList();
            var idItem = itens.First().Id;
            var totalDeItems = itens.Count();
            
            using (new TransactionScope())
            {
                listaApp.DesativarItem(_pontoDemandaId, idItem);
                Assert.IsTrue(listaApp.ListarItens(_pontoDemandaId).Count() == totalDeItems - 1);
            }

        }

        [Test]
        public void ListarSecoesDeUmaLista()
        {
            var listaApp = ObterListaApp();

            var secoes = listaApp.ListarSecoes(_pontoDemandaId);
            Assert.IsTrue(secoes.All(s => s.SubCategorias.Count > 0));
        }

        [Test]
        public void AtualizarEstoqueDeUmItemDeUmaLista()
        {
            var listaApp = ObterListaApp();

            var item = listaApp.ListarItensPorCategoria(_pontoDemandaId, 12000).First();
            using (new TransactionScope())
            {
                listaApp.AtualizarEstoqueDoItem(_pontoDemandaId, item.Id, 12);
                Assert.AreEqual(12, listaApp.ListarItensPorCategoria(_pontoDemandaId, 12000).First().QuantidadeEmEstoque);    
            }
        }

        [Test]
        public void AtualizarConsumoDeUmItemDeUmaLista()
        {
            using (new TransactionScope())
            {
                var listaApp = ObterListaApp();
                listaApp.AtualizarConsumoDoItem(_pontoDemandaId, 186, 5);
                Assert.AreEqual(5, listaApp.ListarItensPorCategoria(_pontoDemandaId, 12000).First().QuantidadeDeConsumo);
            }
        }

        [Test]
        public void AtualizarPeriodoDeUmItemDeUmaLista()
        {
            var listaApp = ObterListaApp();

            var item = listaApp.ListarItensPorCategoria(_pontoDemandaId, 12000).First();
            using (new TransactionScope())
            {
                listaApp.AtualizarPeriodoDoItem(_pontoDemandaId, item.Id, 5);
                Assert.AreEqual(5, listaApp.ListarItensPorCategoria(_pontoDemandaId, 12000).First().Periodo.Id);
            }
        }

        [Test]
        public void AtualizarUmItemDeUmaLista()
        {
            var listaApp = ObterListaApp();

            var item = listaApp.ListarItensPorCategoria(_pontoDemandaId, 12000).First();
            using (new TransactionScope())
            {
                listaApp.AtualizarItem(_pontoDemandaId, item.Id, 5, 3, 2, true);
                var updatedItem = listaApp.ListarItensPorCategoria(_pontoDemandaId, 12000).First();
                Assert.AreEqual(5, updatedItem.QuantidadeDeConsumo);
                Assert.AreEqual(3, updatedItem.QuantidadeEmEstoque);
                Assert.AreEqual(2, updatedItem.Periodo.Id);
                Assert.IsTrue(updatedItem.EhEssencial);
            }
        }

        [Test]
        [Ignore("Existe algum bug na hora de usar o full text search")]
        public void BuscaUmItemDaLista()
        {
            var app = ObterListaApp();
            var itens = app.BuscarItens(_pontoDemandaId, "Bombom");
            Assert.IsTrue(itens.Any());
        }

        private static IListaAplicacao ObterListaApp()
        {
            return new ListaAplicacao(new ListaEF());
        }
    }
}
