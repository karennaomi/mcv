using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface ILegacyTokenAplicacao
    {
        LegacyToken Criar(long usuarioId);
    }

    public class LegacyTokenAplicacao : ILegacyTokenAplicacao
    {
        private readonly IRepositorioLegacyToken _repositorio;
        public LegacyTokenAplicacao(IRepositorioLegacyToken repositorio)
        {
            _repositorio = repositorio;
        }

        public LegacyToken Criar(long usuarioId)
        {
            return _repositorio.Criar(usuarioId);
        }
    }
}
