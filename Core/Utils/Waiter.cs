using System;
using System.Threading;

namespace Core.Utils
{
    public static class Waiter
    {
        public static bool WaitUntil(
            Func<bool> condition,
            double pollingIntervalSeconds = 0.1,
            int timeoutSeconds = 5,
            bool showException = true,
            string error = "condition not reached and time is out")
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            TimeSpan timeForOne = TimeSpan.FromSeconds(pollingIntervalSeconds);
            DateTime timeout = DateTime.Now.AddSeconds(timeoutSeconds);

            while (DateTime.Now <= timeout)
            {
                sw.Restart();
                if (condition())
                {
                    return true;
                }
                sw.Stop();
                int rest = (timeForOne - sw.Elapsed).Milliseconds;
                if (rest > 0)
                {
                    Thread.Sleep(rest);
                }
            }
            if (showException)
            {
                throw new TimeoutException(error);
            }
            return false;
        }
    }
}