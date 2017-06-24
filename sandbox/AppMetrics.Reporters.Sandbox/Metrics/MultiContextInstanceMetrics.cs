// <copyright file="MultiContextInstanceMetrics.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Timer;

namespace AppMetrics.Reporters.Sandbox.Metrics
{
    public class MultiContextInstanceMetrics
    {
        private static IMetrics _metrics;
        private readonly ICounter _instanceCounter;
        private readonly ITimer _instanceTimer;

        public MultiContextInstanceMetrics(string instanceName, IMetrics metrics)
        {
            _metrics = metrics;
            _instanceCounter = _metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.SampleCounter);
            _instanceTimer = _metrics.Provider.Timer.Instance(SampleMetricsRegistry.Timers.SampleTimer);
        }

        public void Run()
        {
            using (_instanceTimer.NewContext())
            {
                _instanceCounter.Increment();
            }
        }

        public void RunSample()
        {
            for (var i = 0; i < 5; i++)
            {
                new MultiContextInstanceMetrics("Sample Instance " + i, _metrics).Run();
            }
        }
    }
}