using System;

namespace LM.Core.Domain.CustomException
{
    public class PontoDemandaInvalidoException : ApplicationException
    {
        public PontoDemandaInvalidoException(string message) : base(message) { }
    }
}
