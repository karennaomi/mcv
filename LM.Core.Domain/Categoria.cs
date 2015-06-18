using System.Collections.Generic;
using System.Linq;

namespace LM.Core.Domain
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cor { get; set; }
        public bool Ativo { get; set; }
        public virtual Categoria CategoriaPai { get; set; }
        public virtual ICollection<Imagem> Imagens { get; set; }
        public virtual ICollection<Categoria> SubCategorias { get; set; }

        public string ImagemPrincipal(int interfaceId, int resolucaoId = 3)
        {
            const string imagemDefault = "http://www.smvcompany.com.br/imagens-prod/placeholder.png";
            if (Imagens == null) return imagemDefault;
            var imagem = Imagens.SingleOrDefault(i => i.Interface == (ImagemInterface)interfaceId && i.Resolucao == (ImagemResolucao)resolucaoId);
            return imagem == null ? imagemDefault : imagem.Path;
        }
    }
}
