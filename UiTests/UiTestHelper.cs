using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Core;
using Core.Configurations;
using Core.Extensions;
using Core.Selenium.WebDriver;
using Core.UI;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UiSteps.Steps;
using Unity;

namespace UiTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class UiTestHelper
    {
        protected UiTestHelper That => (UiTestHelper)MemberwiseClone(); // Workaround for BDDfy to work in parallel

        protected ThreadLocal<StepsContext> StepsContext = new ThreadLocal<StepsContext>();

        protected static Logger Logger = Logger.Instance;

        public ThreadLocal<TestRunner> Runner = new ThreadLocal<TestRunner>();

        public List<Attribute> Attributes = new List<Attribute>();

        protected static DateTime LocalToday = DateTimeExtensions.LocalToday;
        protected static DateTimeOffset Now = DateTimeOffset.UtcNow;

        private UnityContainer _container;

        #region Steps

        protected BaseUiSteps BaseUiSteps => _container.Resolve<BaseUiSteps>();
        protected FlightSearchUiSteps FlightSearchUiSteps => _container.Resolve<FlightSearchUiSteps>();
        protected FlightSearchResultUiSteps FlightSearchResultUiSteps => _container.Resolve<FlightSearchResultUiSteps>();

        #endregion Steps
        
        [OneTimeSetUp]
        public void GlobalSetUp()
        {
            Logger.Debug("[OneTimeSetUp] Feature SetUp is started");

            StepsContext.Value = new StepsContext();

            _container = new UnityContainer();

            _container.RegisterInstance(StepsContext.Value)
                .RegisterType<HttpClient>()
                .RegisterType<BaseUiSteps>()
                .RegisterType<FlightSearchUiSteps>()
                .RegisterType<FlightSearchResultUiSteps>();

            Logger.Debug("[UNITY] Container is initialized");
            Logger.Debug("[OneTimeSetUp] Feature SetUp is finished");
        }

        [SetUp]
        public void BeforeScenario()
        {
            Logger.Debug($"[TEST] {TestContext.CurrentContext.Test.Name}");
            Runner.Value = new TestRunner(ConfigManager.Browser.Type);
            NavigateToUrl();
        }

        [TearDown]
        public void AfterScenario()
        {
            TakeScreenshotForFailedTest();
            Runner.Value.CloseBrowsers();
            StepsContext.Value.Clear();
        }

        private void NavigateToUrl()
        {
            Browser.Current.NavigateToUrl(ConfigManager.Browser.StartUrl);
        }
        
        private void TakeScreenshotForFailedTest()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var fileNameBase = ShortenScreenshotFileName() + $"_{DateTime.Now:ddMMyy_HHmmss}";
                Browser.Current.CaptureScreen(fileNameBase);
            }
        }

        private string ShortenScreenshotFileName()
        {
            var testName = $"VIS_{TestContext.CurrentContext.Test.FullName.Split("VIS").Last()}";
            testName = testName.Split("(")[0];

            // File name length restriction in some DevOps tools such as Azure
            if (testName.Length > 80) testName = testName.Substring(0, 80);

            return testName;
        }
    }
}