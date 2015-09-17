using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IAssinaturaAplicacao
    {
        Assinatura Obter(int id);
        IList<Assinatura> Listar(int usuarioId);
        Assinatura Criar(Assinatura assinatura);
    }

    public class AssinaturaAplicacao : IAssinaturaAplicacao
    {
        private readonly IRepositorioAssinatura _repositorio;
        public AssinaturaAplicacao(IRepositorioAssinatura repositorio)
        {
            _repositorio = repositorio;
        }

        public Assinatura Obter(int id)
        {
            return _repositorio.Obter(id);
        }

        public IList<Assinatura> Listar(int usuarioId)
        {
            return _repositorio.Listar(usuarioId);
        }

        public Assinatura Criar(Assinatura assinatura)
        {
            return _repositorio.Criar(assinatura);
        }
    }
}
