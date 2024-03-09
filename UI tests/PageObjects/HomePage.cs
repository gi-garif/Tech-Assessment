using OpenQA.Selenium;

public class HomePage
{
    private readonly IWebDriver driver;

    // Locators
    private By companyElement = By.XPath("//a[text()='Company']");
    private By overviewLink = By.LinkText("Overview");

    // Constructor
    public HomePage(IWebDriver driver)
    {
        this.driver = driver;
    }

    // Methods
    public void ClickOverviewLink()
    {
        driver.FindElement(overviewLink).Click();
    }

    public void ClickCompanyElement()
    {
        driver.FindElement(companyElement).Click();
    }

}
