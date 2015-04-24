using System;

namespace LM.Core.Domain
{
    public class LMHelper
    {
        public static int ObterIdade(DateTime? dataNascimento)
        {
            if (!dataNascimento.HasValue) throw new ApplicationException("Data de nascimento inválida");
            var hoje = DateTime.Today;
            var idade = hoje.Year - dataNascimento.Value.Year;
            if (dataNascimento.Value.Date > hoje.Date.AddYears(-idade)) idade--;
            return idade;
        }
    }
}
