using NUnit.Framework.Legacy;
using Serilog;

[TestFixture]
public class UiTests
{
    private SeleniumHelper seleniumHelper;

    [SetUp]
    public void SetUp()
    {
        seleniumHelper = new SeleniumHelper();
    }

    [Test]
    public void TestUiWorkflow()
    {
        string baseUrl = "https://www.agdata.com";

        // Perform the UI workflow
        seleniumHelper.NavigateToUrl(baseUrl);
        Log.Information("Navigated to URL: " + baseUrl);

        HomePage homePage = new HomePage(seleniumHelper.Driver);
        homePage.ClickCompanyElement();
        homePage.ClickOverviewLink();

        // Get all values on the page in a List
        CompanyPage companyPage = new CompanyPage(seleniumHelper.Driver);
        var pageValues = companyPage.GetPageValues();

        // Log each extracted value
        foreach (var value in pageValues)
        {
            Log.Information($"Extracted value: {value}");
        }

        // Click on the "Let's Get Started" button
        var getStartedButton = companyPage.GetStartedButton();
        seleniumHelper.ClickElement(getStartedButton);
        Log.Information("Clicked on 'Let's Get Started' button.");

        // Validate that the 'Contact' page is displayed/loaded 
        ContactsPage contactsPage = new ContactsPage(seleniumHelper.Driver);
        ClassicAssert.IsTrue(contactsPage.IsContactPageDisplayed());
        Log.Information("Contact page is displayed.");

        Log.Information("Test passed.");
    }

    [TearDown]
    public void TearDown()
    {
        Log.Information("Test teardown started.");

        seleniumHelper.Dispose();

        Log.Information("Test teardown completed.");
    }
}
