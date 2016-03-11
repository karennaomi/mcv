using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IPlanoAplicacao
    {
        IList<Plano> Listar();
        Plano Obter(int id);
    }

    public class PlanoAplicacao : IPlanoAplicacao
    {
        private readonly IRepositorioPlano _repositorio;
        public PlanoAplicacao(IRepositorioPlano repositorio)
        {
            _repositorio = repositorio;
        }

        public IList<Plano> Listar()
        {
            return _repositorio.Listar();
        }

        public Plano Obter(int id)
        {
            return _repositorio.Obter(id);
        }
    }
}
