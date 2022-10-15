global using Serilog;

using MarvelClaimer;
using System.Text;

Log.Logger =
    new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .CreateLogger();

if (!File.Exists("emails.txt"))
    File.WriteAllText("emails.txt", string.Empty);

AppDomain.CurrentDomain.ProcessExit += (object? _, EventArgs _) => Chrome.Stop();

while (true)
{
    var email = Chrome.CreateMarvelAccount();

    Dictionary<int, string> IdToCode = new()
{
    { 10913, "LM18" },
    { 10912, "MW37" },
    { 10917, "Dora Milaje" },
    { 10918, "new orleans" },
    { 10916, "blood transfusion" },
    { 10914, "JJ15" },
    { 10910, "EJ98Q" },
    { 10920, "Miracleman" },
    { 10919, "Kelly Thompson" },
    { 10930, "TOTEM" },
    { 10915, "EL61" }
};

    var client = new MarvelInsiderClient(Chrome.GetCookie("https://www.marvel.com/insider/home"));

    client.DoActivities(5638, MarvelClaimer.Properties.Resources.GamesActivitiesBody);
    client.DoActivities(1374, MarvelClaimer.Properties.Resources.ComicsActivitiesBody);
    client.DoActivities(1438, MarvelClaimer.Properties.Resources.LatestActivitiesBody);

    client.FillQuestionare(MarvelClaimer.Properties.Resources.ProfileQnaBody);
    client.FillQuestionare(MarvelClaimer.Properties.Resources.NftQnaBody);
    client.FillQuestionare(MarvelClaimer.Properties.Resources.SuperheroQnaBody);
    client.FillQuestionare(MarvelClaimer.Properties.Resources.VillainQnaBody);
    client.FillQuestionare(MarvelClaimer.Properties.Resources.MonsterQnaBody);
    client.FillQuestionare(MarvelClaimer.Properties.Resources.JudgementQnaBody);

    Parallel.ForEach(IdToCode, (kvp, _) =>
    {
        client.RedeemCode(kvp.Key, kvp.Value);
    });

    client.VisitTwitter();
    client.VisitFacebook();

    client.DoReferrals();

    Log.Information("Done with {email}!", email);

    File.AppendAllText("emails.txt", email + '\n');

    Chrome.SignOut();
}