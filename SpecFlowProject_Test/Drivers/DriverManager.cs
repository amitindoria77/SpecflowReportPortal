using BoDi;
using OpenQA.Selenium;
using System;
using System.Configuration;
using OpenQA.Selenium.IE;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Edge;


namespace SpecFlowProject_Test.Drivers
{
    [Binding]
    public class DriverManager
    {
        // Variables
        public IObjectContainer objectContainer;
        public IWebDriver webDriver;

        public DriverManager(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;

        }


        public IWebDriver GetDriver()
        {
            //string v = ConfigurationManager.AppSettings.Get("PreferredDriver");
            if (ConfigurationManager.AppSettings.Get("PreferredDriver") == "#{ChromeDriver}")
            {
                if (ConfigurationManager.AppSettings["IsLocal"] != "true")

                {
                    Uri seleniumGrid = new Uri(ConfigurationManager.AppSettings["SeleniumGrid"]);
                    var options = new ChromeOptions();
                    options.AddArgument("--disable-gpu");
                    webDriver = new RemoteWebDriver(seleniumGrid, options);
                }

                else 
                {
                    webDriver = new ChromeDriver();
                }
                webDriver.Manage().Window.Maximize();
             }

            //else if (ConfigurationManager.AppSettings["PreferredDriver"] == "EdgeDriver")
            //{
            //    var anaheimService = EdgeDriverService.CreateDefaultService(@"D:\Users\B9321\Downloads\erdge driver\","msedgedriver.exe");
            //    webDriver = new EdgeDriver();
            //    webDriver.Manage().Window.Maximize();
            //}

            else if (ConfigurationManager.AppSettings["PreferredDriver"] == "IEDriver")
            {
                webDriver = new InternetExplorerDriver();
                webDriver.Manage().Window.Maximize();
            }

            return webDriver;
        }

        public void MaximizeTheSreen(IWebDriver driver)
        {
            driver.Manage().Window.Maximize();
        }

    }
}
