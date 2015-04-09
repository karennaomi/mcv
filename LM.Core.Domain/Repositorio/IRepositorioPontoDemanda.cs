using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioPontoDemanda
    {
        PontoDemanda Criar(long usuarioId, PontoDemanda novoPontoDemanda);
        IList<PontoDemanda> Listar(long usuarioId);
        PontoDemanda Obter(long usuarioId, long pontoDemandaId);
        void SalvarAlteracoes();
    }
}
