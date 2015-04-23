using System;
namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioRecuperarSenha
    {
        RecuperarSenha Criar(RecuperarSenha recuperarSenha);
        RecuperarSenha ObterPorToken(Guid token);
        void Salvar();
    }
}
