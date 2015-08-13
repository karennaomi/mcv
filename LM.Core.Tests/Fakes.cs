using System.Collections.Generic;
using System.Collections.ObjectModel;
using LM.Core.Domain;
using System;
using NUnit.Framework.Constraints;

namespace LM.Core.Tests
{
    internal class Fakes
    {
        internal const string Email1 = "joedoe@mail.com";

        internal Usuario Usuario(string email = Email1)
        {
            return new Usuario
            {
                Login = email,
                Senha = "123456",
                Integrante = Integrante(email)
            };
        }

        internal Integrante Integrante(string email = Email1)
        {
            var integrante = new Integrante
            {
                DataNascimento = DateTime.Now.AddYears(-18),
                Nome = "Joe Doe",
                Sexo = "M",
                Email = email,
                Telefone = "989998999",
                Tipo = TipoIntegrante.Familia,
                GruposDeIntegrantes = new Collection<GrupoDeIntegrantes>()
            };
            return integrante;
        }

        internal Integrante IntegrantePet()
        {
            var integrante = new Integrante
            {
                Nome = "Douradinho",
                AnimalId = 3,
                Tipo = TipoIntegrante.Pet,
                GruposDeIntegrantes = new Collection<GrupoDeIntegrantes>()
            };
            return integrante;
        }

        internal PontoDemanda PontoDemanda(Usuario usuario = null)
        {
            var u = usuario ?? Usuario();
            u.Id = 7;
            return new PontoDemanda
            {
                Id = 666,
                Tipo = TipoPontoDemanda.Praia,
                Endereco = Endereco(),
                UsuarioCriador = u
            };
        }

        internal Endereco Endereco(string appendLogradouro = "")
        {
            return new Endereco
            {
                Cidade = "São Paulo", 
                Uf = "SP",
                Alias = "Casa de sap",
                Logradouro = "Rua dos bobos" + appendLogradouro,
                Numero = 0,
                Bairro = "Vl Olimpia",
                Cep = "04458001",
                Latitude = -23.611926900000000M,
                Longitude = -46.661498300000000M
            };
        }

        private int _lojaId;
        internal Loja Loja()
        {
            _lojaId = ++_lojaId;
            return new Loja
            {
                Nome = "Loja Teste " + _lojaId,
                LocalizadorId = "saqk8912q45214_" + _lojaId,
                LocalizadorOrigem = "google",
                Info = new LojaInfo
                {
                    RazaoSocial = "TESTE SA" + _lojaId,
                    Endereco = Endereco(_lojaId.ToString())
                }
            };
        }

        internal CompraAtiva CompraAtiva()
        {
            var pontoDemanda = PontoDemanda();
            pontoDemanda.GruposDeIntegrantes = new Collection<GrupoDeIntegrantes> { new GrupoDeIntegrantes{ PontoDemanda = pontoDemanda, Integrante = Integrante()}};
            var compraAtiva = new CompraAtiva {Usuario = Usuario(), PontoDemanda = pontoDemanda};
            return compraAtiva;
        }

        internal TemplateMensagem TemplateMensagemPush()
        {
            return new TemplateMensagemPush
            {
                Mensagem = "Lorem ipsum dolor sit amet"
            };
        }

        internal TemplateMensagem TemplateMensagemEmail()
        {
            return new TemplateMensagemEmail
            {
                Mensagem = "Lorem ipsum dolor sit amet"
            };
        }

        private int _produtoCount;
        internal Produto Produto(string categoria = "Categoria de teste")
        {
            _produtoCount = ++_produtoCount;
            return new Produto
            {
                Info = new ProdutoInfo { Nome = "Macarrão Tabajara " + _produtoCount },
                Ean = "ajsh278aska" + _produtoCount,
                Imagens = new Collection<Imagem>{Imagem()},
                Categorias = new Collection<Categoria> { new Categoria { CategoriaPai = new Categoria {Nome = categoria}, Nome = "SubCategoria " + categoria}}
            };
        }

        internal ListaItem ListaItem(bool ehSugestaoDeCompra = false, string categoria = "", Usuario usuario = null)
        {
            return new ListaItem
            {
                Produto = Produto(categoria), 
                EhSugestaoDeCompra = ehSugestaoDeCompra,
                QuantidadeConsumo = 5,
                QuantidadeEstoque = 3,
                Periodo = new Periodo { Id = 1 },
                AtualizadoPor = usuario ?? Usuario()
            };
        }

