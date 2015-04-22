using LM.Core.Application;
using LM.Core.RepositorioEF;
using NUnit.Framework;
using System.Transactions;

namespace LM.Core.Tests
{
    [TestFixture]
    public class RecuperarSenhaAplicacaoTests
    {
        [Test]
        public void CriaUmaRecuperacaoDeSenha()
        {
            using (new TransactionScope())
            {
                var app = new RecuperarSenhaAplicacao(new RecuperarSenhaEF(),
                new UsuarioAplicacao(new UsuarioEF(), new PersonaAplicacao(new PersonaEF())), new TemplateMensagemAplicacao(new TemplateMensagemEF()), new FilaItemAplicacao(new FilaItemEF()));
                var recuperarSenha = app.RecuperarSenha("thanos@marvel.com");
                Assert.IsTrue(recuperarSenha.Id > 0);
                Assert.IsNotNull(recuperarSenha.Usuario);
            }
        }
    }
}
