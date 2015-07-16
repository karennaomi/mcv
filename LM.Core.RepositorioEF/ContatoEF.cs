using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.RepositorioEF
{
    public class ContatoEF : IRepositorioContato
    {
        private readonly ContextoEF _contexto;
        public ContatoEF()
        {
            _contexto = new ContextoEF();
        }

        public Contato Criar(Contato contato)
        {
            contato = _contexto.Contatos.Add(contato);
            _contexto.SaveChanges();
            return contato;
        }
    }
}
