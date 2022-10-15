using Newtonsoft.Json;
using OpenQA.Selenium;
using Spectre.Console;

namespace MarvelClaimer.Marvel;

public class MarvelNuker
{
    private const string CACHE_FILE = "cache.json";
    private const string EMAILS_FILE = "claimed.txt";

    private CacheFile _cache;

    public MarvelNuker()
    {
        if (!File.Exists(CACHE_FILE))
        {
            Log.Information("Cache file does not exit. Created {Cache}", CACHE_FILE);

            _cache = new();
            _cache.emailBase = AnsiConsole.Ask<string>("Enter your base email (without @gmail.com)");
            _cache.password = AnsiConsole.Prompt(new TextPrompt<string>("Enter the password to create the accounts with").Secret());

            File.WriteAllText(CACHE_FILE, JsonConvert.SerializeObject(_cache));
            return;
        }

        var deserialized = JsonConvert.DeserializeObject<CacheFile>(File.ReadAllText("cache.json"));

        if (deserialized is null)
            throw new NullReferenceException("Could not deserialize cache JSON file, it was null.");

        _cache = deserialized;
    }

    private MarvelInsiderAccount CreateAccount()
    {
        var email = _cache.emailBase + '+' + _cache.lastNum + "@gmail.com";
        var pw = _cache.password;

        var dob = new DateTime(2000, DateTime.Today.Month, DateTime.Today.Day);

        Chrome.OpenUrl("https://www.marvel.com/insider");

        Chrome.WaitForElement(By.ClassName("user-menu-tab"))?.Click();

        Chrome.SwitchToFrame("oneid-iframe");
        Chrome.WaitForElement(By.Id("BtnCreateAccount"))?.Click();

        Chrome.WaitForElement(By.Id("InputFirstName"))?.SendKeys("Parse");
        Chrome.WaitForElement(By.Id("InputLastName"))?.SendKeys("Jason");
        Chrome.WaitForElement(By.Id("InputEmail"))?.SendKeys(email);
        Chrome.WaitForElement(By.Id("password-new"))?.SendKeys(pw);
        Chrome.WaitForElement(By.Id("InputDOB"))?.SendKeys(dob.ToString("MM/dd/yyyy"));
        Thread.Sleep(1000);

        Chrome.WaitForElement(By.Id("BtnSubmit"))?.Click();

        Thread.Sleep(5000);

        _cache.lastNum++;

        File.WriteAllText(CACHE_FILE, JsonConvert.SerializeObject(_cache));

        return new(email, pw);
    }

    void ClaimAccount()
    {
        if (!File.Exists(EMAILS_FILE))
            File.WriteAllText(EMAILS_FILE, string.Empty);

        using var account = CreateAccount();

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

        account.DoActivities(5638, Properties.Resources.GamesActivitiesBody);
        account.DoActivities(1374, Properties.Resources.ComicsActivitiesBody);
        account.DoActivities(1438, Properties.Resources.LatestActivitiesBody);

        account.FillQuestionare(Properties.Resources.ProfileQnaBody);
        account.FillQuestionare(Properties.Resources.NftQnaBody);
        account.FillQuestionare(Properties.Resources.SuperheroQnaBody);
        account.FillQuestionare(Properties.Resources.VillainQnaBody);
        account.FillQuestionare(Properties.Resources.MonsterQnaBody);
        account.FillQuestionare(Properties.Resources.JudgementQnaBody);

        Parallel.ForEach(IdToCode, (kvp, _) =>
        {
            account.RedeemCode(kvp.Key, kvp.Value);
        });

        account.VisitTwitter();
        account.VisitFacebook();
        account.VisitSnapchat();

        account.DoReferrals();

        Log.Information("Done with {email}!", account.Email);
        File.AppendAllText(EMAILS_FILE, account.Email + '\n');

        account.SignOut();
    }

    public void Run()
    {
        while (true)
        {
            ClaimAccount();
        }
    }
}
