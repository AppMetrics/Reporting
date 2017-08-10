// <copyright file="UserValueTimerSample.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using App.Metrics;
using App.Metrics.Timer;

namespace ReportingSandbox.Metrics
{
    public class UserValueTimerSample
    {
        private static IMetrics _metrics;
        private readonly ITimer _timer;

        public UserValueTimerSample(IMetrics metrics)
        {
            _metrics = metrics;

            _timer = _metrics.Provider.Timer.Instance(SampleMetricsRegistry.Timers.Requests);
        }

        public void Process(string documentId)
        {
            using (var context = _timer.NewContext(documentId))
            {
                ActualProcessingOfTheRequest(documentId);

                // if needed elapsed time is available in context.Elapsed
            }
        }

        public void RunSomeRequests()
        {
            for (var i = 0; i < 30; i++)
            {
                var documentId = new Random().Next(10);
                new UserValueTimerSample(_metrics).Process("document-" + documentId.ToString());
            }
        }

        private void ActualProcessingOfTheRequest(string documentId) { Thread.Sleep(new Random().Next(1000)); }

        private void LogDuration(TimeSpan time) { }
    }
}