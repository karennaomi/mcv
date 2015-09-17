using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioAssinatura
    {
        Assinatura Obter(int id);
        IList<Assinatura> Listar(int usuarioId);
        Assinatura Criar(Assinatura assinatura);
        void Salvar();
    }
}
