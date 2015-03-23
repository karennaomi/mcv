using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IPersonaAplicacao
    {
        IList<Persona> Listar();
    }

    public class PersonaAplicacao : IPersonaAplicacao
    {
        private readonly IRepositorioPersona _repositorio;
        public PersonaAplicacao(IRepositorioPersona repositorio)
        {
            _repositorio = repositorio;
        }

        public IList<Persona> Listar()
        {
            return _repositorio.Listar();
        }
    }
}
