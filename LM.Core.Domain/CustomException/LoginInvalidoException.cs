using System;

namespace LM.Core.Domain.CustomException
{
    public class LoginInvalidoException : ApplicationException
    {
        public LoginInvalidoException() : base("[Email] ou [Senha] não conferem!")
        {
        }
    }
}
