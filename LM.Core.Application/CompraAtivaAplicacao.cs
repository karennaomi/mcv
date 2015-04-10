using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface ICompraAtivaAplicacao
    {
        CompraAtiva Obter(long pontoDemandaId);
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

        public CompraAtiva Obter(long pontoDemandaId)
        {
            return _compraAtivaRepo.Obter(pontoDemandaId);
        }

        public CompraAtiva AtivarCompra(long usuarioId, long pontoDemandaId)
        {
            var compraAtiva = _compraAtivaRepo.AtivarCompra(usuarioId, pontoDemandaId);
            _appNotificacao.NotificarIntegrantesDoPontoDamanda(compraAtiva.Usuario, compraAtiva.PontoDemanda, TipoTemplateMensagem.AtivarCompra, "compras");
            return compraAtiva;
        }

        public CompraAtiva FinalizarCompra(long usuarioId, long pontoDemandaId)
        {
            var compraAtiva = _compraAtivaRepo.FinalizarCompra(usuarioId, pontoDemandaId);
            _appNotificacao.NotificarIntegrantesDoPontoDamanda(compraAtiva.Usuario, compraAtiva.PontoDemanda, TipoTemplateMensagem.FinalizarCompra, "compras");
            return compraAtiva;
        }
    }
}
