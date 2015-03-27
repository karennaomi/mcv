using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System;

namespace LM.Core.Tests
{
    public interface ICompraAtivaAplicacao : IRelacionaPontoDemanda
    {
        CompraAtiva AtivarCompra();
        CompraAtiva FinalizarCompra();
    }

    public class CompraAtivaAplicacao : RelacionaPontoDemandaUsuario, ICompraAtivaAplicacao
    {
        private readonly IRepositorioCompraAtiva _compraAtivaRepo;
        public CompraAtivaAplicacao(IRepositorioCompraAtiva compraAtivaRepo, long pontoDemandaId, long usuarioId)
            : base(pontoDemandaId, usuarioId)
        {
            _compraAtivaRepo = compraAtivaRepo;
        }

        public CompraAtiva AtivarCompra()
        {
            return _compraAtivaRepo.AtivarCompra(PontoDemandaId, UsuarioId);
        }


        public CompraAtiva FinalizarCompra()
        {
            var compraAtiva = _compraAtivaRepo.Obter(PontoDemandaId, UsuarioId);
            if (compraAtiva == null) throw new ApplicationException("Nenhuma compra ativa para o ponto de demanda especificado.");
            return _compraAtivaRepo.FinalizarCompra(compraAtiva);
        }
    }
}
