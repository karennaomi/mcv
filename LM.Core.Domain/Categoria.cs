
using System.Collections.Generic;
namespace LM.Core.Domain
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public virtual Categoria CategoriaPai { get; set; }
        public virtual ICollection<Categoria> SubCategorias { get; set; }
    }
}
