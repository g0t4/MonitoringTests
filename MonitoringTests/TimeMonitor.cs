namespace MonitoringTests
{
    using System;
    using System.Reactive.Linq;

    public class TimeMonitor : IDisposable
    {
        public TimeSpan NotifyAfter { get; private set; }
        private readonly Action<TimeMonitor> _Notify;
        public readonly DateTime Started;
        private readonly IDisposable _Observer;

        public TimeMonitor(TimeSpan notifyAfter, Action<TimeMonitor> notify)
        {
            NotifyAfter = notifyAfter;
            _Notify = notify;
            Started = DateTime.Now;
            // todo might want this as a paramter (optional) or maybe compute it off of the notifyAfter duration up to a max of every second or minute, in reality though if you need to know sooner than 100ms then this approach won't be accurate enough anyways and will probably be a performance issue, this time monitor is more for things like a 5 minute process and you want to know if it hits 10 minutes
            var checkFrequency = TimeSpan.FromMilliseconds(100);
            _Observer = Observable.Interval(checkFrequency)
                                  .Subscribe(i => CheckElapsedTime());
        }

        private void CheckElapsedTime()
        {
            var elapsed = DateTime.Now.Subtract(Started);
            if (elapsed < NotifyAfter)
            {
                return;
            }
            _Observer.Dispose();
            _Notify(this);
        }

        public void Dispose()
        {
            _Observer.Dispose();
        }
    }
}