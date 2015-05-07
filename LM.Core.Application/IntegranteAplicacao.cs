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
        Integrante Atualizar(long pontoDemandaId, Integrante integrante);
        void Apagar(long pontoDemandaId, long integranteId);
        void Convidar(long pontoDemandaId, long usuarioId, long id);
        void VerificarSeCpfJaExiste(string cpf);
        void VerificarSeEmailJaExiste(string email);
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
            return _repositorio.Criar(integrante);
        }

        public Integrante Atualizar(long pontoDemandaId, Integrante integrante)
        {
            var integranteToUpdate = Obter(pontoDemandaId, integrante.Id);
            if (integranteToUpdate.Email != integrante.Email) VerificarSeEmailJaExiste(integrante.Email);
            integranteToUpdate.Atualizar(integrante);
            _repositorio.Salvar();
            return integranteToUpdate;
        }

        public void Apagar(long pontoDemandaId, long integranteId)
        {
            var integrante = Obter(pontoDemandaId, integranteId);
            _repositorio.Apagar(integrante);
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

        public void VerificarSeCpfJaExiste(string cpf)
        {
            _repositorio.VerificarSeCpfJaExiste(cpf);
        }

        public void VerificarSeEmailJaExiste(string email)
        {
            _repositorio.VerificarSeEmailJaExiste(email);
        }
    }
}
