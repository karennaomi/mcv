using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using LM.Core.Domain.Servicos;
using LM.Core.RepositorioEF;
using Moq;
using NUnit.Framework;
using System;
using System.Transactions;

namespace LM.Core.Tests
{
    [TestFixture]
    public class IntegranteAplicacaoTests
    {
        private Fakes _fakes;
        private MockUnitOfWorkRepo _mockRepo;
        private const string ImageHost = "http://img.teste.com";
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
            _mockRepo = new MockUnitOfWorkRepo();
        }
        
        [Test]
        public void CriarIntegrante()
        {
            var integrante = _fakes.Integrante("novo_integrante@mail.com");
            var app = ObterAppIntegrante(new UnitOfWorkEF());
            integrante = app.Criar(1, integrante);
            Assert.IsTrue(integrante.Id > 0);
        }

        [Test]
        public void CriarIntegrantePet()
        {
            using (new TransactionScope())
            {
                var integrante = _fakes.Integrante();
                integrante.Nome = "Bidu";
                integrante.Tipo = TipoIntegrante.Pet;
                integrante.DataNascimento = null;
                integrante.Sexo = null;
                integrante.Email = null;
                integrante.Telefone = null;
                integrante.AnimalId = 1;
                var app = ObterAppIntegrante(new UnitOfWorkEF());
                try
                {
                    integrante = app.Criar(1, integrante);

                }
                catch (DbEntityValidationException ex)
                {
                    
                    throw ex;
                }
                Assert.IsTrue(integrante.Id > 0);
            }
        }

        [Test]
        public void AtualizarIntegrante()
        {
            var integranteParaAtualizar = SetIntegranteInMockRepo(_fakes.Integrante());
            integranteParaAtualizar.Usuario = new Usuario { Id = 1 };
            _mockRepo.MockIntegranteRepo.Integrante = integranteParaAtualizar;
            var app = ObterAppIntegrante(_mockRepo.GetMockedRepo());

            var integrante = _fakes.Integrante();
            integrante.Id = 200;
            integrante.Nome = "Nome Atualizado";
            integrante.Email = "email@atualizado.com";
            var dataNascimento = DateTime.Now.AddYears(-17);
            integrante.DataNascimento = dataNascimento;
            integrante.Cpf = "12345678901";
            integrante.Telefone = "2165498754";
            integrante.Sexo = "f";

            var integranteAtualizado = app.Atualizar(100, 1, integrante);
            Assert.AreEqual("Nome Atualizado", integranteAtualizado.Nome);
            Assert.AreEqual("email@atualizado.com", integranteAtualizado.Email);
            Assert.AreEqual(dataNascimento, integranteAtualizado.DataNascimento);
            Assert.AreEqual("12345678901", integranteAtualizado.Cpf);
            Assert.AreEqual("2165498754", integranteAtualizado.Telefone);
            Assert.AreEqual("f", integranteAtualizado.Sexo);
        }

        [Test]
        public void AtualizarIntegrantePet()
        {
            var integranteParaAtualizar = SetIntegranteInMockRepo(_fakes.IntegrantePet());
            integranteParaAtualizar.AnimalId = 3;
            _mockRepo.MockIntegranteRepo.Integrante = integranteParaAtualizar;
            var app = ObterAppIntegrante(_mockRepo.GetMockedRepo());

            var integrante = _fakes.IntegrantePet();
            integrante.Id = 200;
            integrante.Nome = "Bidu";
            integrante.AnimalId = 1 ;

            var integranteAtualizado = app.Atualizar(100, 1, integrante);
            Assert.AreEqual("Bidu", integranteAtualizado.Nome);
            Assert.AreEqual(1, integranteAtualizado.AnimalId);
        }

        [Test]
        public void NãoPodeAtualizarIntegranteComEmailJaExistente()
        {
            var integranteParaAtualizar = SetIntegranteInMockRepo(_fakes.Integrante());
            _mockRepo.MockIntegranteRepo.Integrante = integranteParaAtualizar;
            var app = ObterAppIntegrante(_mockRepo.GetMockedRepo());
            var integrante = _fakes.Integrante();
            integrante.Usuario = new Usuario { Id = 1 };
            integrante.Id = 200;
            integrante.Email = "email@existente.com";
            Assert.Throws<IntegranteExistenteException>(() => app.Atualizar(100, 1, integrante));
        }

