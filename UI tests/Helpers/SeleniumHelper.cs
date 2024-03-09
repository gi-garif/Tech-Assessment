using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;

public class SeleniumHelper
{
    private readonly IWebDriver driver;

    public SeleniumHelper()
    {
        // Configuring Serilog 
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        driver = new ChromeDriver();
    }

    public IWebDriver Driver => driver;

    public void NavigateToUrl(string url)
    {
        Log.Information($"Navigating to URL: {url}");
        driver.Navigate().GoToUrl(url);
    }

    public void ClickElement(By locator)
    {
        try
        {
            Log.Information($"Clicking element with locator: {locator}");
            driver.FindElement(locator).Click();
        }
        catch (Exception ex)
        {
            Log.Error($"Error clicking element: {ex.Message}");
            throw;
        }
    }

    public void Dispose()
    {
        Log.Information("Disposing SeleniumHelper");
        driver.Quit();
    }
}
