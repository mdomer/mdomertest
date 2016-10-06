using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Soneta.Examples.Rozwiazanie.Extender;

namespace Soneta.Examples.Rozwiazanie.Tests
{
    // uznalem ze najwaznisze przy testowaniu beda dwie metody 
    // z klasy GitHubService
    // GetAvarageStatistics() or GetDailyStatistics();
    // ktora odpowieadaja za wyswietlanie prawidlowych danych
    // skupie sie tylko nad GetDaily Statistics()

    [TestFixture]
    public class Tests
    {
        private GitHubService _gitHubService;
        [SetUp]
        protected void SetUp()
        {
            _gitHubService = new GitHubService();
        }

        [Test]
        public void Test1()
        {
            // testowe dane
            var commits = new List<Author>();
            commits.Add(new Author {DateTime = new DateTime(2016, 10, 10), Email = "test1", Name = "test1"});

            var result = _gitHubService.GetDailyStatistics(commits);
            Assert.AreEqual(result.Count(), 1);
        }

        [Test]
        public void Test2()
        {
            // testowe dane
            var commits = new List<Author>();
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test1", Name = "test1" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test1", Name = "test1" });

            var result = _gitHubService.GetDailyStatistics(commits);
            Assert.AreEqual(result.Count(), 1);
        }

        [Test]
        public void Test3()
        {
            // testowe dane
            var commits = new List<Author>();
         
            var result = _gitHubService.GetDailyStatistics(commits);
            Assert.AreEqual(result.Count(), 0);
        }
    }
}
