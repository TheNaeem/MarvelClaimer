using OpenQA.Selenium.Chrome;
using System.Diagnostics;

namespace MarvelClaimer;

public static class Chrome
{
    private static ChromeDriver _driver;

    static Chrome()
    {
        foreach (var chrome in Process.GetProcessesByName("chrome"))
            chrome.Kill();

        var chromeProfileDir = Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Google",
            "Chrome",
            "User Data");

        ChromeOptions options = new ChromeOptions();
        options.AddArgument($"--user-data-dir={chromeProfileDir}\\");
        options.AddArgument("disable-infobars");
        options.AddArgument("--start-maximized");

        _driver = new(@"C:\Users\zkana\Downloads\chromedriver_win32", options);
    }

    public static void OpenUrl(string url)
    {
        _driver.Navigate().GoToUrl(url);
    }
}
