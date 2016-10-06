#define Rozwiazanie

using System;
using Soneta.Business.Licence;
using Soneta.Business.UI;
using Soneta.Examples.Rozwiazanie.Extender;

#if Rozwiazanie

[assembly: FolderView("Soneta.Examples",
    Priority = 10,
    Description = "Przykłady implementacji enova365",
    BrickColor = FolderViewAttribute.BlueBrick,
    Icon = "Soneta.Examples.Utils.examples.ico;Soneta.Examples",
    Contexts = new object[] { LicencjeModułu.All }
)]

[assembly: FolderView("Soneta.Examples/Rozwiazania zadania rekrutacyjnego",
    Priority = 13,
    Description = "Zadania rekrutacyjne Michal Domeredzki",
    ObjectPage = "Statystyki.GitHub.pageform.xml",
    ObjectType = typeof(GitHubService), 
    ReadOnlySession = false,
    ConfigSession = false
)]

#endif
