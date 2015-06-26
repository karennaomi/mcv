using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class EmailCapturadoEF : IRepositorioEmailCapturado
    {
        private readonly ContextoEF _contexto;
        public EmailCapturadoEF()
        {
            _contexto = new ContextoEF();
        }

        public IEnumerable<EmailCapturado> Listar()
        {
            return _contexto.EmailsCapturados.AsNoTracking().AsEnumerable();
        }

        public EmailCapturado Criar(EmailCapturado emailCapturado)
        {
            var emailCapturadoExistente = _contexto.EmailsCapturados.SingleOrDefault(e => e.Email == emailCapturado.Email);
            if (emailCapturadoExistente != null) return emailCapturadoExistente;
            emailCapturado = _contexto.EmailsCapturados.Add(emailCapturado);
            _contexto.SaveChanges();
            return emailCapturado;
        }
    }
}
