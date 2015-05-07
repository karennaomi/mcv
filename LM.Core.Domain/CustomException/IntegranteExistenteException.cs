using System;

namespace LM.Core.Domain.CustomException
{
    public class IntegranteExistenteException : ApplicationException
    {
        public string Campo { get; set; }
        public IntegranteExistenteException() { }

        public IntegranteExistenteException(string campo)
            : base(string.Format("O {0} informado já está cadastrado!", campo))
        {
            Campo = campo;
        }
    }
}
