using System;
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
            var usuario = _contexto.Usuarios.First();
            Assert.IsNotNull(usuario.StatusUsuarioPontoDemanda);
            Assert.IsNotNull(usuario.Integrante);
        }

        [Test]
        public void MapearContratoUsuario()
        {
            var usuario = _contexto.Usuarios.First();
            Assert.IsNotNull(usuario.Contratos.First());
        }

        [Test]
        public void MapearPontosDemanda()
        {
            var pontoDemanda = _contexto.PontosDemanda.First();
            Assert.IsNotNull(pontoDemanda.Endereco);
            Assert.IsNotNull(pontoDemanda.Listas.First());
            Assert.IsNotNull(pontoDemanda.GruposDeIntegrantes.First().Integrante);
        }

        [Test]
        public void MapearPontoDemandaLojasFavoritas()
        {
            var pontoDemanda = _contexto.PontosDemanda.First();
            Assert.IsNotNull(pontoDemanda.LojasFavoritas.First());
            Assert.IsNotNull(pontoDemanda.LojasFavoritas.First().Info.Endereco);
        }

        [Test]
        public void MapearIntegrante()
        {
            var integrante = _contexto.Integrantes.First(i => i.Usuario != null);
            Assert.IsNotNull(integrante.GruposDeIntegrantes.First());
            Assert.IsNotNull(integrante.GruposDeIntegrantes.First().PontoDemanda);
            Assert.IsNotNull(integrante.Usuario);
        }

        [Test]
        public void MapearEndereco()
        {
            var endereco = _contexto.Set<Endereco>().First();
            Assert.IsNotNull(endereco);
        }

        [Test]
        public void MapearCategorias()
        {
            var secao = _contexto.Categorias.First(c => c.Nome == "LIMPEZA");
            Assert.IsNotNull(secao.Imagens.First());
            Assert.IsNotNull(secao.SubCategorias.First());
        }

        [Test]
        public void MapearProdutos()
        {
            var produto = _contexto.Produtos.First();
            Assert.IsNotNull(produto.Info);
            Assert.IsNotNull(produto.Imagens.First());
            Assert.IsNotNull(produto.Categorias.First());
        }

        [Test]
        public void MapearLista()
        {
            var lista = _contexto.Listas.First();
            Assert.IsNotNull(lista.PontoDemanda);
            Assert.IsNotNull(lista.Itens.First());
            Assert.IsNotNull(lista.Itens.First().Periodo);
            Assert.IsNotNull(lista.Itens.First().AtualizadoPor);
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
        public void MapearCompraAtiva()
        {
            var comrpaAtiva = _contexto.ComprasAtivas.First();
            Assert.IsNotNull(comrpaAtiva.PontoDemanda);
            Assert.IsNotNull(comrpaAtiva.Usuario);
        }

        [Test]
        public void MapearCorreiosCep()
        {
            var contextoCorreios = new ContextoCorreiosEF();
            var endereco = contextoCorreios.EnderecosCorreios.First();
            Assert.IsNotNull(endereco);
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
            var filaItemMensagem = _contexto.FilaItens.OfType<FilaItemMensagem>().First();
            Assert.IsNotNull(filaItemMensagem.FilaMensagens.First());

            var filaItemProduto = _contexto.FilaItens.OfType<FilaItemProduto>().First();
            Assert.IsNotNull(filaItemProduto.FilaProdutos.First());
        }

        [Test]
        public void MapearEmailsCapturados()
        {
            var emailsCapturados = _contexto.EmailsCapturados;
            Assert.IsNotNull(emailsCapturados.First());
        }

        [Test]
        public void MapearAnimais()
        {
            var animais = _contexto.Animais;
            Assert.IsNotNull(animais.First());
        }

        [Test]
        public void MapearIntegrantePet()
        {
            var integrante = _contexto.Integrantes.First(i => i.Tipo == TipoIntegrante.Pet);
            Assert.IsNotNull(integrante.AnimalId);
        }

        [Test]
        public void MapearProdutoPrecos()
        {
            var preco = _contexto.Set<ProdutoPreco>().First();
            Assert.IsNotNull(preco);
        }

        [Test]
        public void MapearContatos()
        {
            var contatos = _contexto.Contatos;
            Assert.IsNotNull(contatos.First());
        }

        [Test]
        public void MapearCompra()
        {
            var compra = _contexto.Compras.First();
            Assert.IsNotNull(compra.PontoDemanda);
            Assert.IsNotNull(compra.Integrante);
        }

        [Test]
        public void MapearCompraItemSubstituto()
        {
            var compra = _contexto.Compras.First();
            Assert.IsNotNull(compra.Itens.First(i => i.ItemSubstituto != null).ItemSubstituto.Original);
        }

        [Test]
        public void MapearMotivosSubstituicao()
        {
            var motivos = _contexto.MotivosSubstituicao;
            Assert.IsNotNull(motivos.First());
        }

        [Test]
        public void MapearPlano()
        {
            var plano = _contexto.Planos;
            Assert.IsNotNull(plano.First());
        }
    }
}
