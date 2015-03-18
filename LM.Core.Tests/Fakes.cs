using LM.Core.Domain;
using System;

namespace LM.Core.Tests
{
    internal class Fakes
    {
        internal static Usuario Usuario()
        {
            return new Usuario
            {
                Nome = "John Doe",
                Email = "johndoe@mail.com",
                Cpf = "472.724.573-68",
                DataNascimento = DateTime.Now.AddYears(-30),
                Senha = "123456",
                Sexo = "M",
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
                }
            };
        }
    }
}
