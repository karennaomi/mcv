using System.Linq;
using LM.Core.Domain;
using LM.Core.RepositorioEF;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class UsuarioTests
    {
        [Test]
        [Ignore]
        public void HashOldPasswords()
        {
            var contexto = new ContextoEF();
            var usuarios = contexto.Usuarios.Where(u => !u.Senha.Contains(":"));
            foreach (var usuario in usuarios)
            {
                usuario.Senha = PasswordHash.CreateHash(usuario.Senha);
            }
            contexto.SaveChanges();
        }
    }
}
