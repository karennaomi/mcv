using System.Collections.Generic;
using System.Linq;

namespace LM.Core.Domain
{
    public class Produto
    {
        public int Id { get; set; }
        public virtual ProdutoInfo Info { get; set; }
        public virtual ICollection<Imagem> Imagens { get; set; }
        public virtual ICollection<Categoria> Categorias { get; set; }

        public string Nome()
        {
            return Info == null ? "Produto sem informação." : Info.Nome;
        }

        public Imagem ImagemPrincipal()
        {
            return Imagens.First();
        }
    }

    public class ProdutoInfo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
    }
}
