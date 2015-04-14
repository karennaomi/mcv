using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LM.Core.Domain
{
    public class Endereco
    {
        public Endereco()
        {
            DataInclusao = DataAlteracao = DateTime.Now;
        }

        public long Id { get; set; }
        [DisplayName("Endereço")]
        public string Descricao { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string Alias { get; set; }
        [Required(ErrorMessage = "O campo [Cep] é de preenchimento obrigatório!", AllowEmptyStrings = false)]
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        [Required(ErrorMessage = "O campo [Cidade] é de preenchimento obrigatório!", AllowEmptyStrings = false)]
        public virtual Cidade Cidade { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder(Descricao);
            sb.Append(" ");
            sb.Append(Numero);

            Adicionar(sb, Bairro);
            Adicionar(sb, Cep);
            if (Cidade != null)
            {
                Adicionar(sb, Cidade.Nome);
                if (Cidade.Uf != null)
                {
                    Adicionar(sb, Cidade.Uf.Sigla);
                }
            }
            return sb.ToString();
        }

        private void Adicionar(StringBuilder sb, string conteudo)
        {
            if (string.IsNullOrWhiteSpace(conteudo)) return;
            sb.Append(" - ");
            sb.Append(conteudo);
        }
    }
}


