using System.Data.Entity;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class AssinaturaEF : IRepositorioAssinatura
    {
        private readonly ContextoEF _contexto;
        public AssinaturaEF()
        {
            _contexto = new ContextoEF();
        }

        public Assinatura Obter(int id)
        {
            return _contexto.Assinaturas.Find(id);
        }

        public IList<Assinatura> Listar(int usuarioId)
        {
            return _contexto.Assinaturas.AsNoTracking().Where(a => a.Usuario.Id == usuarioId).ToList();
        }

        public Assinatura Criar(Assinatura assinatura)
        {
            _contexto.Entry(assinatura.Usuario).State = EntityState.Unchanged;
            _contexto.Entry(assinatura.Plano).State = EntityState.Unchanged;
            return _contexto.Assinaturas.Add(assinatura);
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
