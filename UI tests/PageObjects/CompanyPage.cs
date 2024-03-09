using OpenQA.Selenium;

public class CompanyPage
{
    private readonly IWebDriver driver;

    // Locators
    private string ValuesSection => "section.four-col-textarea";
    private string H3Selector => "h3";
    private By GetStartedButtonLocator => By.CssSelector("a.btn[href='/contact']");

    public CompanyPage(IWebDriver driver)
    {
        this.driver = driver;
    }

    public List<string> GetPageValues()
    {
        List<string> values = new List<string>();

        try
        {
            // Locate the section containing the values
            IWebElement section = driver.FindElement(By.CssSelector(ValuesSection));

            // Find all h3 elements within the section
            IReadOnlyCollection<IWebElement> h3Elements = section.FindElements(By.TagName(H3Selector));

            // Extract text from each h3 element
            foreach (IWebElement h3Element in h3Elements)
            {
                values.Add(h3Element.Text);
            }
        }
        catch (NoSuchElementException)
        {
            // Handle the case when the element is not found
            Console.WriteLine("Error: Section or h3 elements not found");
        }

        return values;
    }

    public By GetStartedButton()
    {
        return GetStartedButtonLocator;
    }
}
