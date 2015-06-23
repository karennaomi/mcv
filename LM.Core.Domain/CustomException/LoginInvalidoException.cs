using System;

namespace LM.Core.Domain.CustomException
{
    public class LoginInvalidoException : ApplicationException
    {
        public LoginInvalidoException(string message = "E-mail ou Senha não conferem!") : base(message)
        {
        }
    }
}
