using System;

namespace LM.Core.Domain.CustomException
{
    public class UsuarioExistenteException : ApplicationException
    {
        public UsuarioExistenteException()
        {
        }

        public UsuarioExistenteException(long cpf) : base(string.Format("O CPF {0} informado já está cadastrado!", cpf))
        {

        }
    }
}
