using OpenQA.Selenium;
using SpecFlowProject_Test.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowProject_Test.Steps
{
    [Binding]
    public sealed class CommonSteps : TechTalk.SpecFlow.Steps
    {
        #region Variables

        private readonly BasePage basePage;

        #endregion

        #region Consturctors

        public CommonSteps(IWebDriver driver)
        {

            basePage = new BasePage(driver);
        }

        #endregion

        #region When


        [When(@"I login as (.*)")]
        public void WhenILoginAs(string userName)
        {
            basePage.NavigateToLoginPage();
            basePage.LoginPageM().EnterUserName(userName);
        }

        #endregion

    }
}
