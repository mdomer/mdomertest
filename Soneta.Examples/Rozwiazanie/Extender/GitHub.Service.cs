using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Soneta.Business;
using Soneta.Business.UI;

namespace Soneta.Examples.Rozwiazanie.Extender
{
    public class GitHubService
    {
        // linki do githuba i api
        //11
        //22
        // 33
        //44
        //55
        //66
       
        private const string RepoName = "angular/angular";
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
                return _allCommits.GroupBy(x => x.DateTime.Date)
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
                return _allCommits.GroupBy(x => x.Email)
                .Select(y => new AverageStatistic
                {
                    Author = y.Key,
                    AverageCommitCount = (double)(y.GroupBy(c => c.DateTime.Date).Select(v => v.Count()).Sum()) /
                           (double)(y.GroupBy(c => c.DateTime.Date).Select(v => v.Count()).Count())
                }).ToList();
            }
        }

        // sciaga wszystkie commity do repozytorium
        public void GetAllCommitsForUri(string uri)
        {
            var returnJson = DownloadString(uri);
            var parsedJson = ConvertToListOfCommits(returnJson);
            _allCommits = parsedJson.Select(x => x.Commit.Author).ToList();
        }

        public string DownloadString(string uri)
        {
            try
            {
                var client = new WebClient();
                string returnValue;
                // wazne zeby dodać naglowek USER-AGENT
                client.Headers.Add("user-agent", "michal.d85@gmail.com");

                using (var data = client.OpenRead(uri))
                {
                    if (data == null)
                    {
                        throw new Exception("Problem z polaczeniem");
                    }
                    var reader = new StreamReader(data);
                    returnValue = reader.ReadToEnd();
                    data.Close();
                    reader.Close();
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
            return null;
        }

        public static List<ReturnValueItem> ConvertToListOfCommits(string jsonData)
        {
            return JsonConvert.DeserializeObject<List<ReturnValueItem>>(jsonData);
        }
    }
}
