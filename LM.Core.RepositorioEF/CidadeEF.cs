using System.Collections.Generic;
using System.Data.Entity;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class CidadeEF : IRepositorioCidade
    {
        private readonly ContextoEF _contexto;
        public CidadeEF()
        {
            _contexto = new ContextoEF();
        }
        public CidadeEF(ContextoEF contexto)
        {
            _contexto = contexto;
        }

        public Cidade Buscar(string nome)
        {
            try
            {
                var cidade = _contexto.Cidades.AsNoTracking().SingleOrDefault(c => c.Nome == nome);
                if (cidade == null) return null;
                var cidadeLocal = _contexto.Cidades.Local.SingleOrDefault(c => c.Id == cidade.Id);
                if (cidadeLocal != null)
                {
                    return cidadeLocal;
                }
                _contexto.Entry(cidade).State = EntityState.Unchanged;
                return cidade;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public IList<Cidade> Listar(int ufId)
        {
            return _contexto.Cidades.AsNoTracking().Where(c => c.Uf.Id == ufId).ToList();
        }
    }
}
