using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System.Linq;

namespace LM.Core.Application
{
    public interface IIntegranteAplicacao
    {
        Integrante Obter(long pontoDemandaId, long id);
        Integrante Criar(long pontoDemandaId, Integrante integrante);
        Integrante Atualizar(long pontoDemandaId, long usuarioId, Integrante integrante);
        void Desativar(long pontoDemandaId, long usuarioId, long integranteId);
        void RemoverDoPonto(long pontoDemandaId, long usuarioId, long integranteId);
        void Convidar(long pontoDemandaId, long usuarioId, long id, string imageHost);
        IEnumerable<Animal> Animais();
    }

    public class IntegranteAplicacao : IIntegranteAplicacao
    {
        private readonly IRepositorioIntegrante _repositorio;
        private readonly INotificacaoAplicacao _appNotificacao;
        public IntegranteAplicacao(IRepositorioIntegrante repositorio, INotificacaoAplicacao appNotificacao)
        {
            _repositorio = repositorio;
            _appNotificacao = appNotificacao;
        }

        public Integrante Obter(long pontoDemandaId, long id)
        {
            var integrante = _repositorio.Obter(id);
            if (integrante.GruposDeIntegrantes.All(g => g.PontoDemanda.Id != pontoDemandaId)) throw new IntegranteNaoPertenceAPontoDemandaException();
            return integrante;
        }

        public Integrante Criar(long pontoDemandaId, Integrante integrante)
        {
            if (!string.IsNullOrWhiteSpace(integrante.Email)) _repositorio.VerificarSeEmailJaExiste(integrante.Email);
            integrante.GruposDeIntegrantes = new Collection<GrupoDeIntegrantes> { new GrupoDeIntegrantes { PontoDemanda = new PontoDemanda { Id = pontoDemandaId } } };
            return _repositorio.Criar(integrante);
        }

        public Integrante Atualizar(long pontoDemandaId, long usuarioId, Integrante integrante)
        {
            var integranteToUpdate = Obter(pontoDemandaId, integrante.Id);
            if (integranteToUpdate.EhUsuarioDoSistema() && usuarioId != integranteToUpdate.Usuario.Id) throw new ApplicationException(LMResource.Integrante_NaoPodeAtualizarQuemJaEhUsuario);
            if (!string.IsNullOrWhiteSpace(integrante.Email) && integranteToUpdate.Email != integrante.Email) _repositorio.VerificarSeEmailJaExiste(integrante.Email);
            integranteToUpdate.Atualizar(integrante);
            _repositorio.Salvar();
            return integranteToUpdate;
        }

        public void Desativar(long pontoDemandaId, long usuarioId, long integranteId)
        {
            var integrante = Obter(pontoDemandaId, integranteId);
            ValidarAcaoNoIntegrante(pontoDemandaId, usuarioId, integrante);
            integrante.Ativo = false;
            _repositorio.Salvar();
        }

        public void RemoverDoPonto(long pontoDemandaId, long usuarioId, long integranteId)
        {
            var integrante = Obter(pontoDemandaId, integranteId);
            ValidarAcaoNoIntegrante(pontoDemandaId, usuarioId, integrante);
            _repositorio.RemoverGrupo(integrante, pontoDemandaId);
            if (integrante.Usuario == null)
            {
                _repositorio.Remover(integrante);
            }
            else
            {
                ResetarStatus(pontoDemandaId, integrante);
            }
            _repositorio.Salvar();
        }

        private static void ValidarAcaoNoIntegrante(long pontoDemandaId, long usuarioId, Integrante integrante)
        {
            if (integrante.Usuario == null) return;
            if (integrante.Usuario.Id == usuarioId) throw new ApplicationException(LMResource.Integrante_NaoPodeDesativar);
            if (integrante.GruposDeIntegrantes.Single(g => g.PontoDemanda.Id == pontoDemandaId).PontoDemanda.UsuarioCriador.Id == integrante.Usuario.Id)
                throw new ApplicationException("Não pode excluir o criador da casa.");
        }

        private static void ResetarStatus(long pontoDemandaId, Integrante integrante)
        {
            if(integrante.GruposDeIntegrantes.Any()) return;
            integrante.Usuario.StatusUsuarioPontoDemanda.Add(new StatusUsuarioPontoDemanda
            {
                StatusCadastro = StatusCadastro.EtapaDeInformacoesPessoaisCompleta,
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now,
                PontoDemandaId = pontoDemandaId
            });
        }

        public void Convidar(long pontoDemandaId, long usuarioId, long id, string imageHost)
        {
            var convidado = Obter(pontoDemandaId, id);
            if (!convidado.PodeSerConvidado()) throw new ApplicationException(LMResource.Integrante_NaoPodeSerConvidado);
            convidado.DataConvite = DateTime.Now;
            convidado.EhUsuarioConvidado = true;
            var pontoDemanda = convidado.GruposDeIntegrantes.Single(g => g.PontoDemanda.Id == pontoDemandaId).PontoDemanda;
            var remetente = pontoDemanda.GruposDeIntegrantes.Single(g => g.Integrante.Usuario != null && g.Integrante.Usuario.Id == usuarioId).Integrante;
            var extraParams = new { ImageHost = imageHost };
            _appNotificacao.Notificar(remetente, convidado, pontoDemanda, TipoTemplateMensagem.ConviteIntegrante, extraParams);
            _repositorio.Salvar();
        }

        public IEnumerable<Animal> Animais()
        {
            return _repositorio.Animais();
        }
    }
}
