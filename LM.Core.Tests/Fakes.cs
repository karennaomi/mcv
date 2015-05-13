﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LM.Core.Domain;
using System;
using NUnit.Framework;

namespace LM.Core.Tests
{
    internal class Fakes
    {
        internal Usuario Usuario()
        {
            return new Usuario
            {
                Login = "joe123@doe.com",
                Senha = "123456",
                Integrante = Integrante()
            };
        }

        internal Integrante Integrante()
        {
            var integrante = new Integrante
            {
                DataNascimento = DateTime.Now.AddYears(-18),
                Nome = "Joe Doe",
                Sexo = "M",
                Email = "joe123@doe.com",
                GrupoDeIntegrantes = new GrupoDeIntegrantes {Id = 1, PontosDemanda = new Collection<PontoDemanda>(), Integrantes = new Collection<Integrante>()},
                Telefone = "989998999"
            };
            integrante.GrupoDeIntegrantes.Integrantes.Add(integrante);
            return integrante;
        }

        internal PontoDemanda PontoDemanda()
        {
            var usuario = Usuario();
            usuario.Id = 7;
            return new PontoDemanda
            {
                Id = 666,
                Tipo = TipoPontoDemanda.Praia,
                GrupoDeIntegrantes = new GrupoDeIntegrantes {Id = 1, Integrantes = new Collection<Integrante>()},
                Endereco = Endereco(),
                UsuarioCriador = usuario
            };
        }

        internal Endereco Endereco()
        {
            return new Endereco
            {
                Cidade = new Cidade {Nome = "São Paulo"},
                Alias = "Casa de sap",
                Logradouro = "Rua dos bobos",
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
                Idlocalizador = "saqk8912q45214_" + _lojaId,
                OrigemLocalizador = "google",
                Info = new LojaInfo
                {
                    RazaoSocial = "TESTE SA" + _lojaId,
                    Endereco = Endereco()
                }
            };
        }

        internal CompraAtiva CompraAtiva()
        {
            var compraAtiva = new CompraAtiva {Usuario = Usuario(), PontoDemanda = PontoDemanda()};
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

        internal Produto Produto(string categoria)
        {
            return new Produto
            {
                Info = new ProdutoInfo { Nome = "Macarrão Tabajara" },
                Ean = "ajsh278aska",
                Categorias = new Collection<Categoria> { new Categoria { CategoriaPai = new Categoria {Nome = categoria} }}
            };
        }

        internal ListaItem ListaItem(bool ehSugestaoDeCompra = false, string categoria = "")
        {
            return new ListaItem { Produto = Produto(categoria), EhSugestaoDeCompra = ehSugestaoDeCompra };
        }

        internal PedidoItem PedidoItem(StatusPedido status = StatusPedido.Comprado, string categoria = "")
        {
            return new PedidoItem { Produto = Produto(categoria), Status = status };
        }

        internal ListaCompraItem ListaCompraItem()
        {
            return new ListaCompraItem
            {
                Item = ListaItem(),
                Quantidade = 3,
                Valor = 4.5M,
                Status = StatusCompra.Comprado
            };
        }

        internal Compra CompraNotSoFake()
        {
            var agora = DateTime.Now;
            //Esses valores precisam ser valores válidos na base para simular uma compra
            const int pontoDemandaId = 1;
            const int integranteId = 1;
            const int usuarioId = 3;
            const int lojaId = 1;
            var listaItemIds = new[] { new[] { 1, 27395 } };
            var pedidoItemIds = new[] { new[] { 10187, 23271 }, new[] { 10188, 102 } };
            
            return new Compra
            {
                PontoDemanda = new PontoDemanda { Id = pontoDemandaId },
                Integrante = new Integrante { Id = integranteId, Usuario = new Usuario { Id = usuarioId } },
                Itens = new Collection<CompraItem>
                {
                    new ListaCompraItem { Item = new ListaItem { Id = listaItemIds[0][0] }, ProdutoId = listaItemIds[0][1], Quantidade = 2, Valor = 2.5M, Status = StatusCompra.Comprado },
                    new PedidoCompraItem { Item = new PedidoItem{ Id = pedidoItemIds[0][0] }, ProdutoId = pedidoItemIds[0][1], Quantidade = 1, Valor = 1.25M, Status = StatusCompra.NaoEncontrado},
                    new PedidoCompraItem { Item = new PedidoItem{ Id = pedidoItemIds[1][0] }, ProdutoId = pedidoItemIds[1][1], Quantidade = 3, Valor = 2.75M, Status = StatusCompra.Comprado}
                },
                DataInicioCompra = agora.AddHours(-1.5),
                DataCapturaPrimeiroItemCompra = agora.AddHours(-1.5),
                DataFimCompra = agora,
                LojaId = lojaId
            };
        }

        internal Lista Lista()
        {
            return new Lista
            {
                PontoDemanda = PontoDemanda(),
                Nome = "Lista de teste",
                Itens = new Collection<ListaItem>
                {
                    ListaItem(true, "B"), ListaItem(true, "A"), ListaItem(),
                }
            };
        }

        internal IEnumerable<PedidoItem> PedidoItens()
        {
            return new List<PedidoItem> { PedidoItem(StatusPedido.Pendente, "C"), PedidoItem() };
        }
    }
}
