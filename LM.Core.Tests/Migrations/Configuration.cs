using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using LM.Core.Domain;
using LM.Core.RepositorioEF;

namespace LM.Core.Tests.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LM.Core.RepositorioEF.ContextoEF>
    {
        private readonly Fakes _fakes;
        public Configuration()
        {
            _fakes = new Fakes();
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LM.Core.RepositorioEF.ContextoEF context)
        {
            //Usuarios
            var usuario = _fakes.Usuario();
            usuario.StatusUsuarioPontoDemanda = new Collection<StatusUsuarioPontoDemanda> { new StatusUsuarioPontoDemanda { StatusCadastro = StatusCadastro.CadastroIniciado } };
            usuario.Integrante.GruposDeIntegrantes.Add(new GrupoDeIntegrantes
            {
                Integrante = usuario.Integrante,
                Nome = "Grupo de Integrantes",
                Papel = PapelIntegrante.Administrador
            });

            //Contratos
            usuario.Contratos = new Collection<Contrato> {_fakes.Contrato()};

            //PontoDemanda
            var pontoDemanda = _fakes.PontoDemanda(usuario);
            pontoDemanda.GruposDeIntegrantes = usuario.Integrante.GruposDeIntegrantes;

            //LojasFavoritas
            pontoDemanda.LojasFavoritas = new Collection<Loja> { _fakes.Loja(), _fakes.Loja() };

            //Categorias
            context.Categorias.Add(_fakes.Categoria());

            //Pedidos
            var pedidoItem = _fakes.PedidoItem();
            pedidoItem.Integrante = pontoDemanda.UsuarioCriador.Integrante;
            pedidoItem.PontoDemanda = pontoDemanda;
            context.PedidoItens.Add(pedidoItem);

            //CompraAtiva
            var compraAtiva = _fakes.CompraAtiva();
            compraAtiva.PontoDemanda = pontoDemanda;
            compraAtiva.Usuario = pontoDemanda.UsuarioCriador;
            context.ComprasAtivas.Add(compraAtiva);

            //Lista
            var lista = _fakes.Lista(pontoDemanda.UsuarioCriador);
            lista.PontoDemanda = pontoDemanda;
            context.Listas.Add(lista);
            context.SaveChanges();
            //TemplatesMensagem
            context.TemplatesMensagens.Add(_fakes.TemplateMensagemEmail());
            context.TemplatesMensagens.Add(_fakes.TemplateMensagemPush());
            
            //RecuperarSenha
            var recuperarSenha = _fakes.RecuperarSenha();
            recuperarSenha.Usuario = pontoDemanda.UsuarioCriador;
            context.RecuperarSenhas.Add(recuperarSenha);

            ////FilaItens
            context.FilaItens.Add(_fakes.FilaItemMensagem());
            context.FilaItens.Add(_fakes.FilaItemProduto());

            ////EmailsCapturados
            context.EmailsCapturados.Add(_fakes.EmailCapturado());

            ////Animais
            var animal = _fakes.Animal();
            context.Animais.Add(animal);

            //IntegrantePet
            var integrantePet = _fakes.IntegrantePet();
            integrantePet.AnimalId = animal.Id;
            context.Integrantes.Add(integrantePet);

            //ProdutoPreco
            var produto = _fakes.Produto();
            produto.Precos = new Collection<ProdutoPreco> { _fakes.ProdutoPreco() };
            context.Produtos.Add(produto);

            //Contatos
            context.Contatos.Add(_fakes.Contato());

            //Motivo Substituicao
            _fakes.MotivosSubstituicao().ToList().ForEach(m => context.MotivosSubstituicao.Add(m));

            //Compras
            var compra = _fakes.Compra();
            compra.Integrante = usuario.Integrante;
            compra.LojaId = pontoDemanda.LojasFavoritas.First().Id;
            compra.PontoDemanda = pontoDemanda;
            compra.Itens.Add(_fakes.ListaCompraItem(lista.Itens.First()));
            compra.Itens.Add(_fakes.ListaCompraItem(lista.Itens.Skip(1).First()));
            compra.Itens.Add(_fakes.PedidoCompraItem(pedidoItem));
            compra.AdicionarItemSubstituto(_fakes.ListaCompraItem(lista.Itens.Skip(2).First()), _fakes.ListaCompraItem(lista.Itens.Skip(3).First()), context.MotivosSubstituicao.Local.First());
            context.Compras.Add(compra);

            //Periodo
            context.Set<Periodo>().Add(_fakes.PeriodoEventual());

            try
            {
                context.SaveChanges();

            }
            catch (DbEntityValidationException ex)
            {
                
                throw;
            }
        }
    }
}
