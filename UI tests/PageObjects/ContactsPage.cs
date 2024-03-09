using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

public class ContactsPage
{
    private readonly IWebDriver driver;

    public ContactsPage(IWebDriver driver)
    {
        this.driver = driver;
    }

    public bool IsContactPageDisplayed()
    {
        try
        {
            // Wait for the document to be in the "complete" state
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

            // If no exceptions are thrown, consider the 'Contact' page as loaded
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            // Handle the case when the document is not in the "complete" state within the specified time
            Console.WriteLine("Error: 'Contact' page not loaded completely within the timeout period");
            return false;
        }
    }
}
