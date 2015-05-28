using System;
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
        void Convidar(long pontoDemandaId, long usuarioId, long id);
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
            if (integranteToUpdate.EhUsuarioDoSistema() && usuarioId != integranteToUpdate.Usuario.Id) throw new ApplicationException("Não pode atualizar um usuário do sistema.");
            if (!string.IsNullOrWhiteSpace(integrante.Email) && integranteToUpdate.Email != integrante.Email) _repositorio.VerificarSeEmailJaExiste(integrante.Email);
            integranteToUpdate.Atualizar(integrante);
            _repositorio.Salvar();
            return integranteToUpdate;
        }

        public void Desativar(long pontoDemandaId, long usuarioId, long integranteId)
        {
            var integrante = Obter(pontoDemandaId, integranteId);
            if(integrante.Usuario != null)
            { 
                if (integrante.Usuario.Id == usuarioId) throw new ApplicationException("Não pode desativar integrante.");
                if (integrante.GruposDeIntegrantes.Single(g => g.PontoDemanda.Id == pontoDemandaId).PontoDemanda.UsuarioCriador.Id == integrante.Usuario.Id) throw new ApplicationException("Não pode excluir o criador da casa.");
            }
            integrante.Ativo = false;
            _repositorio.Salvar();
        }

        public void Convidar(long pontoDemandaId, long usuarioId, long id)
        {
            var convidado = Obter(pontoDemandaId, id);
            if (!convidado.PodeSerConvidado()) throw new ApplicationException("Este integrante não pode ser convidado.");
            convidado.DataConvite = DateTime.Now;
            var pontoDemanda = convidado.GruposDeIntegrantes.Single(g => g.PontoDemanda.Id == pontoDemandaId).PontoDemanda;
            var integrante = convidado.GruposDeIntegrantes.Single(g => g.Integrante.Usuario != null && g.Integrante.Usuario.Id == usuarioId).Integrante;
            _appNotificacao.Notificar(integrante, convidado, pontoDemanda, TipoTemplateMensagem.ConviteIntegrante, null);
            _repositorio.Salvar();
        }
    }
}
