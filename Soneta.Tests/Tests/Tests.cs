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

        // prawidlowa agregacja

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

        [Test]
        public void Test4()
        {
            // testowe dane
            var commits = new List<Author>();
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test1", Name = "test1" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 09), Email = "test1", Name = "test1" });

            var result = _gitHubService.GetDailyStatistics(commits);
            Assert.AreEqual(result.Count(), 2);
        }

        [Test]
        public void Test5()
        {
            // testowe dane
            var commits = new List<Author>();
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test1", Name = "test1" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 09), Email = "test1", Name = "test1" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 09), Email = "test2", Name = "test2" });

            var result = _gitHubService.GetDailyStatistics(commits);
            Assert.AreEqual(result.Count(), 3);
        }

        [Test]
        public void Test6()
        {
            // testowe dane
            var commits = new List<Author>();
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test1", Name = "test1" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 09), Email = "test1", Name = "test1" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 09), Email = "test2", Name = "test2" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test2", Name = "test2" });

            var result = _gitHubService.GetDailyStatistics(commits);
            Assert.AreEqual(result.Count(), 4);
        }

        [Test]
        public void Test7()
        {
            // testowe dane
            var commits = new List<Author>();
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test1", Name = "test1" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 09), Email = "test1", Name = "test1" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 09), Email = "test2", Name = "test2" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test2", Name = "test2" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test2", Name = "test2" });

            var result = _gitHubService.GetDailyStatistics(commits);
            Assert.AreEqual(result.Count(), 4);
        }

        // liczba commitow

        [Test]
        public void Test8()
        {
            // testowe dane
            var commits = new List<Author>();
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test1@op.pl", Name = "test1" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 09), Email = "test1@op.pl", Name = "test1" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 09), Email = "test2@op.pl", Name = "test2" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test2@op.pl", Name = "test2" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test2@op.pl", Name = "test2" });

            var result = _gitHubService.GetDailyStatistics(commits).ToList();
            Assert.AreEqual(result[0].Author, "test1@op.pl");
            Assert.AreEqual(result[0].CommitsCount, 1);

            Assert.AreEqual(result[1].Author, "test2@op.pl");
            Assert.AreEqual(result[1].CommitsCount, 2);

            Assert.AreEqual(result[2].Author, "test1@op.pl");
            Assert.AreEqual(result[2].CommitsCount, 1);

            Assert.AreEqual(result[3].Author, "test2@op.pl");
            Assert.AreEqual(result[3].CommitsCount, 1);
        }

        [Test]
        public void Test9()
        {
            // testowe dane
            var commits = new List<Author>();
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test2@op.pl", Name = "test2" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test2@op.pl", Name = "test2" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test2@op.pl", Name = "test2" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test2@op.pl", Name = "test2" });


            var result = _gitHubService.GetDailyStatistics(commits).ToList();
            Assert.AreEqual(result[0].Author, "test2@op.pl");
            Assert.AreEqual(result[0].CommitsCount, 4);
        }

        // sortowanie po datcie
        [Test]
        public void Test10()
        {
            // testowe dane
            var commits = new List<Author>();
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 09), Email = "test2@op.pl", Name = "test2" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 08), Email = "test2@op.pl", Name = "test2" });
            commits.Add(new Author { DateTime = new DateTime(2016, 10, 10), Email = "test2@op.pl", Name = "test2" });
        

            var result = _gitHubService.GetDailyStatistics(commits).ToList();
            Assert.AreEqual(result.Count(), 3);
            Assert.AreEqual(result[0].Date, "10.10.2016");
            Assert.AreEqual(result[1].Date, "09.10.2016");
            Assert.AreEqual(result[2].Date, "08.10.2016");

        }
    }
}
