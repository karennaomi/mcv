using System.Data.Entity;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.RepositorioEF
{
    public class RecuperarSenhaEF : IRepositorioRecuperarSenha
    {
        private readonly ContextoEF _contexto;
        public RecuperarSenhaEF()
        {
            _contexto = new ContextoEF();
        }

        public RecuperarSenha Criar(RecuperarSenha recuperarSenha)
        {
            _contexto.Entry(recuperarSenha.Usuario).State= EntityState.Unchanged;
            recuperarSenha = _contexto.RecuperarSenhas.Add(recuperarSenha);
            _contexto.SaveChanges();
            return recuperarSenha;
        }

        public RecuperarSenha ObterPorToken(System.Guid token)
        {
            return _contexto.RecuperarSenhas.Include("Usuario").SingleOrDefault(r => r.Token == token);
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
