using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LM.Core.Domain
{
    public class Produto
    {
        public Produto() : this(0)
        { }

        public Produto(int categoriaId)
        {
            DataInclusao = DateTime.Now;
            Ativo = true;
            Origem = "usuario";
            if(categoriaId > 0) Categorias = new Collection<Categoria> {new Categoria {Id = categoriaId}};
        }

        public int Id { get; set; }

        private string _ean;
        [LMRequired]
        public string Ean 
        { 
            get { return _ean; }
            set
            {
                _ean = value;
                EanValido = ValidarEan(value);
            } 
        }

        public bool EanValido { get; set; }
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

        private static bool ValidarEan(string ean)
        {
            if(string.IsNullOrWhiteSpace(ean))return false;
            
            var tamanhoEan = ean.Length;
            if (!Constantes.Produto.TamanhosEanValidos.Contains(tamanhoEan)) return false;

            var digitoVerificador = int.Parse(ean[tamanhoEan - 1].ToString());
            var digitos = ean.Substring(0, tamanhoEan - 1);
            var primeiroMultiplicador = tamanhoEan%2 == 0 ? 3 : 1;
            var soma = digitos.Select((d, i) => int.Parse(d.ToString()) * ((i % 2 == 0) ? primeiroMultiplicador : 4 - primeiroMultiplicador)).Sum();
            var resultado = ((1000 - soma) % 10);
            return resultado == digitoVerificador;
        }
    }

    public class ProdutoInfo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
    }
}
