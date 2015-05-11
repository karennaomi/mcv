﻿namespace LM.Core.Domain
{
    public class EnderecoViaCep
    {
        private readonly Endereco _endereco;
        public EnderecoViaCep()
        {
            _endereco = new Endereco{Cidade = new Cidade {Uf = new Uf()}};
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

        public string Bairro
        {
            get { return _endereco.Bairro; }
            set { _endereco.Bairro = value; }
        }

        public string Localidade
        {
            get { return _endereco.Cidade.Nome; }
            set { _endereco.Cidade.Nome = value; }
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