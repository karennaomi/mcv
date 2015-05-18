using System;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System.Linq;

namespace LM.Core.Application
{
    public interface IIntegranteAplicacao
    {
        Integrante Obter(long pontoDemandaId, long id);
        Integrante Criar(Integrante integrante);
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
            if (integrante.GrupoDeIntegrantes.PontosDemanda.All(p => p.Id != pontoDemandaId)) throw new IntegranteNaoPertenceAPontoDemandaException();
            return integrante;
        }

        public Integrante Criar(Integrante integrante)
        {
            if (!string.IsNullOrWhiteSpace(integrante.Email)) _repositorio.VerificarSeEmailJaExiste(integrante.Email);
            return _repositorio.Criar(integrante);
        }

        public Integrante Atualizar(long pontoDemandaId, long usuarioId, Integrante integrante)
        {
            var integranteToUpdate = Obter(pontoDemandaId, integrante.Id);
            if (usuarioId != integranteToUpdate.Usuario.Id && integranteToUpdate.EhUsuarioDoSistema()) throw new ApplicationException("Não pode atualizar um usuário do sistema.");
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
                if (integrante.GrupoDeIntegrantes.PontosDemanda.Single(p => p.Id == pontoDemandaId).UsuarioCriador.Id == integrante.Usuario.Id) throw new ApplicationException("Não pode excluir o criador da casa.");
            }
            integrante.Ativo = false;
            _repositorio.Salvar();
        }

        public void Convidar(long pontoDemandaId, long usuarioId, long id)
        {
            var convidado = Obter(pontoDemandaId, id);
            if (!convidado.PodeSerConvidado()) throw new ApplicationException("Este integrante não pode ser convidado.");
            convidado.DataConvite = DateTime.Now;
            var pontoDemanda = convidado.GrupoDeIntegrantes.PontosDemanda.Single(p => p.Id == pontoDemandaId);
            var integrante = convidado.GrupoDeIntegrantes.Integrantes.Single(i => i.Usuario != null && i.Usuario.Id == usuarioId);
            _appNotificacao.Notificar(integrante, convidado, pontoDemanda, TipoTemplateMensagem.ConviteIntegrante, null);
            _repositorio.Salvar();
        }
    }
}
