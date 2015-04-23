using System;

namespace LM.Core.Domain.CustomException
{
    public class ObjetoNaoEncontradoException : ApplicationException
    {
        public ObjetoNaoEncontradoException(string message) : base(message) { }
    }
}
