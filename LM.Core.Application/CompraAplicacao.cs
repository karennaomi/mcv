using System;
using System.Collections.Generic;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface ICompraAplicacao
    {
        Compra Criar(Compra compra);
    }

    public class CompraAplicacao : ICompraAplicacao
    {
        private readonly IRepositorioCompra _compraRepo;
        public CompraAplicacao(IRepositorioCompra compraRepo)
        {
            _compraRepo = compraRepo;
        }

        public Compra Criar(Compra compra)
        {
            compra.Validar();
            return _compraRepo.Criar(compra);
        }
    }
}
