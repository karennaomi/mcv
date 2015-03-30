using System;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface ICompraAtivaAplicacao : IRelacionaPontoDemanda, IRelacionaUsuario
    {
        CompraAtiva AtivarCompra();
        CompraAtiva FinalizarCompra();
    }

    public class CompraAtivaAplicacao : RelacionaPontoDemandaUsuario, ICompraAtivaAplicacao
    {
        private readonly IRepositorioCompraAtiva _compraAtivaRepo;
        private readonly IPontoDemandaAplicacao _appPontoDemanda;
        private readonly INotificacaoAplicacao _appNotificacao;
        public CompraAtivaAplicacao(IRepositorioCompraAtiva compraAtivaRepo, IPontoDemandaAplicacao appPontoDemanda, INotificacaoAplicacao appNotificacao) : this(compraAtivaRepo, appPontoDemanda, appNotificacao, 0, 0) { }
        public CompraAtivaAplicacao(IRepositorioCompraAtiva compraAtivaRepo, IPontoDemandaAplicacao appPontoDemanda, INotificacaoAplicacao appNotificacao, long pontoDemandaId, long usuarioId) : base(pontoDemandaId, usuarioId)
        {
            _compraAtivaRepo = compraAtivaRepo;
            _appPontoDemanda = appPontoDemanda;
            _appNotificacao = appNotificacao;
        }

        public CompraAtiva AtivarCompra()
        {
            var compraAtiva = _compraAtivaRepo.AtivarCompra(PontoDemandaId, UsuarioId);
            NotificarIntegrantes();
            return compraAtiva;
        }

        private void NotificarIntegrantes()
        {
            _appPontoDemanda.UsuarioId = UsuarioId;
            var pontoDemanda = _appPontoDemanda.Obter(PontoDemandaId);
            var integrantesComUsuario = pontoDemanda.GrupoDeIntegrantes.Integrantes.Where(i => i.Usuario != null).ToList();
            if(!integrantesComUsuario.Any()) return;
            var usuarios = integrantesComUsuario.Where(i => i.Usuario.Id != UsuarioId).Select(i => i.Usuario);
            foreach (var usuario in usuarios)
            {
                _appNotificacao.EnviarNotificacao(usuario.DeviceType, usuario.DeviceId, "O modo de compra foi habilitado no ponto de demanda " + pontoDemanda.Nome, "compras");
            }
        }

        public CompraAtiva FinalizarCompra()
        {
            var compraAtiva = _compraAtivaRepo.Obter(PontoDemandaId, UsuarioId);
            if (compraAtiva == null) throw new ApplicationException("Nenhuma compra ativa para o ponto de demanda especificado.");
            return _compraAtivaRepo.FinalizarCompra(compraAtiva);
        }
    }
}
