using OpenQA.Selenium.Chrome;
using System.Diagnostics;

namespace MarvelClaimer;

public static class Chrome
{
    private static ChromeDriver _driver;

    private static void CloseAllInstances()
    {
        foreach (var chrome in Process.GetProcessesByName("chrome"))
            chrome.Kill();
    }

    static Chrome()
    {
        CloseAllInstances();

        var chromeProfileDir = Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Google",
            "Chrome",
            "User Data");

        ChromeOptions options = new ChromeOptions();
        options.AddArgument($"--user-data-dir={chromeProfileDir}\\");
        options.AddArgument("disable-infobars");
        options.AddArgument("--start-maximized");

        _driver = new(Environment.CurrentDirectory, options);
    }

    public static void OpenUrl(string url)
    {
        _driver.Navigate().GoToUrl(url);
    }

    public static void Stop()
    {
        _driver.Close();
        _driver.Dispose();
        CloseAllInstances();
    }
}
