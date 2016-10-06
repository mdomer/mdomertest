using System;
using System.Runtime.Serialization;

namespace Soneta.Examples.Rozwiazanie.Extender
{
    // obiekty do prezentacji na formularzu

    public class DailyStatistic
    {
        public string Date { get; set; }
        public string Author { get; set; }
        public int CommitsCount { get; set; }
    }

    public class AverageStatistic
    {
        public string Author { get; set; }
        public double AverageCommitCount { get; set; }
    }

    // obiekty do deserializacji z api github-a

    [DataContract]
    public class Author
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }
        [DataMember(Name = "date")]
        public DateTime DateTime { get; set; }
    }

    [DataContract]
    public class ReturnValueItem
    {
        [DataMember(Name = "sha")]
        public string Sha { get; set; }
        [DataMember(Name = "commit")]
        public Commit Commit { get; set; }
    }

    [DataContract]
    public class Commit
    {
        [DataMember(Name = "author")]
        public Author Author { get; set; }
    }
}
