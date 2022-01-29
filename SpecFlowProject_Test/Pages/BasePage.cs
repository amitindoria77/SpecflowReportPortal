using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowProject_Test.Pages
{
    [Binding]
    public class BasePage
    {
        public IWebDriver driver;
        public Actions action;
        public WebDriverWait wait;
        public IJavaScriptExecutor executor;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            action = new Actions(driver);
            executor = (IJavaScriptExecutor)driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["WaitSeconds"])));
        }



        #region WebObjects

        public LoginPage LoginPageM()
        {
            return new LoginPage(driver);
        }
        #endregion

        #region Methods
        public void NavigateToLoginPage()
        {

            driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["StartPage"]);
        }


        #endregion
    }
}
