
namespace LM.Core.Domain
{
    public class Loja
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Idlocalizador { get; set; }
        public string OrigemLocalizador { get; set; }
        public string EnderecoResumido { get; set; }
        public Endereco EnderecoLoja { get; set; }

        public Loja()
        {
            EnderecoLoja = new Endereco();
        }
    }
}
