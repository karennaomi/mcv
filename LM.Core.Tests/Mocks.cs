using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using Moq;

namespace LM.Core.Tests
{
    public class MockUsuarioRepo
    {
        public Integrante Integrante { private get; set; }
        public Usuario Usuario { private get; set; }

        public IRepositorioUsuario GetMockedRepo()
        {
            var mock = new Mock<IRepositorioUsuario>();
            mock.Setup(m => m.UsuarioConvidado("integrante@convidado.com")).Returns(Integrante);
            mock.Setup(m => m.ObterPorLogin("usuario@login.com")).Returns(Usuario);
            mock.Setup(m => m.Obter(1)).Returns(Usuario);
            mock.Setup(m => m.VerificarSeEmailJaExiste("email@existente.com")).Throws<IntegranteExistenteException>();
            return mock.Object;
        }
    }

    public class MockPontoDemandaRepo
    {
        public PontoDemanda PontoDemanda { private get; set; }

        public IRepositorioPontoDemanda GetMockedRepo()
        {
            var repoMock = new Mock<IRepositorioPontoDemanda>();
            repoMock.Setup(r => r.Obter(1, 100)).Returns(PontoDemanda);

            repoMock.Setup(r => r.Criar(1, PontoDemanda)).Returns<PontoDemanda>(x => { x.Id = 100; return x; });
            repoMock.Setup(r => r.Listar(1)).Returns(new List<PontoDemanda>
                {
                    new PontoDemanda { Id = 100 }, new PontoDemanda { Id = 101 }
                });
            return repoMock.Object;
        }
    }
}
