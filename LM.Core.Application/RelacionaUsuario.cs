
namespace LM.Core.Application
{
    public interface IRelacionaUsuario
    {
        long UsuarioId { get; set; }
    }
    
    public abstract class RelacionaUsuario : IRelacionaUsuario
    {
        public long UsuarioId { get; set; }
        protected RelacionaUsuario(long usuarioId)
        {
            UsuarioId = usuarioId;
        }
    }
}