        internal PedidoItem PedidoItem(StatusPedido status = StatusPedido.Comprado, string categoria = "")
        {
            return new PedidoItem
            {
                Produto = Produto(categoria), 
                Status = status,
                QuantidadeSugestaoCompra = 1
            };
        }

        internal ListaCompraItem ListaCompraItem(ListaItem item = null)
        {
            return new ListaCompraItem
            {
                Item = item ?? ListaItem(),
                Quantidade = 3,
                Valor = 4.5M,
                Status = StatusCompra.Comprado
            };
        }

        internal PedidoCompraItem PedidoCompraItem(PedidoItem item = null)
        {
            return new PedidoCompraItem
            {
                Item = item ?? PedidoItem(),
                Quantidade = 3,
                Valor = 4.5M,
                Status = StatusCompra.Comprado
            };
        }

        internal Lista Lista(Usuario usuario = null)
        {
            return new Lista
            {
                PontoDemanda = PontoDemanda(),
                Nome = "Lista de teste",
                Itens = new Collection<ListaItem>
                {
                    ListaItem(true, "B", usuario), ListaItem(true, "A", usuario), ListaItem(false, "C", usuario), ListaItem(false, "D", usuario)
                }
            };
        }

        internal IEnumerable<PedidoItem> PedidoItens()
        {
            return new List<PedidoItem> { PedidoItem(StatusPedido.Pendente, "C"), PedidoItem() };
        }

        internal Contrato Contrato()
        {
            return new Contrato { Ativo = true, Conteudo = "Lorem ipsum dolor sit amet", DataInclusao = DateTime.Now.AddDays(-10), InicioVigencia = DateTime.Now.AddDays(-10), FimVigencia = DateTime.Now.AddYears(10) };
        }

        internal Imagem Imagem()
        {
            return new Imagem {Path = "/imagem/limpeza.jpg"};
        }

        internal Categoria Categoria()
        {
            return new Categoria
            {
                Nome = "LIMPEZA", 
                Imagens = new Collection<Imagem> { Imagem() },
                SubCategorias = new Collection<Categoria> { new Categoria { Nome = "ÁLCOOL & REMOVEDOR" } }
            };
        }

        internal RecuperarSenha RecuperarSenha()
        {
            return new RecuperarSenha();
        }

        internal FilaItemMensagem FilaItemMensagem()
        {
            return new FilaItemMensagem
            {
                FilaMensagens = new Collection<FilaMensagem>
                {
                    new FilaMensagemEmail
                    {
                        Assunto = "Assunto teste",
                        Corpo = "Lorem ipsum dolor sit amet",
                        EmailDestinatario = Email1,
                    }
                }
            };
        }

        internal FilaItemProduto FilaItemProduto()
        {
            return new FilaItemProduto
            {
                FilaProdutos = new Collection<FilaProduto>
                {
                    new FilaProduto
                    {
                        Descricao = "Lorem ipsum",
                        Ean = "1234567890123",
                        ProdutoId = 123,
                        Imagem = "/imagem/produto.jpg"
                    }
                }
            };
        }

        internal EmailCapturado EmailCapturado()
        {
            return new EmailCapturado {Email = Email1};
        }

        internal Animal Animal()
        {
            return new Animal {Nome = "Cachorro"};
        }

        internal ProdutoPreco ProdutoPreco()
        {
            return new ProdutoPreco {PrecoMax = 50, PrecoMin = 20, DataPreco = DateTime.Now};
        }

        internal Contato Contato()
        {
            return new Contato {Email = Email1, Mensagem = "Lorem Ipsum", Nome = "Joe Doe"};
        }

        internal Compra Compra()
        {
            return new Compra
            {
                DataCapturaPrimeiroItemCompra = DateTime.Now.AddHours(-2),
                DataInicioCompra = DateTime.Now.AddHours(-2),
                DataFimCompra = DateTime.Now,
                DataInclusao = DateTime.Now,
                Itens = new Collection<CompraItem>()
            };
        }

        internal Periodo PeriodoEventual()
        {
            return new Periodo{ Nome = "Eventual", Id = 1};
        }

        internal IList<MotivoSubstituicao> MotivosSubstituicao()
        {
            return new List<MotivoSubstituicao> {
                new MotivoSubstituicao { Id = 1, Motivo = "Não encontrei", Ativo = true },
                new MotivoSubstituicao { Id = 2, Motivo = "Preço", Ativo = true },
            };
        }
    }
}
