using System.Collections.Generic;
using System.Collections.ObjectModel;
using LM.Core.Domain;
using System;

namespace LM.Core.Tests
{
    internal class Fakes
    {
        internal static Usuario Usuario(int idade = 30, string sexo = "M")
        {
            return new Usuario
            {
                Nome = "John Doe",
                Email = "johndoe@mail.com",
                Cpf = "472.724.573-68",
                DataNascimento = DateTime.Now.AddYears(-idade),
                Senha = "123456",
                Sexo = sexo,
                Tipo = TipoUsuario.Administrador

            };
        }

        internal static PontoDemanda PontoDemanda()
        {
            return new PontoDemanda
            {
                Id = 666,
                Tipo = TipoPontoDemanda.Praia,
                GrupoDeIntegrantes = new GrupoDeIntegrantes
                {
                    Integrantes = new List<Integrante>{ new Integrante{ Id = 1234, Usuario = new Usuario { Id = 6}}}
                },
                Endereco = Endereco()
            };
        }

        internal static Endereco Endereco()
        {
            return new Endereco
            {
                Cidade = new Cidade {Nome = "São Paulo"},
                Alias = "Casa de sap",
                Descricao = "Rua dos bobos",
                Numero = 0,
                Bairro = "Vl Olimpia",
                Cep = "04458001",
                Latitude = -23.611926900000000M,
                Longitude = -46.661498300000000M
            };
        }


        internal static Integrante Integrante(int idade, string sexo)
        {
            return new Integrante
            {
                Usuario = Usuario(idade, sexo)
            };
        }

        internal static ICollection<Loja> Lojas()
        {
            return new Collection<Loja>
            {
                new Loja { Nome = "Loja Teste 1", Idlocalizador = "saqk8912q45214", OrigemLocalizador = "google", Info = new LojaInfo{ RazaoSocial = "TESTE 1 SA", Endereco = Endereco()}},
                new Loja { Nome = "Loja Teste 2", Idlocalizador = "saqka023hs8an5214", OrigemLocalizador = "google", Info = new LojaInfo{ RazaoSocial = "TESTE 2 SA", Endereco = Endereco()}},
                new Loja { Nome = "Loja Teste 3", Idlocalizador = "saqk02k32s85214", OrigemLocalizador = "google", Info = new LojaInfo{ RazaoSocial = "TESTE 3 SA", Endereco = Endereco()}}
            };
        }
    }
}
