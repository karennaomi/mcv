using System;
using LM.Core.Application;
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
    public class ListaAplicacaoTests
    {
        private static long _integranteId, _usuarioId;
        private PontoDemanda _pontoDemanda; 
        private Fakes _fakes;
        private Produto _produto1, _produto2;

        [TestFixtureSetUp]
        public void Init()
        {
            var contexto = new ContextoEF();
            _pontoDemanda = contexto.PontosDemanda.First();

            var integrante = _pontoDemanda.GruposDeIntegrantes.First().Integrante;
            _integranteId = integrante.Id;
            _usuarioId = integrante.Usuario.Id;

            _fakes = new Fakes();

            _produto1 = contexto.Produtos.Add(_fakes.Produto());
            _produto2 = contexto.Produtos.Add(_fakes.Produto());
            contexto.SaveChanges();
        }
        
        [Test]
        public void NaoPodeAdiconarUmItemRepetidoEmUmaLista()
        {
            var listaApp = ObterListaApp();
            var item1 = new ListaItem
            {
                QuantidadeConsumo = 5,
                QuantidadeEstoque = 3,
                Periodo = new Periodo { Id = 4 },
                Produto = _produto2
            };

            var item2 = new ListaItem
            {
                QuantidadeConsumo = 2,
                QuantidadeEstoque = 4,
                Periodo = new Periodo { Id = 2 },
                Produto = _produto2
            };
            
            item1 = listaApp.AdicionarItem(_usuarioId, _pontoDemanda.Id, item1);
            Assert.IsTrue(item1.Id > 0);
            var listaApp2 = ObterListaApp();
            Assert.Throws<ApplicationException>(() => listaApp2.AdicionarItem(_usuarioId, _pontoDemanda.Id, item2));
        }

        [Test]
        public void AdiconarUmItemEmUmaLista()
        {
            var listaApp = ObterListaApp();

            var item = new ListaItem
            {
                QuantidadeConsumo = 5,
                QuantidadeEstoque = 3,
                Periodo = new Periodo { Id = 2 },
                Produto = _produto1
            };

            item = listaApp.AdicionarItem(_usuarioId, _pontoDemanda.Id, item);
            Assert.IsTrue(item.Id > 0);
        }

        [Test]
        public void RemoverUmItemDeUmaLista()
        {
            var listaApp = ObterListaApp();

            var itens = listaApp.ListarItens(_pontoDemanda.Id).ToList();
            var idItem = itens.First().Id;
            var totalDeItems = itens.Count();
            
            using (new TransactionScope())
            {
                listaApp.DesativarItem(_usuarioId, _pontoDemanda.Id, idItem);
                Assert.IsTrue(listaApp.ListarItens(_pontoDemanda.Id).Count() == totalDeItems - 1);
            }

        }

        [Test]
        public void ListarSecoesDeUmaLista()
        {
            var listaApp = ObterListaApp();

            var secoes = listaApp.ListarSecoes(_pontoDemanda.Id);
            Assert.IsTrue(secoes.All(s => s.SubCategorias.Count > 0));
        }

        [Test]
        public void AtualizarEstoqueDeUmItemDeUmaLista()
        {
            var listaApp = ObterListaApp();
            var item = _pontoDemanda.Listas.First().Itens.First();
            listaApp.AtualizarEstoqueDoItem(_pontoDemanda.Id, _integranteId, item.Id, 12);
            Assert.AreEqual(12, listaApp.ListarItens(_pontoDemanda.Id).First(i => i.Id == item.Id).QuantidadeEstoque);    
        }

        [Test]
        public void AtualizarConsumoDeUmItemDeUmaLista()
        {
            using (new TransactionScope())
            {
                var listaApp = ObterListaApp();
                var item = _pontoDemanda.Listas.First().Itens.First();
                listaApp.AtualizarConsumoDoItem(_usuarioId, _pontoDemanda.Id, item.Id, 8);
                Assert.AreEqual(8, listaApp.ListarItens(_pontoDemanda.Id).First(i => i.Id == item.Id).QuantidadeConsumo);
            }
        }

        [Test]
        public void AtualizarPeriodoDeUmItemDeUmaLista()
        {
            var listaApp = ObterListaApp();
            var item = _pontoDemanda.Listas.First().Itens.First();
            listaApp.AtualizarPeriodoDoItem(_usuarioId, _pontoDemanda.Id, item.Id, 3);
            Assert.AreEqual(3, listaApp.ListarItens(_pontoDemanda.Id).First(i => i.Id == item.Id).Periodo.Id);
        }

        [Test]
        public void AtualizarUmItemDeUmaLista()
        {
            var listaApp = ObterListaApp();
            var item = _pontoDemanda.Listas.First().Itens.First();
            var itemToUpdate = new ListaItem
            {
                Id = item.Id,
                QuantidadeConsumo = 9,
                QuantidadeEstoque = 7,
                Periodo = new Periodo { Id = 2 },
                EhEssencial = true
            };
            listaApp.AtualizarItem(_pontoDemanda.Id, _integranteId, itemToUpdate);
            var updatedItem = listaApp.ListarItens(_pontoDemanda.Id).First(i => i.Id == itemToUpdate.Id);
            Assert.AreEqual(9, updatedItem.QuantidadeConsumo);
            Assert.AreEqual(7, updatedItem.QuantidadeEstoque);
            Assert.AreEqual(2, updatedItem.Periodo.Id);
            Assert.IsTrue(updatedItem.EhEssencial);
        }

        [Test]
        [Ignore("Esta muito lento precisa rever este metodo")]
        public void BuscaUmItemDaLista()
        {
            var app = ObterListaApp();
            var itens = app.BuscarItens(_pontoDemanda.Id, "Bombom");
            Assert.IsTrue(itens.Any());
        }

        [Test]
        public void ListarItens()
        {
            var listaApp = ObterListaApp();

            var itens = listaApp.ListarItens(_pontoDemanda.Id);
            Assert.IsTrue(itens.Any());
        }

        private static IListaAplicacao ObterListaApp()
        {
            return new ListaAplicacao(new ListaEF(new Mock<IRepositorioProcedures>().Object));
        }
    }
}