        [Test]
        public void NãoPodeAtualizarIntegranteQueNaoPertenceAoPontoDeDemanda()
        {
            var integrante = SetIntegranteInMockRepo(_fakes.Integrante());
            _mockRepo.MockIntegranteRepo.Integrante = integrante;
            var app = ObterAppIntegrante(_mockRepo.GetMockedRepo());
            Assert.Throws<IntegranteNaoPertenceAPontoDemandaException>(() => app.Atualizar(9999, 1, integrante));
        }

        [Test]
        public void NãoPodeAtualizarIntegranteQueEhUsuarioDoSistema()
        {
            var integranteParaAtualizar = _fakes.Integrante();
            integranteParaAtualizar = SetIntegranteInMockRepo(integranteParaAtualizar);
            integranteParaAtualizar.Usuario = new Usuario { Id = 2 };
            _mockRepo.MockIntegranteRepo.Integrante = integranteParaAtualizar;
            var app = ObterAppIntegrante(_mockRepo.GetMockedRepo());
            
            var integrante = _fakes.Integrante();
            integrante.Id = 200;
            integrante.Nome = "Nome Atualizado";
            var ex = Assert.Throws<ApplicationException>(() => app.Atualizar(100, 1, integrante));
            Assert.AreEqual("Não pode atualizar um usuário do sistema.", ex.Message);
        }

        [Test]
        public void PodeAtualizarIntegranteQueEhUsuarioDoSistemaSeForEleMesmo()
        {
            var integranteParaAtualizar = _fakes.Integrante();
            integranteParaAtualizar.Usuario = new Usuario { Id = 1 };
            integranteParaAtualizar = SetIntegranteInMockRepo(integranteParaAtualizar);
            _mockRepo.MockIntegranteRepo.Integrante = integranteParaAtualizar;

            var integrante = _fakes.Integrante();
            integrante.Id = 200;
            integrante.Nome = "Nome Atualizado";
            var app = ObterAppIntegrante(_mockRepo.GetMockedRepo());
            var integranteAtualizado = app.Atualizar(100, 1, integrante);
            Assert.AreEqual("Nome Atualizado", integranteAtualizado.Nome);
        }

        [Test]
        public void PodeAtualizarIntegranteQueNaoEhUsuarioDoSistema()
        {
            var integranteParaAtualizar = _fakes.Integrante();
            integranteParaAtualizar = SetIntegranteInMockRepo(integranteParaAtualizar);
            integranteParaAtualizar.Usuario = null;
            _mockRepo.MockIntegranteRepo.Integrante = integranteParaAtualizar;

            var integrante = _fakes.Integrante();
            integrante.Id = 200;
            integrante.Nome = "Nome Atualizado";
            var app = ObterAppIntegrante(_mockRepo.GetMockedRepo());
            var integranteAtualizado = app.Atualizar(100, 1, integrante);
            Assert.AreEqual("Nome Atualizado", integranteAtualizado.Nome);
        }

        [Test]
        public void NaoPodeDesativarUmIntegranteQueNaoPertenceAoPontoDeDemanda()
        {
            var integrante = SetIntegranteInMockRepo(_fakes.Integrante());
            _mockRepo.MockIntegranteRepo.Integrante = integrante;
            var app = ObterAppIntegrante(_mockRepo.GetMockedRepo());
            Assert.Throws<IntegranteNaoPertenceAPontoDemandaException>(() => app.Desativar(9999, 1, 200));
        }

        [Test]
        public void PodeDesativarUmIntegranteQuePertenceAoPonteDemanda()
        {
            var integrante = SetIntegranteInMockRepo(_fakes.Integrante());
            integrante.Usuario = new Usuario { Id = 2, StatusUsuarioPontoDemanda = new Collection<StatusUsuarioPontoDemanda>{ new StatusUsuarioPontoDemanda{ StatusCadastro = StatusCadastro.CadastroIniciado}}};
            _mockRepo.MockIntegranteRepo.Integrante = integrante;
            var app = ObterAppIntegrante(_mockRepo.GetMockedRepo());
            Assert.IsTrue(integrante.Ativo);
            app.Desativar(100, 1, 200);
            Assert.IsFalse(integrante.Ativo);
        }

