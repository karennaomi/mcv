using System.Collections.ObjectModel;
using LM.Core.Domain;
using System;

namespace LM.Core.Tests
{
    internal class Fakes
    {
        internal Usuario Usuario()
        {
            return new Usuario
            {
                Login = "joe123@doe.com",
                Senha = "123456",
                Integrante = Integrante()
            };
        }

        internal Integrante Integrante()
        {
            var integrante = new Integrante
            {
                DataNascimento = DateTime.Now.AddYears(-18),
                EhUsuarioSistema = false,
                Nome = "Joe Doe",
                Sexo = "M",
                Email = "joe123@doe.com",
                GrupoDeIntegrantes = new GrupoDeIntegrantes {Id = 1, PontosDemanda = new Collection<PontoDemanda>(), Integrantes = new Collection<Integrante>()},
                Telefone = "989998999"
            };
            integrante.GrupoDeIntegrantes.Integrantes.Add(integrante);
            return integrante;
        }

        internal PontoDemanda PontoDemanda()
        {
            var usuario = Usuario();
            usuario.Id = 7;
            return new PontoDemanda
            {
                Id = 666,
                Tipo = TipoPontoDemanda.Praia,
                GrupoDeIntegrantes = new GrupoDeIntegrantes {Id = 1, Integrantes = new Collection<Integrante>()},
                Endereco = Endereco(),
                UsuarioCriador = usuario
            };
        }

        internal Endereco Endereco()
        {
            return new Endereco
            {
                Cidade = new Cidade {Nome = "São Paulo"},
                Alias = "Casa de sap",
                Logradouro = "Rua dos bobos",
                Numero = 0,
                Bairro = "Vl Olimpia",
                Cep = "04458001",
                Latitude = -23.611926900000000M,
                Longitude = -46.661498300000000M
            };
        }

        private int _lojaId;
        internal Loja Loja()
        {
            _lojaId = ++_lojaId;
            return new Loja
            {
                Nome = "Loja Teste " + _lojaId,
                Idlocalizador = "saqk8912q45214_" + _lojaId,
                OrigemLocalizador = "google",
                Info = new LojaInfo
                {
                    RazaoSocial = "TESTE SA" + _lojaId,
                    Endereco = Endereco()
                }
            };
        }
    }
}
