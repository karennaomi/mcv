using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using Moq;
using System.Collections.Generic;

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
            var mock = new Mock<IRepositorioPontoDemanda>();
            mock.Setup(m => m.Obter(1, 100)).Returns(PontoDemanda);

            mock.Setup(m => m.Criar(1, PontoDemanda)).Returns<PontoDemanda>(x => { x.Id = 100; return x; });
            mock.Setup(m => m.Listar(1)).Returns(new List<PontoDemanda>
                {
                    new PontoDemanda { Id = 100 }, new PontoDemanda { Id = 101 }
                });
            return mock.Object;
        }
    }

    public class MockIntegranteRepo
    {
        public Integrante Integrante { get; set; }
        public Integrante Convidado { get; set; }

        public IRepositorioIntegrante GetMockedRepo()
        {
            var mock = new Mock<IRepositorioIntegrante>();
            mock.Setup(m => m.Obter(200)).Returns(Integrante);
            mock.Setup(m => m.Obter(201)).Returns(Convidado);
            mock.Setup(m => m.Criar(Integrante)).Returns<Integrante>(x => x);
            mock.Setup(m => m.VerificarSeEmailJaExiste("email@existente.com")).Throws<IntegranteExistenteException>();
            return mock.Object;
        }
    }

    public class MockCompraAtivaRepo
    {
        public CompraAtiva CompraAtiva { private get; set; }

        public IRepositorioCompraAtiva GetMockedRepo()
        {
            var mock = new Mock<IRepositorioCompraAtiva>();
            mock.Setup(m => m.AtivarCompra(1, 100)).Returns(CompraAtiva);
            mock.Setup(m => m.Obter(100)).Returns(CompraAtiva);
            mock.Setup(m => m.AtivarCompra(1, 101)).Returns(CompraAtiva);
            mock.Setup(m => m.Obter(101)).Returns((CompraAtiva)null);
            return mock.Object;
        }
    }

    public class MockTemplateMessageRepo
    {
        public string Tipo { private get; set; }

        public IRepositorioTemplateMensagem GetMockedRepo()
        {
            var mock = new Mock<IRepositorioTemplateMensagem>();
            mock.Setup(m => m.ObterPorTipoTemplate(TipoTemplateMensagem.AtivarCompra)).Returns(GetTemplateMensagem());
            return mock.Object;
        }

        private TemplateMensagem GetTemplateMensagem()
        {
            var fakes = new Fakes();
            switch (Tipo)
            {
                case "push":return fakes.TemplateMensagemPush();
                case "email": return fakes.TemplateMensagemEmail();
                default: return null;
            }
        }
    }

    public class MockListaRepo
    {
        public Lista Lista { private get; set; }

        public IRepositorioLista GetMockedRepo()
        {
            var mock = new Mock<IRepositorioLista>();
            mock.Setup(m => m.ObterListaPorPontoDemanda(100)).Returns(Lista);
            return mock.Object;
        }
    }
    
    public class MockPedidoRepo
    {
        public IEnumerable<PedidoItem> PedidoItens { private get; set; }

        public IRepositorioPedido GetMockedRepo()
        {
            var mock = new Mock<IRepositorioPedido>();
            mock.Setup(m => m.ListarItens(100)).Returns(PedidoItens);
            return mock.Object;
        }
    }

    public class MockCompraRepo
    {
        public IRepositorioCompra GetMockedRepo()
        {
            var mock = new Mock<IRepositorioCompra>();
            return mock.Object;
        }
    }
}

