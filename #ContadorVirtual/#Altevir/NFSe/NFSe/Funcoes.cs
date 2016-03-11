using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace NFSe
{
    class Funcoes
    {
        /// <summary>
        /// Método responsável por fazer a consulta de um determinado campo no xml.
        /// </summary>
        /// <param name="ds">Objeto DataSet.</param>
        /// <param name="nomeTabela">Nome da tabela a ser pesquisada.</param>
        /// <param name="linha">Número da linha a ser pesquisada.</param>
        /// <param name="campo">Nome do campo a ser pesquisado.</param>
        /// <param name="upperCase">true - converte o texto em maiúsculo.</param>
        /// <returns>Retorna uma string com o dado encontrado.</returns>
        public static string RetornarCampoXml(DataSet ds, string nomeTabela, int linha, string campo, bool upperCase)
        {
            string resultado = "";
            resultado = upperCase == true ? ds.Tables[nomeTabela].Rows[linha][campo].ToString().Trim().ToUpper() : ds.Tables[nomeTabela].Rows[linha][campo].ToString().Trim();

            return resultado;
        }
    }
}
