using System;
using OpenQA.Selenium;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Gherkin.Model;


namespace SpecFlowProject_Test.Hooks
{
    [Binding]
    public sealed class SaveScreenShots
    {
        #region Constants
        private const string IMAGE_FORMAT = ".png";
        private const string BACKLASH_ESCAPE = "\\";
        private const string US_CULTURE = "en-US";
        #endregion

        #region Instance Variable

        private bool takeScreenShots = Boolean.Parse(ConfigurationManager.AppSettings["TakeScreenShots"]);
        public static IWebDriver webDriver;

        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;
        private readonly TestContext testContext;

        private static ExtentTest featureName;
        private static ExtentTest scenario;
        private static ExtentReports extent;
        #endregion

        #region Constructor

        public SaveScreenShots(IWebDriver driver, TestContext testContext, FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            webDriver = driver;
            this.testContext = testContext;
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;
        }

        #endregion
        #region hooks
        [BeforeTestRun]
        public static void InitializeReport()
        {
            //Initilise Extent report before test starts
            string path1 = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "");
            string path = path1 + "Report\\index.html";
            ExtentHtmlReporter htmlReporter = new ExtentHtmlReporter(path);
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            //create dynamic fature name
            var featureTitle = featureContext.FeatureInfo.Title;
            featureName = extent.CreateTest<Feature>(featureTitle);
        }

        [BeforeScenario]
        public static void BeforeScenario(ScenarioContext scenarioContext)
        {
            scenario = featureName.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        [AfterStep]
        public void InsertReportSteps()
        {
            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();

            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "When")
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "Then")
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
            }
            else if (scenarioContext.TestError != null)
            {
                var failedScreen = TakescreenC(scenarioContext.ScenarioInfo.Title.Trim());

                if (stepType == "Given")
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message, failedScreen);
                else if (stepType == "When")
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message, failedScreen);
                else if (stepType == "And")
                    scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message, failedScreen);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message, failedScreen);
            }

            if (ScenarioContext.Current.ScenarioExecutionStatus.ToString() == "StepDefinitionPending")
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "When")
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "And")
                    scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");

            }
        }

        [AfterStep]
        public void TakeSreenShotAtFailStep()
        {
            if (takeScreenShots && scenarioContext.TestError != null)
            {
                SaveScreenShot(featureContext, scenarioContext, testContext);
            }
        }
        [AfterTestRun]
        public static void tearDownReport()
        {
            //webDriver.Close();
            //webDriver.Dispose();
            extent.Flush();
        }
        #endregion

        #region Methods

        public void SaveScreenShot(FeatureContext featureContext, ScenarioContext scenarioContext, TestContext testContext)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var featureTitle = featureContext.FeatureInfo.Title;
            var scenarioTitle = scenarioContext.ScenarioInfo.Title;
            var screenshotPath = currentDirectory + BACKLASH_ESCAPE + RemoveSpecialCharacters(featureTitle);
            var screenShotName = RemoveSpecialCharacters(scenarioTitle) + IMAGE_FORMAT;
            var imagePath = screenshotPath + BACKLASH_ESCAPE + screenShotName;

            Directory.CreateDirectory(screenshotPath);
            TakeScreenShot(imagePath);
            testContext.AddResultFile(imagePath);

        }

        public void TakeScreenShot(string path)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)webDriver).GetScreenshot();
                screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
            }
            catch (Exception e)
            {

            }

        }

        public string RemoveSpecialCharacters(string value)
        {
            var textInfo = new CultureInfo(US_CULTURE, false).TextInfo;
            var rgx = new Regex("[^a-zA-Z0-9]");
            return rgx.Replace(textInfo.ToTitleCase(value), string.Empty);
        }

        public MediaEntityModelProvider TakescreenC(string screenName)
        {
            var screenshot = ((ITakesScreenshot)webDriver).GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, screenName).Build();

        }
        #endregion
    }
}
