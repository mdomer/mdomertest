using NUnit.Framework;

namespace Soneta.Examples.Rozwiazanie.Tests
{
    // uznalem ze najwaznisze przy testowaniu beda dwie metody 
    // z klasy GitHubService
    // GetAvarageStatistics() or GetDailyStatistics();
    // 
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            Assert.AreEqual(1,1);
        }
    }
}