        [Test]
        public void NaoPodeSeDesativar()
        {
            var integrante = SetIntegranteInMockRepo(_fakes.Integrante());
            integrante.Usuario = new Usuario { Id = 1 };
            _mockRepo.MockIntegranteRepo.Integrante = integrante;
            var app = ObterAppIntegrante(_mockRepo.GetMockedRepo());
            var ex = Assert.Throws<ApplicationException>(() => app.Desativar(100, 1, 200));
            Assert.AreEqual("Não pode desativar integrante.", ex.Message);
        }

        [Test]
        public void NaoPodeDesativarOCriadorDoPonto()
        {
            var integrante = SetIntegranteInMockRepo(_fakes.Integrante());
            integrante.Usuario = new Usuario { Id = 7 }; //Usuario criardor do ponto, ver em Fakes.PontoDemanda
            _mockRepo.MockIntegranteRepo.Integrante = integrante;
            var app = ObterAppIntegrante(_mockRepo.GetMockedRepo());
            var ex = Assert.Throws<ApplicationException>(() => app.Desativar(100, 1, 200));
            Assert.AreEqual("Não pode excluir o criador da casa.", ex.Message);
        }

        [Test]
        public void ConvidarIntegrante()
        {
            var integrante = SetIntegranteInMockRepo(_fakes.Integrante());
            _mockRepo.MockIntegranteRepo.Integrante = integrante;
            var convidado = SetIntegranteInMockRepo(_fakes.Integrante());
            convidado.Usuario = null;
            convidado.Id = 201;
            convidado.GruposDeIntegrantes = integrante.GruposDeIntegrantes;
            _mockRepo.MockIntegranteRepo.Convidado = convidado;
            var app = ObterAppIntegrante(_mockRepo.GetMockedRepo());
            app.Convidar(100, 1, 201, ImageHost);
            Assert.IsTrue(convidado.DataConvite.Value.Date == DateTime.Now.Date);
            Assert.IsTrue(convidado.EhUsuarioConvidado);
        }

        [Test]
        public void RemoverIntegranteDoGrupo()
        {
            var app1 = ObterAppIntegrante(new UnitOfWorkEF());
            var integrante = _fakes.Integrante("teste_remover_do_ponto@teste.com");
            integrante.Usuario = new Usuario { Login = "teste_remover_do_ponto@teste.com", Senha = "123456", StatusUsuarioPontoDemanda = new Collection<StatusUsuarioPontoDemanda>{new StatusUsuarioPontoDemanda{ StatusCadastro = StatusCadastro.EtapaDeInformacoesPessoaisCompleta }}};
            integrante = app1.Criar(1, integrante);
            var app2 = ObterAppIntegrante(new UnitOfWorkEF());
            app2.RemoverDoPonto(1, 1, integrante.Id);
        }

        private Integrante SetIntegranteInMockRepo(Integrante integrante)
        {
            integrante.Id = 200;
            integrante.Usuario = new Usuario { Id = 1 };
            var pontoDemanda = _fakes.PontoDemanda();
            pontoDemanda.Id = 100;
            integrante.GruposDeIntegrantes.Add(new GrupoDeIntegrantes { PontoDemanda = pontoDemanda, Integrante = integrante});
            pontoDemanda.GruposDeIntegrantes = integrante.GruposDeIntegrantes;
            return integrante;
        }

        private static IIntegranteAplicacao ObterAppIntegrante(IUnitOfWork repo)
        {
            var notificacaoApp = new NotificacaoAplicacao(new Mock<IServicoRest>().Object, new Mock<ITemplateMensagemAplicacao>().Object, new Mock<IFilaItemAplicacao>().Object);
            return new IntegranteAplicacao(repo, notificacaoApp);
        }
    }
}
