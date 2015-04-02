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
        private readonly IPontoDemandaAplicacao _appPontoDemanda;
        private readonly INotificacaoAplicacao _appNotificacao;
        public CompraAtivaAplicacao(IRepositorioCompraAtiva compraAtivaRepo, IPontoDemandaAplicacao appPontoDemanda, INotificacaoAplicacao appNotificacao)
        {
            _compraAtivaRepo = compraAtivaRepo;
            _appPontoDemanda = appPontoDemanda;
            _appNotificacao = appNotificacao;
        }

        public CompraAtiva AtivarCompra(long usuarioId, long pontoDemandaId)
        {
            var compraAtiva = _compraAtivaRepo.AtivarCompra(usuarioId, pontoDemandaId);
            NotificarIntegrantes(usuarioId, pontoDemandaId);
            return compraAtiva;
        }

        private void NotificarIntegrantes(long usuarioId, long pontoDemandaId)
        {
            var pontoDemanda = _appPontoDemanda.Obter(usuarioId, pontoDemandaId);
            var integrantesComUsuario = pontoDemanda.GrupoDeIntegrantes.Integrantes.Where(i => i.Usuario != null).ToList();
            if(!integrantesComUsuario.Any()) return;
            var usuarios = integrantesComUsuario.Where(i => i.Usuario.Id != usuarioId).Select(i => i.Usuario);
            foreach (var usuario in usuarios)
            {
                _appNotificacao.EnviarNotificacao(usuario.DeviceType, usuario.DeviceId, "O modo de compra foi habilitado no ponto de demanda " + pontoDemanda.Nome, "compras");
            }
        }

        public CompraAtiva FinalizarCompra(long usuarioId, long pontoDemandaId)
        {
            var compraAtiva = _compraAtivaRepo.Obter(usuarioId, pontoDemandaId);
            if (compraAtiva == null) throw new ApplicationException("Nenhuma compra ativa para o ponto de demanda especificado.");
            return _compraAtivaRepo.FinalizarCompra(compraAtiva);
        }
    }
}
