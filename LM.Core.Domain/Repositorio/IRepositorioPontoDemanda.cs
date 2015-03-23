using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioPontoDemanda
    {
        PontoDemanda Salvar(PontoDemanda pontoDemanda);
        IList<PontoDemanda> Listar(long usuarioId);
        PontoDemanda Obter(long id, long usuarioId);
        void DefinirFrequenciaDeConsumo(long pontoDemandaId, long usuarioId, int frequencia);
    }
}
