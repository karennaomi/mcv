using System;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System.Linq;

namespace LM.Core.Application
{
    public interface IIntegranteAplicacao
    {
        Integrante Criar(Integrante integrante);
        void Apagar(long id, long pontoDemandaId);
    }

    public class IntegranteAplicacao : IIntegranteAplicacao
    {
        private readonly IRepositorioIntegrante _repositorio;
        private readonly IPersonaAplicacao _appPersona;
        private readonly IPontoDemandaAplicacao _appPontoDemanda;
        public IntegranteAplicacao(IRepositorioIntegrante repositorio, IPersonaAplicacao appPersona, IPontoDemandaAplicacao appPontoDemanda)
        {
            _repositorio = repositorio;
            _appPersona = appPersona;
            _appPontoDemanda = appPontoDemanda;
        }

        public Integrante Criar(Integrante integrante)
        {
            if (integrante.Persona == null) integrante.Persona = ObterPersonaDoUsuario(integrante.Usuario);
            return _repositorio.Criar(integrante);
        }

        private Persona ObterPersonaDoUsuario(Usuario usuario)
        {
            var personas = _appPersona.Listar().Where(p => p.Perfil != "EMPREGADO");
            var idadeUsuario = usuario.ObterIdade();
            var persona = personas.SingleOrDefault(p => p.IdadeInicial <= idadeUsuario && p.IdadeFinal >= idadeUsuario && p.Sexo == usuario.Sexo);
            if (persona == null) throw new ApplicationException("Não foi possível selecionar uma persona a partir do usuário atual.");
            return persona;
        }

        public void Apagar(long id, long pontoDemandaId)
        {
            var pontoDemanda = _appPontoDemanda.Obter(pontoDemandaId);
            if (pontoDemanda.GrupoDeIntegrantes.Integrantes.All(i => i.Id != id)) throw new IntegranteNaoPertenceAPontoDemandaException();
            _repositorio.Apagar(id);
        }
    }
}
