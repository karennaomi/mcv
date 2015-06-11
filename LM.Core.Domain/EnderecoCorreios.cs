namespace LM.Core.Domain
{
    public class EnderecoCorreios
    {
        private readonly Endereco _endereco;
        public EnderecoCorreios()
        {
            _endereco = new Endereco{Cidade = new Cidade {Uf = new Uf()}};
        }
        
        public string Bairro
        {
            get { return _endereco.Bairro; }
            set { _endereco.Bairro = value; }
        }

        public string Cidade
        {
            get { return _endereco.Cidade.Nome; }
            set { _endereco.Cidade.Nome = value; }
        }

        public string Cep
        {
            get { return _endereco.Cep; }
            set { _endereco.Cep = value; }
        }

        public string Endereco
        {
            get { return _endereco.Logradouro; }
            set { _endereco.Logradouro = value; }
        }

        public string Uf
        {
            get { return _endereco.Cidade.Uf.Sigla; }
            set { _endereco.Cidade.Uf.Sigla = value; }
        }

        public Endereco ObterEndereco()
        {
            return _endereco;
        }
    }
}