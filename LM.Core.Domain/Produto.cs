using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LM.Core.Domain
{
    public class Produto
    {
        public Produto() { }

        public Produto(string nome, string ean, string origem, int categoriaId, string imagemPath = "")
        {
            DataInclusao = DateTime.Now;
            Ativo = true;
            Ean = ean;
            Origem = origem;
            DefinirNome(nome);
            DefinirImagem(imagemPath);
            DefinirCategoria(categoriaId);
        }

        private void DefinirNome(string nome)
        {
            Info = new ProdutoInfo {Nome = nome};
        }

        private void DefinirImagem(string imagemPath)
        {
            if (Imagens == null) Imagens = new Collection<Imagem>();
            Imagens.Add(string.IsNullOrEmpty(imagemPath) ? new Imagem { Id = 1 } : new Imagem { Interface = ImagemInterface.All, Resolucao = ImagemResolucao.NotAssigned, Path = imagemPath });
        }

        private void DefinirCategoria(int categoriaId)
        {
            if (categoriaId <= 0) categoriaId = 3;
            Categorias = new Collection<Categoria> { new Categoria { Id = categoriaId } };
        }

        public int Id { get; set; }
        [LMMaxLength(Constantes.Produto.TamanhoMaximoEan)]
        public string Ean { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string Origem { get; set; }
        public virtual ProdutoInfo Info { get; set; }
        public virtual ICollection<Imagem> Imagens { get; set; }
        public virtual ICollection<Categoria> Categorias { get; set; }
        public virtual ICollection<PontoDemanda> PontosDemanda { get; set; }
        public virtual ICollection<ProdutoPreco> Precos { get; set; }

        public string Nome()
        {
            return Info == null ? "Produto sem informação." : Info.Nome;
        }

        public Imagem ImagemPrincipal()
        {
            return Imagens != null && Imagens.Any() ? Imagens.First() : new Imagem();
        }

        public static Func<Produto, bool> ProtectProductPredicate(long pontoDemandaId)
        {
            return
                (produto =>
                    ((produto.PontosDemanda == null || !produto.PontosDemanda.Any() ||
                      produto.PontosDemanda.Any(pontoDemanda => pontoDemanda.Id == pontoDemandaId))));
        }

        public decimal PrecoMedio()
        {
            if (Precos == null) return 0;
            var preco = Precos.FirstOrDefault(p => p.Ativo);
            if (preco == null) return 0;
            if (preco.PrecoMax.HasValue && preco.PrecoMin.HasValue)
            {
                return (preco.PrecoMax.Value + preco.PrecoMin.Value) / 2;
            }
            if (preco.PrecoMin.HasValue) 
            {
                return preco.PrecoMin.Value;
            }
            return preco.PrecoMax.HasValue ? preco.PrecoMax.Value : 0;
        }
    }

    public class ProdutoInfo
    {
        public int Id { get; set; }
        [LMRequired]
        public string Nome { get; set; }
        public string Marca { get; set; }
    }
}
