using LM.Core.Domain;
using LM.Core.Repository.EntityFramework;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace LM.Core.Repository
{
    public interface IRepositorioPontoDemanda
    {
        PontoDemanda Salvar(PontoDemanda pontoDemanda);
        IList<PontoDemanda> Listar(long usuarioId);
        PontoDemanda Obter(long id, long usuarioId);
        void DefinirFrequenciaDeConsumo(long pontoDemandaId, long usuarioId, int frequencia);
    }

    public class PontoDemandaADO : IRepositorioPontoDemanda
    {
        private readonly ContextoEF _contextoEF;
        public PontoDemandaADO()
        {
            _contextoEF = new ContextoEF();
        }

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

        public IList<PontoDemanda> Listar(long usuarioId)
        {
            return _contextoEF.PontosDemanda.Where(d => d.GrupoDeIntegrantes.Integrantes.Any(i => i.Usuario.Id == usuarioId)).ToList();
        }

        public PontoDemanda Obter(long id, long usuarioId)
        {
            var pontoDemanda = _contextoEF.PontosDemanda.SingleOrDefault(d => d.GrupoDeIntegrantes.Integrantes.Any(i => i.Usuario.Id == usuarioId) && d.Id == id);
            return pontoDemanda;
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
    }
}
