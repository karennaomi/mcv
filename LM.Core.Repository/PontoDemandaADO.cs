using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LM.Core.Repository
{
    public class PontoDemandaADO : IRepositorioPontoDemanda
    {
        public PontoDemanda Criar(PontoDemanda pontoDemanda)
        {
            throw new NotImplementedException("Use o metodo da classe PontoDemandaEF do pacote LM.Core.RepositorioEF");
        }

        public void DefinirFrequenciaDeConsumo(long pontoDemandaId, long usuarioId, int frequencia)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdPontoDemanda", pontoDemandaId),
                new SqlParameter("@IdUsuario", usuarioId),
                new SqlParameter("@FrequenciaCompra", frequencia),
            };
            
            using (var contexto = new ContextoADO())
            {
                contexto.ExecutaProcedure("SP_WEB_ATUALIZA_FREQUENCIA_COMPRA", parameters);
            }
        }


        public IList<PontoDemanda> Listar(long usuarioId)
        {
            throw new NotImplementedException("Use o metodo da classe PontoDemandaEF do pacote LM.Core.RepositorioEF");
        }

        public PontoDemanda Obter(long id, long usuarioId)
        {
            throw new NotImplementedException("Use o metodo da classe PontoDemandaEF do pacote LM.Core.RepositorioEF");
        }
    }
}
