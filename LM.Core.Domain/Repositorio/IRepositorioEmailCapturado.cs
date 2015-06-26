using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioEmailCapturado
    {
        IEnumerable<EmailCapturado> Listar();
        EmailCapturado Criar(EmailCapturado emailCapturado);
    }
}
