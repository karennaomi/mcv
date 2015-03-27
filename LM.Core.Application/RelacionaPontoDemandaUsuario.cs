
namespace LM.Core.Application
{

    public abstract class RelacionaPontoDemandaUsuario : IRelacionaPontoDemanda, IRelacionaUsuario
    {
        public long PontoDemandaId { get; set; }
        public long UsuarioId { get; set; }
        protected RelacionaPontoDemandaUsuario(long pontoDemandaId, long usuarioId)
        {
            PontoDemandaId = pontoDemandaId;
            UsuarioId = usuarioId;
        }
    }
}
