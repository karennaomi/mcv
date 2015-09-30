using LM.Core.RepositorioEF;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class FtsInterceptorTests
    {
        [Test]
        public void BuildSearchStringWithOneWord()
        {
            Assert.AreEqual("(-FTSPREFIX-\"leite*\")", FtsInterceptor.Fts("leite"));
        }

        [Test]
        public void BuildSearchStringWithTwoWords()
        {
            Assert.AreEqual("(-FTSPREFIX-\"leite*\" AND \"batavo*\")", FtsInterceptor.Fts("leite batavo"));
        }
    }
}
