#define Rozwiazanie

using System;
using Soneta.Business.UI;
using Soneta.Examples.Rozwiazanie.Extender;

#if Rozwiazanie

[assembly: FolderView("Soneta.Examples/Rozwiazania zadania rekrutacyjnego",
    Priority = 13,
    Description = "Zadania rekrutacyjne Michal Domeredzki",
    ObjectPage = "Statystyki.GitHub.pageform.xml",
    ObjectType = typeof(GitHubService), 
    ReadOnlySession = false,
    ConfigSession = false
)]

#endif
