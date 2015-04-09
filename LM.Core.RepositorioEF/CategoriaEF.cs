﻿using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class CategoriaEF : IRepositorioCategoria
    {
        private readonly ContextoEF _contexto;
        public CategoriaEF()
        {
            _contexto = new ContextoEF();
        }

        public IList<Categoria> Listar(int secaoId)
        {
            return _contexto.Categorias.AsNoTracking().Where(c => c.CategoriaPai.Id == secaoId).OrderBy(c => c.Nome).ToList();
        }
    }
}
