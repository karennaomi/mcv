using LM.Core.Domain;
using LM.Core.RepositorioEF;
using NUnit.Framework;
using System.Linq;

namespace LM.Core.Tests
{
    [TestFixture]
    public class MapearLMContextTests
    {
        private readonly ContextoEF _contexto = new ContextoEF();
        
        [Test]
        public void MapearUsuarios()
        {
            var usuario = _contexto.Usuarios.OrderBy(u => u.Id).Skip(50).First();
            Assert.IsNotNull(usuario.StatusUsuarioPontoDemanda);
            Assert.IsNotNull(usuario.Integrante);
        }

        [Test]
        public void MapearPontosDemanda()
        {
            var pontoDemanda = _contexto.PontosDemanda.First();
            Assert.IsNotNull(pontoDemanda.Endereco);
            Assert.IsNotNull(pontoDemanda.Endereco.Cidade);
            Assert.IsNotNull(pontoDemanda.Endereco.Cidade.Uf);
            Assert.IsTrue(pontoDemanda.GruposDeIntegrantes.Any());
            Assert.IsNotNull(pontoDemanda.GruposDeIntegrantes.First().Integrante);
            Assert.IsTrue(pontoDemanda.Listas.Any());
        }

        [Test]
        public void MapearPontoDemandaLojasFavoritas()
        {
            var pontoDemanda = _contexto.PontosDemanda.Find(4);
            Assert.IsTrue(pontoDemanda.LojasFavoritas.Any());
            Assert.IsNotNull(pontoDemanda.LojasFavoritas.First().Info.Endereco);
        }


        [Test]
        public void MapearIntegrante()
        {
            var integrante = _contexto.Integrantes.First(i => i.Usuario != null);
            Assert.IsNotNull(integrante.GruposDeIntegrantes);
            Assert.IsNotNull(integrante.GruposDeIntegrantes.First().PontoDemanda);
            Assert.IsNotNull(integrante.Usuario);
        }

        [Test]
        public void MapearCategorias()
        {
            var secao = _contexto.Categorias.Single(c => c.Nome == "LIMPEZA");
            Assert.IsTrue(secao.Imagens.Any());
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
            var compra = _contexto.Compras.First();
            Assert.IsNotNull(compra.PontoDemanda);
            Assert.IsNotNull(compra.Integrante);
            //Assert.IsTrue(compra.Itens.OfType<ListaCompraItem>().Any());
            //Assert.IsTrue(compra.Itens.OfType<PedidoCompraItem>().Any());
        }

        [Test]
        [Ignore("Não tem dado na base para testar o mapeamento")]
        public void MapearCompraItemSubstituto()
        {
            var compra = _contexto.Compras.First(c => c.Id == 10251);
            Assert.IsNotNull(compra.Itens.First(i => i.ItemSubstituto != null).ItemSubstituto.Original);
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

        [Test]
        public void MapearTemplateMensagem()
        {
            var templateEmail = _contexto.TemplatesMensagens.OfType<TemplateMensagemEmail>().First();
            var templatePush = _contexto.TemplatesMensagens.OfType<TemplateMensagemPush>().First();
            Assert.IsNotNull(templateEmail);
            Assert.IsNotNull(templatePush);
        }

        [Test]
        public void MapearLojas()
        {
            var loja = _contexto.Lojas.First();
            Assert.IsNotNull(loja.Info);
            Assert.IsNotNull(loja.Info.Endereco);
        }

        [Test]
        public void MapearRecuperarSenhas()
        {
            var recuperarSenha = _contexto.RecuperarSenhas.First();
            Assert.IsNotNull(recuperarSenha.Usuario);
        }

        [Test]
        public void MapearFilaItens()
        {
            var filaItem = _contexto.FilaItens.First();
            Assert.IsTrue(filaItem.Mensagens.Any());
        }

        [Test]
        public void MapearLegacyTokens()
        {
            var legacyToken = _contexto.LegacyTokens.First();
            Assert.IsNotNull(legacyToken);
        }

        [Test]
        public void MapearUfs()
        {
            var ufs = _contexto.Ufs;
            Assert.IsTrue(ufs.Any());
        }
    }
}

