using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace MCV.Api.Controllers
{
    public class Funcoes
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

        public static XmlDocument DadosMensagem(string cnpj, string uf, string versaoLayout)
        {
            StringWriter sWriter = new StringWriter();
            XmlDocument doc = new XmlDocument();

            using (XmlTextWriter xWriter = new XmlTextWriter(sWriter))
            {
                xWriter.WriteStartDocument();
                xWriter.Formatting = Formatting.None;
                xWriter.WriteStartElement("ConsCad", "http://www.portalfiscal.inf.br/nfe");
                xWriter.WriteAttributeString("versao", versaoLayout);
                xWriter.WriteStartElement("infCons");
                xWriter.WriteElementString("xServ", "CONS-CAD");
                xWriter.WriteElementString("UF", uf.ToUpper());
                //xWriter.WriteElementString("IE", "123456");
                xWriter.WriteElementString("CNPJ", cnpj);
                //xWriter.WriteElementString("CPF", "11111111111111");
                xWriter.WriteEndElement();
                xWriter.WriteEndElement();
                xWriter.WriteEndDocument();
            }

            doc.LoadXml(sWriter.ToString().Replace("utf-16", "utf-8"));

            return doc;
        }
    }
}
