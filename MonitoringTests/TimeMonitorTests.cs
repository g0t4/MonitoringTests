namespace MonitoringTests
{
    using System;
    using System.Threading;
    using NUnit.Framework;
    using Properties;

    [TestFixture]
    public class TimeMonitorTests : AssertionHelper
    {
        [Test]
        public void LongRunningMethod_RunsOverExpected_Notifies()
        {
            var ranOver = false;

            using (new TimeMonitor(TimeSpan.FromMilliseconds(100), m => ranOver = true))
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(400));
            }

            Expect(ranOver);
        }

        [Test]
        public void LongRunningMethod_UnderExpected_DoesNotNotify()
        {
            var ranOver = false;

            using (new TimeMonitor(TimeSpan.FromMilliseconds(400), m => ranOver = true))
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
            }

            Expect(ranOver, Is.False);
        }

        public void Sample()
        {
            using (WhenThis.RunsOver(Settings.Default.SettlementsWarningDuration).Then(SendSettlementsWarning))
            {
                ProcessSettlements();
            }
        }

        private void ProcessSettlements()
        {
        }

        private void SendSettlementsWarning(TimeMonitor monitor)
        {
        }
    }
}