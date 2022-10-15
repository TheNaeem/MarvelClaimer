global using Serilog;

using MarvelClaimer;
using MarvelClaimer.Marvel;
using OpenQA.Selenium;

Log.Logger =
    new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .CreateLogger();

AppDomain.CurrentDomain.ProcessExit += (object? _, EventArgs _) => Chrome.Stop();

new MarvelNuker().Run();