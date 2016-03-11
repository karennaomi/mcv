using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IEmailCapturadoAplicacao
    {
        IEnumerable<EmailCapturado> Listar();
        EmailCapturado Criar(EmailCapturado emailCapturado);
    }

    public class EmailCapturadoAplicacao : IEmailCapturadoAplicacao
    {
        private readonly IRepositorioEmailCapturado _repositorio;
        public EmailCapturadoAplicacao(IRepositorioEmailCapturado repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<EmailCapturado> Listar()
        {
            return _repositorio.Listar();
        }

        public EmailCapturado Criar(EmailCapturado emailCapturado)
        {
            return _repositorio.Criar(emailCapturado);
        }
    }
}
