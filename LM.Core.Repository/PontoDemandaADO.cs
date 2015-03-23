using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LM.Core.Repository
{
    public class PontoDemandaADO : IRepositorioPontoDemanda
    {
        public PontoDemanda Salvar(PontoDemanda pontoDemanda)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdPtoDemanda", pontoDemanda.Id),
                new SqlParameter("@IdUsuario", pontoDemanda.Usuario.Id),
                new SqlParameter("@NmEndereco", pontoDemanda.Endereco.Descricao),
                new SqlParameter("@CEP", pontoDemanda.Endereco.Cep),
                new SqlParameter("@Bairro", pontoDemanda.Endereco.Bairro),
                new SqlParameter("@Latitude", pontoDemanda.Endereco.Latitude),
                new SqlParameter("@Longitude", pontoDemanda.Endereco.Longitude),
                new SqlParameter("@Cidade", pontoDemanda.Endereco.Cidade.Nome),
                new SqlParameter("@NumeroEndereco", pontoDemanda.Endereco.Numero),
                new SqlParameter("@NomePontoDemanda", pontoDemanda.Nome)
            };

            using (var contexto = new ContextoADO())
            {
                pontoDemanda.Id = contexto.ExecutaProcedureEscalar("SP_WEB_SALVA_PONTO_DEMANDA", parameters);
                return pontoDemanda;
            }
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
