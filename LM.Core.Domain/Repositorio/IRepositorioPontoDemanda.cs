using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioPontoDemanda
    {
        PontoDemanda Criar(long usuarioId, PontoDemanda novoPontoDemanda);
        PontoDemanda Atualizar(long usuarioId, PontoDemanda pontoDemanda);
        Loja AdicionarLojaFavorita(long usuarioId, PontoDemanda pontoDemanda, Loja loja);
        IList<PontoDemanda> Listar(long usuarioId);
        PontoDemanda Obter(long usuarioId, long pontoDemandaId);
        void Salvar();
    }
}
