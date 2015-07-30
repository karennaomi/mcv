namespace LM.Core.Domain
{
    public class EnderecoPostmon
    {
        private readonly Endereco _endereco;
        public EnderecoPostmon()
        {
            _endereco = new Endereco();
        }
        
        public string Bairro
        {
            get { return _endereco.Bairro; }
            set { _endereco.Bairro = value; }
        }

        public string Cidade
        {
            get { return _endereco.Cidade; }
            set { _endereco.Cidade = value; }
        }

        public string Cep
        {
            get { return _endereco.Cep; }
            set { _endereco.Cep = value; }
        }

        public string Logradouro
        {
            get { return _endereco.Logradouro; }
            set { _endereco.Logradouro = value; }
        }

        public string Estado
        {
            get { return _endereco.Uf; }
            set { _endereco.Uf = value; }
        }

        public Endereco ObterEndereco()
        {
            return _endereco;
        }
    }
}