using System;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface ICompraAtivaAplicacao
    {
        CompraAtiva Obter(long pontoDemandaId);
        bool ExisteCompraAtiva(long pontoDemandaId);
        CompraAtiva AtivarCompra(long usuarioId, long pontoDemandaId);
        CompraAtiva DesativarCompra(long pontoDemandaId);
        CompraAtiva FinalizarCompra(long pontoDemandaId);
    }

    public class CompraAtivaAplicacao : ICompraAtivaAplicacao
    {
        private readonly IRepositorioCompraAtiva _repositorio;
        private readonly INotificacaoAplicacao _appNotificacao;
        public CompraAtivaAplicacao(IRepositorioCompraAtiva compraAtivaRepo, INotificacaoAplicacao appNotificacao)
        {
            _repositorio = compraAtivaRepo;
            _appNotificacao = appNotificacao;
        }

        public CompraAtiva Obter(long pontoDemandaId)
        {
            return _repositorio.Obter(pontoDemandaId);
        }

        public CompraAtiva AtivarCompra(long usuarioId, long pontoDemandaId)
        {
            var compraAtiva = _repositorio.Obter(pontoDemandaId);
            if (compraAtiva != null) return compraAtiva;
            compraAtiva = _repositorio.AtivarCompra(usuarioId, pontoDemandaId);
            _appNotificacao.NotificarIntegrantesDoPontoDamanda(compraAtiva.Usuario.Integrante, compraAtiva.PontoDemanda, TipoTemplateMensagem.AtivarCompra, new { Action = "compras" });
            return compraAtiva;
        }

        public CompraAtiva DesativarCompra(long pontoDemandaId)
        {
            var compraAtiva = DefinarDataFimCompraAtiva(pontoDemandaId, TipoTemplateMensagem.DesativarCompra);
            _repositorio.Salvar();
            return compraAtiva;
        }

        public CompraAtiva FinalizarCompra(long pontoDemandaId)
        {
            return DefinarDataFimCompraAtiva(pontoDemandaId, TipoTemplateMensagem.FinalizarCompra);
        }

        private CompraAtiva DefinarDataFimCompraAtiva(long pontoDemandaId, TipoTemplateMensagem template)
        {
            var compraAtiva = Obter(pontoDemandaId);
            compraAtiva.FimCompra = DateTime.Now;
            _appNotificacao.NotificarIntegrantesDoPontoDamanda(compraAtiva.Usuario.Integrante, compraAtiva.PontoDemanda, template, new { Action = "compras" });
            return compraAtiva;
        }

        public bool ExisteCompraAtiva(long pontoDemandaId)
        {
            return _repositorio.Obter(pontoDemandaId) != null;
        }
    }
}
