using System;

namespace LM.Core.Domain.CustomException
{
    public class IntegranteNaoPertenceAPontoDemandaException : ApplicationException
    {
        public IntegranteNaoPertenceAPontoDemandaException() : base("Este integrante não pertence ao local atual.")
        {
        }
    }
}
