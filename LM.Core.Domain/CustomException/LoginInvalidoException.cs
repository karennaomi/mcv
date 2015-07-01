using System;

namespace LM.Core.Domain.CustomException
{
    public class LoginInvalidoException : ApplicationException
    {
        public LoginInvalidoException(string message) : base(message) { }
    }
}
