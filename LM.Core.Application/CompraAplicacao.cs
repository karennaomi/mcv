using System;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface ICompraplicacao
    {
        Compra Criar(Compra compra);
    }

    public class CompraAplicacao : ICompraplicacao
    {
        private readonly IRepositorioCompra _compraRepo;
        public CompraAplicacao(IRepositorioCompra compraRepo)
        {
            _compraRepo = compraRepo;
        }

        public Compra Criar(Compra compra)
        {
            if(compra.Itens == null || !compra.Itens.Any()) throw new ApplicationException("A compra deve possuir itens.");
            return _compraRepo.Criar(compra);
        }
    }
}
