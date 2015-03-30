using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.RepositorioEF;
using NUnit.Framework;
using System.Linq;
using System.Transactions;

namespace LM.Core.Tests
{
    [TestFixture]
    public class PedidoAplicacaoTests
    {
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
                item = pedidoApp.AdicionarItem(item);
                Assert.IsTrue(item.Id > 0);
            }
        }

        [Test]
        public void RemoverUmItemDeUmPedido()
        {
            var pedidoApp = ObterPedidoApp();

            var itens = pedidoApp.ListarItensPorCategoria(2000);
            var idItem = itens.First().Id;
            var totalDeItems = itens.Count();
            
            using (new TransactionScope())
            {
                pedidoApp.RemoverItem(idItem);
                Assert.IsTrue(pedidoApp.ListarItensPorCategoria(2000).Count() == totalDeItems - 1);
            }
        }

        [Test]
        public void ListarSecoesDeUmPedido()
        {
            var pedidoApp = ObterPedidoApp();

            var secoes = pedidoApp.ListarSecoes(StatusPedido.Pendente);
            Assert.IsTrue(secoes.All(s => s.SubCategorias.Count > 0));
        }

        [Test]
        public void AtualizarQuantidadeDeUmItemDeUmPedido()
        {
            var pedidoApp = ObterPedidoApp();

            var item = pedidoApp.ListarItensPorCategoria(2000).First();
            using (new TransactionScope())
            {
                pedidoApp.AtualizarQuantidadeDoItem(item.Id, 12);
                Assert.AreEqual(12, pedidoApp.ListarItensPorCategoria(2000).First().Quantidade);
            }
        }

        private static IPedidoAplicacao ObterPedidoApp()
        {
            return new PedidoAplicacao(new PedidoEF(), new ContextoEF().PontosDemanda.First().Id);
        }
    }
}
