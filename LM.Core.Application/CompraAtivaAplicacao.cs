using System;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface ICompraAtivaAplicacao
    {
        CompraAtiva AtivarCompra(long usuarioId, long pontoDemandaId);
        CompraAtiva FinalizarCompra(long usuarioId, long pontoDemandaId);
    }

    public class CompraAtivaAplicacao : ICompraAtivaAplicacao
    {
        private readonly IRepositorioCompraAtiva _compraAtivaRepo;
        private readonly INotificacaoAplicacao _appNotificacao;
        public CompraAtivaAplicacao(IRepositorioCompraAtiva compraAtivaRepo, INotificacaoAplicacao appNotificacao)
        {
            _compraAtivaRepo = compraAtivaRepo;
            _appNotificacao = appNotificacao;
        }

        public CompraAtiva AtivarCompra(long usuarioId, long pontoDemandaId)
        {
            var compraAtiva = _compraAtivaRepo.AtivarCompra(usuarioId, pontoDemandaId);
            _appNotificacao.NotificarIntegrantesDoPontoDamanda(usuarioId, pontoDemandaId, TipoTemplateMensagem.AtivarCompra, "compras");
            return compraAtiva;
        }

        public CompraAtiva FinalizarCompra(long usuarioId, long pontoDemandaId)
        {
            var compraAtiva = _compraAtivaRepo.Obter(usuarioId, pontoDemandaId);
            if (compraAtiva == null) throw new ApplicationException("Nenhuma compra ativa para o ponto de demanda especificado.");
            compraAtiva = _compraAtivaRepo.FinalizarCompra(compraAtiva);
            _appNotificacao.NotificarIntegrantesDoPontoDamanda(usuarioId, pontoDemandaId, TipoTemplateMensagem.FinalizarCompra, "compras");
            return compraAtiva;
        }
    }
}
