using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Soneta.Examples.Rozwiazanie.Extender
{
    public class GitHubService
    {
        // linki do githuba i api
       
        private const string RepoName = "auth0-blog/angular2-authentication-sample";
        //private const string RepoName = "mdomer/mdomertest";
        private const string GitHubApiUrl = "https://api.github.com/repos/{0}/commits";
        private const string GitHubAppUrl = "https://github.com/{0}";
        private List<Author> _allCommits;


        // informacyjny string
        public string RepoUri
        {
            get { return string.Format(GitHubAppUrl, RepoName); }
        }
        
        // ilość commit-ów, które dana osoba wprowadziła danego dnia
        public IEnumerable<DailyStatistic> DailyStatstics
        {
            get
            {
                if (_allCommits == null)
                {
                    GetAllCommitsForUri(string.Format(GitHubApiUrl, RepoName));
                }
                return GetDailyStatistics(_allCommits);
            }
        }

        public IEnumerable<DailyStatistic> GetDailyStatistics(List<Author> commits)
        {
            return commits.GroupBy(x => x.DateTime.Date)
                .Select(y => new
                {
                    Date = y.Key,
                    Value = y.GroupBy(v => v.Email)
                        .Select(s => new DailyStatistic
                        {
                            Date = y.Key.ToShortDateString(),
                            Author = s.Key,
                            CommitsCount = s.Select(a => a.Email).Count()
                        }).ToList()
                }).SelectMany(x => x.Value).OrderByDescending(x => x.Date)
                .ToList(); // agregacja po dniu
        }

        // średnia ilość commit-ów dodawanych przez daną osobę dziennie
        public IEnumerable<AverageStatistic> AvarageStatistics
        {
            get
            {
                if (_allCommits == null)
                {
                    GetAllCommitsForUri(string.Format(GitHubApiUrl, RepoName));
                }
                return GetAvarageStatistics(_allCommits);
            }
        }

        public IEnumerable<AverageStatistic> GetAvarageStatistics(List<Author> commits)
        {
            return commits.GroupBy(x => x.Email)
                .Select(y => new AverageStatistic
                {
                    Author = y.Key,
                    AverageCommitCount = (double)(y.GroupBy(c => c.DateTime.Date).Select(v => v.Count()).Sum()) /
                           (double)(y.GroupBy(c => c.DateTime.Date).Select(v => v.Count()).Count())
                }).ToList();
        }

        // sciaga wszystkie commity do repozytorium
        private void GetAllCommitsForUri(string uri)
        {
            var allCommits = new List<ReturnValueItem>();

            var respond = DownloadString(uri);
            allCommits.AddRange(ConvertToListOfCommits(respond.Value));

            while (!string.IsNullOrWhiteSpace(respond.NextUrl))
            {
                respond = DownloadString(respond.NextUrl);
                allCommits.AddRange(ConvertToListOfCommits(respond.Value));
            }

            _allCommits = allCommits.Select(x => x.Commit.Author).ToList();
        }

        // zapytanie GET 
        private WebRespond DownloadString(string uri)
        {
            try
            {
                var respond = new WebRespond();
                var client = new WebClient();
                client.Headers.Add("user-agent", "michal.d85@gmail.com");

                using (var data = client.OpenRead(uri))
                {
                    respond.NextUrl = GetNextLink(client.ResponseHeaders["Link"]);
                    if (data == null)
                    {
                        throw new Exception("Problem z polaczeniem");
                    }
                    var reader = new StreamReader(data);
                    respond.Value = reader.ReadToEnd();
                    data.Close();
                    reader.Close();
                }
                return respond;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
            return null;
        }

        private List<ReturnValueItem> ConvertToListOfCommits(string jsonData)
        {
            return JsonConvert.DeserializeObject<List<ReturnValueItem>>(jsonData);
        }

        // obsluga stronicowania z github-a
        // przyklad : 
        // <https://api.github.com/repositories/34740196/commits?page=2>; rel="next",
        // <https://api.github.com/repositories/34740196/commits?page=4>; rel="last"
        private string GetNextLink(string linkHeader)
        {
            var arr = linkHeader.Split(',');
            // jezeli dostaniemy nie poprawna odpowiedz 
            if (arr.Length == 0)
            {
                throw new Exception("Niepoprawna odpowiedz z serwera");
            }
            if (arr.FirstOrDefault(x => x.Contains("rel=\"next\"")) == null)
            {
                return null;
            }
            var regex = new Regex("<(.*?)>");
            var returnVal = regex.Match(arr.First()).Value;
            returnVal = returnVal.Replace('<', ' ');
            returnVal = returnVal.Replace('>', ' ');
            return returnVal.Trim();
        }
    }
}
