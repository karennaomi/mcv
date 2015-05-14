﻿using System;
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
            DataInclusao = DataAlteracao = DateTime.Now;
            Ativo = true;
            if(categoriaId > 0) Categorias = new Collection<Categoria> {new Categoria {Id = categoriaId}};
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "O codigo EAN do produto é obrigatório.")]
        public string Ean { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public virtual ProdutoInfo Info { get; set; }
        public virtual ICollection<Imagem> Imagens { get; set; }
        public virtual ICollection<Categoria> Categorias { get; set; }

        public string Nome()
        {
            return Info == null ? "Produto sem informação." : Info.Nome;
        }

        public Imagem ImagemPrincipal()
        {
            return Imagens.Any() ? Imagens.First() : new Imagem();
        }
    }

    public class ProdutoInfo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
    }
}
