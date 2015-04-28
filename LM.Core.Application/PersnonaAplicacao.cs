using System;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IPersonaAplicacao
    {
        IList<Persona> Listar();
        Persona Obter(int idade, string sexo, string perfil);
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

        public Persona Obter(int idade, string sexo, string perfil)
        {
            var personas = Listar();
            if (perfil.ToLower() == "pet") return personas.First(p => p.Perfil.StartsWith("PET"));
            if (perfil.ToLower() == "empregado") return personas.First(p => p.Perfil.StartsWith("EMPREGADO") && p.Sexo == sexo);
            personas = personas.Where(p => p.Perfil != "EMPREGADO" && !p.Perfil.StartsWith("PET")).ToList();
            var persona = personas.SingleOrDefault(p => p.IdadeInicial <= idade && p.IdadeFinal >= idade && p.Sexo == sexo);
            if (persona == null) throw new ApplicationException(string.Format("Não foi possível selecionar uma persona a partir da idade e sexo informados. Idade: {0} - Sexo: {1}", idade, sexo));
            return persona;
        }
    }
}
