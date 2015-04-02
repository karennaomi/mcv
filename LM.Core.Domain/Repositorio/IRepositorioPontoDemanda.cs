using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioPontoDemanda
    {
        PontoDemanda Criar(PontoDemanda pontoDemanda);
        IList<PontoDemanda> Listar(long usuarioId);
        PontoDemanda Obter(long usuarioId, long pontoDemandaId);
        void SalvarAlteracoes();
    }
}
