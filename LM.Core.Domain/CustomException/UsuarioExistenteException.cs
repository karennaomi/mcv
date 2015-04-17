using System;

namespace LM.Core.Domain.CustomException
{
    public class UsuarioExistenteException : ApplicationException
    {
        public string Campo { get; set; }
        public UsuarioExistenteException() { }

        public UsuarioExistenteException(string campo)
            : base(string.Format("O {0} informado já está cadastrado!", campo))
        {
            Campo = campo;
        }
    }
}
