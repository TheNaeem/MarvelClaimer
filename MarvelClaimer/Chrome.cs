using OpenQA.Selenium;
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

    private static bool IsElementPresent(By by)
    {
        try
        {
            _driver.FindElement(by);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public static async Task OpenUrl(string url)
    {
        _driver.Navigate().GoToUrl(url);

        string readyState = string.Empty;
        while (readyState != "complete")
        {
            readyState = (string)_driver.ExecuteScript("return document.readyState");
            await Task.Delay(10);
        }

        if (IsElementPresent(By.Id("vjs_video_3")))
        {
            Log.Information("Found a video clicking it!");

            _driver.FindElement(By.ClassName("vjs-big-play-button")).Click();

            await Task.Delay(3000);
        }
    }

    public static string GetCookie(string url)
    {
        _driver.Navigate().GoToUrl(url);

        var cookie = (string)_driver.ExecuteScript("return document.cookie");

        return cookie;
    }

    public static void Stop()
    {
        _driver.Close();
        _driver.Dispose();
        CloseAllInstances();
    }
}
