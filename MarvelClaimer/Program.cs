global using Serilog;
using MarvelClaimer;
using System;

try
{
    Log.Logger =
        new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Console()
        .CreateLogger();

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

    const string cookieFile = "cookie.txt";

    if (!File.Exists(cookieFile))
    {
        File.WriteAllText(string.Empty, cookieFile);
        Log.Error("Please put your cookies in the cookie.txt file.");
        Console.ReadKey();
        return;
    }

    var client = new MarvelInsiderClient(File.ReadAllText(cookieFile));

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

    Chrome.Stop();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}