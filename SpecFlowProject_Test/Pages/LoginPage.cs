using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace SpecFlowProject_Test.Pages
{
    [Binding]
    public class LoginPage : BasePage
    {

        public LoginPage(IWebDriver driver) : base(driver)
        {
            this.driver = driver;
        }

        #region By

        private By UserNameBy
        {
            get { return By.XPath("//*[@id='email']"); }
        }

        #endregion

        #region WebObject

        public IWebElement UseName()
             => driver.FindElement(UserNameBy);

        #endregion

        #region Methods 


        public void EnterUserName(string username)
        {
            //wait.Until(ExpectedConditions.
            //string value = ConfigurationManager.AppSettings.GetValues("IsLocal");

            UseName().Clear();
            UseName().SendKeys(username);

        }
        #endregion
    }
}
