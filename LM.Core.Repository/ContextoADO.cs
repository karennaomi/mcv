using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LM.Core.Repository
{
    public class ContextoADO: IDisposable
    {
        private readonly SqlConnection _conexaoSol;
        public ContextoADO()
        {
            _conexaoSol = new SqlConnection(ConfigurationManager.ConnectionStrings["SOL"].ConnectionString);
            _conexaoSol.Open();
            
        }

        public void Dispose()
        {
            if(_conexaoSol.State == ConnectionState.Open)
                _conexaoSol.Close();
        }

        public void ExecutaProcedure(string proc, SqlParameter[] parameters = null)
        {
            var sqlCommand = new SqlCommand(proc, _conexaoSol) { CommandType = CommandType.StoredProcedure };
            if (parameters == null) sqlCommand.ExecuteNonQuery();
            sqlCommand.Parameters.AddRange(parameters);
            sqlCommand.ExecuteNonQuery();
        }
     
        public SqlDataReader ExecutaProcedureRetorno(string proc, SqlParameter[] parameters = null)
        {
            var sqlCommand = new SqlCommand(proc, _conexaoSol) { CommandType = CommandType.StoredProcedure };
            if (parameters == null) return sqlCommand.ExecuteReader();
            sqlCommand.Parameters.AddRange(parameters);
            return sqlCommand.ExecuteReader();
        }

        public int ExecutaProcedureEscalar(string proc, SqlParameter[] parameters = null)
        {
            var sqlCommand = new SqlCommand(proc, _conexaoSol) { CommandType = CommandType.StoredProcedure };
            if (parameters == null) return int.Parse(sqlCommand.ExecuteScalar().ToString());
            sqlCommand.Parameters.AddRange(parameters);
            return int.Parse(sqlCommand.ExecuteScalar().ToString());
        }
    }
}
