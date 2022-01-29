using BoDi;
using OpenQA.Selenium;
using SpecFlowProject_Test.Drivers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Chrome;

namespace SpecFlowProject_Test.Hooks
{
    [Binding]
    class BeforeAndAfter
    {


        private readonly IObjectContainer container;
        private readonly DriverManager driverMngr;
        public IWebDriver webDriver;


        public BeforeAndAfter(IObjectContainer container, DriverManager driverMngr)
        {
            this.container = container;
            this.driverMngr = driverMngr;
        }


        [BeforeScenario]
        public void Initialise()
        {

            string text =ConfigurationManager.AppSettings["IsLocal"];
            Console.WriteLine(text);
            //webDriver = new ChromeDriver();
            webDriver = driverMngr.GetDriver();
            container.RegisterInstanceAs(webDriver);

        }

        [AfterScenario()]
        public void TeardownAccountScenario()
        {
            webDriver.Quit();
        }

    }
}
