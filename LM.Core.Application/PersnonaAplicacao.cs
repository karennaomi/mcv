using System.Collections.Generic;
using LM.Core.Domain;
using LM.Core.Repository;

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
