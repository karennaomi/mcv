using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.RepositorioEF
{
    public class LegacyTokenEF : IRepositorioLegacyToken
    {
        private readonly ContextoEF _contexto;
        public LegacyTokenEF()
        {
            _contexto = new ContextoEF();
        }

        public LegacyToken Criar(long usuarioId)
        {
            var legacyToken = new LegacyToken {UsuarioId = usuarioId};
            legacyToken = _contexto.LegacyTokens.Add(legacyToken);
            return legacyToken;
        }
    }
}
