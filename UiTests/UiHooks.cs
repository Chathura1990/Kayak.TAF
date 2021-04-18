using Core;
using Core.Utils;
using NUnit.Framework;

namespace UiTests
{
    [SetUpFixture]
    public class UiHooks
    {
        private readonly Logger Logger = Logger.Instance;

        [OneTimeSetUp]
        public void BeforeFeature()
        {
            Logger.Debug("[OneTimeSetUp] Global SetUp is started");

            ResourceUtils.DeleteDirectory(ResourceUtils.GetRootPath("Screenshots"));

            Logger.Debug("[OneTimeSetUp] Global SetUp is finished");
        }
    }
}