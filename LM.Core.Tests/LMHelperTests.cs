using LM.Core.Domain;
using NUnit.Framework;
using System;

namespace LM.Core.Tests
{
    [TestFixture]
    public class LMHelperTests
    {
        [Test]
        public void ObterIdade18Anos()
        {
            Assert.AreEqual(18, LMHelper.ObterIdade(DateTime.Now.AddYears(-18)));
        }
    }
}
