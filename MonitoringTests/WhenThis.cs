namespace MonitoringTests
{
    using System;

    public class WhenThis
    {
        private TimeSpan _RunsOver;

        public static WhenThis RunsOver(TimeSpan runsOver)
        {
            return new WhenThis
                {
                    _RunsOver = runsOver
                };
        }

        public TimeMonitor Then(Action<TimeMonitor> then)
        {
            return new TimeMonitor(_RunsOver, then);
        }
    }
}