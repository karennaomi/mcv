using System;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
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
        private readonly IRepositorioCompraAtiva _repositorio;
        private readonly INotificacaoAplicacao _appNotificacao;
        public CompraAtivaAplicacao(IRepositorioCompraAtiva compraAtivaRepo, INotificacaoAplicacao appNotificacao)
        {
            _repositorio = compraAtivaRepo;
            _appNotificacao = appNotificacao;
        }

        public CompraAtiva Obter(long pontoDemandaId)
        {
            var compraAtiva = _repositorio.Obter(pontoDemandaId);
            if (compraAtiva == null) throw new ObjetoNaoEncontradoException("Nenhuma compra ativa para o ponto de demanda especificado.");
            return compraAtiva;
        }

        public CompraAtiva AtivarCompra(long usuarioId, long pontoDemandaId)
        {
            if (_repositorio.Obter(pontoDemandaId) != null) throw new ApplicationException("Já existe uma compra ativa neste ponto de demanda.");
            var compraAtiva = _repositorio.AtivarCompra(usuarioId, pontoDemandaId);
            _appNotificacao.NotificarIntegrantesDoPontoDamanda(compraAtiva.Usuario.Integrante, compraAtiva.PontoDemanda, TipoTemplateMensagem.AtivarCompra, new { Action = "compras" });
            return compraAtiva;
        }

        public CompraAtiva FinalizarCompra(long usuarioId, long pontoDemandaId)
        {
            var compraAtiva = Obter(pontoDemandaId);
            compraAtiva.FimCompra = DateTime.Now;
            _repositorio.Salvar();
            _appNotificacao.NotificarIntegrantesDoPontoDamanda(compraAtiva.Usuario.Integrante, compraAtiva.PontoDemanda, TipoTemplateMensagem.FinalizarCompra, new { Action = "compras" });
            return compraAtiva;
        }
    }
}
