using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;

namespace MarvelClaimer;

public static class Chrome
{
    private static ChromeDriver _driver;
    private static CacheFile _cache;

    private static void CloseAllInstances()
    {
        foreach (var chrome in Process.GetProcessesByName("chrome"))
            chrome.Kill();
    }

    static Chrome()
    {
        if (!File.Exists("cache.json"))
        {
            File.WriteAllText("cache.json", JsonConvert.SerializeObject(new CacheFile()));
        }

        _cache = JsonConvert.DeserializeObject<CacheFile>(File.ReadAllText("cache.json"));

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

    public static void OpenUrl(string url, bool checkForVideo = false)
    {
        _driver.Navigate().GoToUrl(url);

        string readyState = string.Empty;
        while (readyState != "complete")
        {
            readyState = (string)_driver.ExecuteScript("return document.readyState");
            Thread.Sleep(10);
        }

        if (checkForVideo && IsElementPresent(By.ClassName("vjs-big-play-button")))
        {
            try
            {
                Log.Information("Found a video clicking it!");

                _driver.FindElement(By.ClassName("vjs-big-play-button")).Click();

                Thread.Sleep(1500);
            }
            catch { }
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

    private static IWebElement? WaitForElement(By by)
    {
        var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));

        return wait.Until(_ =>
        {
            try
            {
                var ret = _driver.FindElement(by);

                if (ret.Displayed)
                    return ret;
            }
            catch (NoSuchElementException e)
            {
                return null;
            }
            catch (ElementNotVisibleException e)
            {
                return null;
            }

            return null;
        });
    }

    public static IWebElement MoveToElement(IWebElement element)
    {
        Actions actions = new Actions(_driver);
        actions.MoveToElement(element);
        actions.Perform();

        return element;
    }

    public static string CreateMarvelAccount()
    {
        var email = _cache.emailBase + '+' + _cache.lastNum + "@gmail.com";
        var pw = _cache.password;

        var dob = new DateTime(2000, DateTime.Today.Month, DateTime.Today.Day);

        OpenUrl("https://www.marvel.com/insider");

        WaitForElement(By.ClassName("user-menu-tab"))?.Click();

        _driver.SwitchTo().Frame("oneid-iframe");
        WaitForElement(By.Id("BtnCreateAccount"))?.Click();

        WaitForElement(By.Id("InputFirstName"))?.SendKeys("Parse");
        WaitForElement(By.Id("InputLastName"))?.SendKeys("Jason");
        WaitForElement(By.Id("InputEmail"))?.SendKeys(email);
        WaitForElement(By.Id("password-new"))?.SendKeys(pw);
        WaitForElement(By.Id("InputDOB"))?.SendKeys(dob.ToString("MM/dd/yyyy"));
        Thread.Sleep(1000);

        WaitForElement(By.Id("BtnSubmit"))?.Click();

        Thread.Sleep(10000);

        //_driver.Navigate().Refresh();

        _cache.lastNum++;

        File.WriteAllText("cache.json", JsonConvert.SerializeObject(_cache));

        return email;
    }

    public static void SignOut()
    {
        OpenUrl("https://www.marvel.com/");

        WaitForElement(By.Id("mvl-user-menu__desktop"))?.Click();
        WaitForElement(By.Id("logout"))?.Click();
    }
}
