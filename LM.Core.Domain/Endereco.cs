using System;
using System.Text;

namespace LM.Core.Domain
{
    public class Endereco
    {
        public Endereco()
        {
            DataInclusao = DateTime.Now;
        }

        public long Id { get; set; }
        public string Logradouro { get; set; }
        public int? Numero { get; set; }
        public string Complemento { get; set; }
        public string Alias { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder(Logradouro);
            if(!string.IsNullOrWhiteSpace(Logradouro)) Adicionar(sb, Numero, ", ");
            Adicionar(sb, Bairro, " - ");
            if (!string.IsNullOrEmpty(Cidade)) Adicionar(sb, Cidade, " ");
            if (!string.IsNullOrEmpty(Uf)) Adicionar(sb, Uf, " - ");
            return sb.ToString();
        }

        private static void Adicionar(StringBuilder sb, object conteudo, string separador)
        {
            if(conteudo == null) return;
            if (string.IsNullOrWhiteSpace(conteudo.ToString())) return;
            if (sb.Length != 0) sb.Append(separador);
            sb.Append(conteudo);
        }
    }
}


