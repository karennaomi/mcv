using LM.Core.Application;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class NotificacaoAplicacaoTests
    {
        [Test]
        [Ignore]
        public void EnviaNotificacao()
        {
            var appNotificacao = new NotificacaoAplicacao("http://localhost:45678");
            appNotificacao.EnviarNotificacao("apple", "724e138631b28402e9621f927010cccbf02ff424869d42217a40f075a8bfe01d", "Teste de push", "compras");
        }
    }
}
