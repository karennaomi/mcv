using System.Linq;
using System.Runtime.Remoting;
using LM.Core.Domain;
using LM.Core.RepositorioEF;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class MapearLMContextTests
    {
        private readonly ContextoEF _contexto = new ContextoEF();
        
        [Test]
        public void MapearUsuarios()
        {
            var usuario = _contexto.Usuarios.First();
            Assert.IsNotNull(usuario.StatusUsuarioPontoDemanda);
            Assert.IsNotNull(usuario.MapIntegrantes);
            Assert.IsTrue(usuario.MapIntegrantes.Any());
            Assert.IsNotNull(usuario.Integrante);
        }

        [Test]
        public void MapearPontosDemanda()
        {
            var pontoDemanda = _contexto.PontosDemanda.First();
            Assert.IsNotNull(pontoDemanda.GrupoDeIntegrantes);
            Assert.IsNotNull(pontoDemanda.Endereco);
            Assert.IsNotNull(pontoDemanda.Endereco.Cidade);
            Assert.IsNotNull(pontoDemanda.Endereco.Cidade.Uf);
            Assert.IsTrue(pontoDemanda.Listas.Any());
        }

        [Test]
        public void MapearIntegrante()
        {
            var integrante = _contexto.Integrantes.First();
            Assert.IsNotNull(integrante.GrupoDeIntegrantes);
            Assert.IsNotNull(integrante.Persona);
            Assert.IsNotNull(integrante.Usuario);
        }

        [Test]
        public void MapearPersona()
        {
            var persona = _contexto.Personas.First();
            Assert.IsNotNull(persona);
        }

        [Test]
        public void MapearCategorias()
        {
            var secao = _contexto.Categorias.Single(c => c.Nome == "LIMPEZA");
            Assert.IsTrue(secao.SubCategorias.Any());
        }

        [Test]
        public void MapearProdutos()
        {
            var produto = _contexto.Produtos.Find(8);
            Assert.IsNotNull(produto.Info);
            Assert.IsTrue(produto.Imagens.Any());
            Assert.IsTrue(produto.Categorias.Any());
        }

        [Test]
        public void MapearLista()
        {
            var lista = _contexto.Listas.First();
            Assert.IsNotNull(lista.PontoDemanda);
            Assert.IsTrue(lista.Itens.Any());
            Assert.IsNotNull(lista.Itens.First().Periodo);
        }

        [Test]
        public void MapearPedido()
        {
            var pedido = _contexto.PedidoItens.First();
            Assert.IsNotNull(pedido.PontoDemanda);
            Assert.IsNotNull(pedido.Produto);
            Assert.IsNotNull(pedido.Integrante);
        }

        [Test]
        public void MapearCompra()
        {
            var compra = _contexto.Compras.Find(20304);
            Assert.IsNotNull(compra.PontoDemanda);
            Assert.IsNotNull(compra.Integrante);
            Assert.IsNotNull(compra.Itens);
            Assert.IsTrue(compra.Itens.Any());
            Assert.IsNotNull(compra.Itens.OfType<ListaCompraItem>().First().Item);
            Assert.IsNotNull(compra.Itens.OfType<PedidoCompraItem>().First().Item);
        }

        [Test]
        public void MapearCidades()
        {
            var cidade = _contexto.Cidades.First();
            Assert.IsNotNull(cidade.Uf);
        }

        [Test]
        public void MapearCompraAtiva()
        {
            var comrpaAtiva = _contexto.ComprasAtivas.First();
            Assert.IsNotNull(comrpaAtiva.PontoDemanda);
            Assert.IsNotNull(comrpaAtiva.Usuario);
        }
    }
}

