﻿using System;
using System.Data.Entity.Validation;
using System.Runtime.InteropServices;
using System.Transactions;
using LM.Core.Application;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using LM.Core.RepositorioEF;
using Moq;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class IntegranteAplicacaoTests
    {
        private Fakes _fakes;
        [TestFixtureSetUp]
        public void Init()
        {
            _fakes = new Fakes();
        }

        [Test]
        public void CriarIntegrante()
        {
            
            using (new TransactionScope())
            {
                var integrante = _fakes.Integrante();
                integrante.GrupoDeIntegrantes.PontosDemanda = null;
                var app = new IntegranteAplicacao(new IntegranteEF(), new NotificacaoAplicacao(null, new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF())));
                integrante = app.Criar(integrante);
                Assert.IsTrue(integrante.Id > 0);
            }
        }

        [Test]
        public void CriarIntegrantePet()
        {
            using (new TransactionScope())
            {
                var integrante = _fakes.Integrante();
                integrante.Nome = "Bidu";
                integrante.Tipo = TipoIntegrante.Pet;
                var app = new IntegranteAplicacao(new IntegranteEF(), new NotificacaoAplicacao(null, new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF())));
                try
                {
                    integrante = app.Criar(integrante);
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
            using (new TransactionScope())
            {
                var integrante = _fakes.Integrante();
                integrante.Id = 9;
                var app = new IntegranteAplicacao(new IntegranteEF(), new NotificacaoAplicacao(null, new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF())));
                integrante = app.Atualizar(17, integrante);
                Assert.IsTrue(integrante.Id > 0);
            }
        }

        [Test]
        public void NãoPodeAtualizarIntegranteComEmailJaExistente()
        {
            using (new TransactionScope())
            {
                var integrante = _fakes.Integrante();
                integrante.Id = 9;
                integrante.Email = "johndoe@mail.com";
                var app = new IntegranteAplicacao(new IntegranteEF(), new NotificacaoAplicacao(null, new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF())));
                Assert.Throws<IntegranteExistenteException>(() => app.Atualizar(17, integrante));
            }
        }

        [Test]
        public void NãoPodeAtualizarIntegranteQueNaoPertenceAoPontoDeDemanda()
        {
            using (new TransactionScope())
            {
                var integrante = _fakes.Integrante();
                integrante.Id = 9;
                integrante.Email = "johndoe@mail.com";
                var app = new IntegranteAplicacao(new IntegranteEF(), new NotificacaoAplicacao(null, new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF())));
                Assert.Throws<IntegranteNaoPertenceAPontoDemandaException>(() => app.Atualizar(9876, integrante));
            }
        }

        [Test]
        public void NaoPodeApagarUmIntegranteQueNaoPertenceAoPonteDemandaEspecificado()
        {
            var integrante = _fakes.Integrante();
            var app = ObterAppIntegrante(ObterIntegranteRepo(integrante).Object);
            Assert.Throws<IntegranteNaoPertenceAPontoDemandaException>(() => app.Apagar(9999, 123));
        }

        [Test]
        public void PodeApagarUmIntegranteQuePertenceAoPonteDemandaEspecificado()
        {
            var integrante = _fakes.Integrante();
            var repoMock = ObterIntegranteRepo(integrante);
            var app = ObterAppIntegrante(repoMock.Object);
            app.Apagar(666, 1234);
            repoMock.Verify(r => r.Apagar(integrante), Times.Once);
        }

        [Test]
        public void ConvidarIntegrante()
        {
            using (new TransactionScope())
            {
                var app = ObterAppIntegrante(new IntegranteEF());
                app.Convidar(20, 49, 53);
                var convidado = app.Obter(20, 53);
                Assert.IsTrue(convidado.DataConvite.Value.Date == DateTime.Now.Date);
            }
        }

        private static IIntegranteAplicacao ObterAppIntegrante(IRepositorioIntegrante repo)
        {
            return new IntegranteAplicacao(repo, new NotificacaoAplicacao(null, new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF())));
        }

        private Mock<IRepositorioIntegrante> ObterIntegranteRepo(Integrante integrante)
        {
            integrante.GrupoDeIntegrantes.PontosDemanda.Add(_fakes.PontoDemanda());
            var repoMock = new Mock<IRepositorioIntegrante>();
            repoMock.Setup(r => r.Obter(It.IsAny<long>())).Returns(integrante);
            repoMock.Setup(r => r.Criar(It.IsAny<Integrante>())).Returns<Integrante>(x => x);
            return repoMock;
        }
    }
}
