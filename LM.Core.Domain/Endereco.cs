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

        public virtual Cidade Cidade { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder(Logradouro);
            if(!string.IsNullOrWhiteSpace(Logradouro)) Adicionar(sb, Numero, ", ");
            Adicionar(sb, Bairro, " - ");
            if (Cidade == null) return sb.ToString();
            Adicionar(sb, Cidade.Nome, " ");
            if (Cidade.Uf != null) Adicionar(sb, Cidade.Uf.Sigla, " - ");
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


