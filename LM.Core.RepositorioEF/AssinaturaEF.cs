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
            assinatura = _contexto.Assinaturas.Add(assinatura);
            _contexto.SaveChanges();
            return assinatura;
        }
    }
}
