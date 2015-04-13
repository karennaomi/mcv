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
                if (cidade == null) throw new ApplicationException(string.Format("A cidade {0} não foi encontrada na nossa base de dados.", nome));
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
                throw new ApplicationException("Foram encontradas mais de uma cidade para o nome informado.");
            }
        }

        public void LimparCidadeNovas()
        {
            var cidadesLocaisNovas = _contexto.Cidades.Local.Where(c => c.Id == 0).ToList();
            foreach (var cidadeLocal in cidadesLocaisNovas)
            {
                _contexto.Cidades.Local.Remove(cidadeLocal);
            }
        }
    }
}
