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
                GrupoDeIntegrantes = new GrupoDeIntegrantes
                {
                    Integrantes = new []{ new Integrante{ Id = 1234 }}
                },
                Endereco = new Endereco { Cidade = new Cidade { Nome = "São Paulo" }, Alias = "Casa de sap", Descricao = "Rua dos bobos", Numero = 0, Bairro = "Vl Olimpia", Cep = "04458001", Latitude = -23.611926900000000M, Longitude = -46.661498300000000M }
            };
        }

        internal static Integrante Integrante(int idade, string sexo)
        {
            return new Integrante
            {
                Usuario = Usuario(idade, sexo)
            };
        }
    }
}
