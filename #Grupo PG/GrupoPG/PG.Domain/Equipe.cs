using System.Collections.Generic;

namespace PG.Domain
{
    public class Equipe
    {
        public int Id { get; set; }
        public string NomeEquipe { get; set; }
        public virtual ICollection<Funcionario> IFuncionarios { get; set; }

        

    }
}