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
            // code that should normally run in a certain duration but may eventually or randomly take longer and someone wants to know about it
        }

        private void SendSettlementsWarning(TimeMonitor monitor)
        {
            // put whatever code to send the warning here, this just shows how you can use the constraints of the monitor to format the message
            var message = new
                {
                    Subject = "Settlements Are Taking Longer Than Expected",
                    Body = "Process started at " + monitor.Started + " and is expected to complete in " + monitor.NotifyAfter + " but it's " + DateTime.Now " and they are not yet complete.",
                    //Recipients
                };
            // message.Send();
        }
    }
}