using System;

namespace LM.Core.Domain.CustomException
{
    public class PontoDemandaNaoPertenceAoUsuarioException : ApplicationException
    {
        public PontoDemandaNaoPertenceAoUsuarioException() : base("Ponto de demanda não pertence ao usuário atual.") { }
    }
}
