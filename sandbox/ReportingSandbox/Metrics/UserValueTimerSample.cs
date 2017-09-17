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
        private readonly ITimer _timer;

        public UserValueTimerSample(IMetrics metrics)
        {
            _timer = metrics.Provider.Timer.Instance(SampleMetricsRegistry.Timers.Requests);
        }

        public static void RunSomeRequests(IMetrics metrics)
        {
            for (var i = 0; i < 30; i++)
            {
                var documentId = new Random().Next(10);
                new UserValueTimerSample(metrics).Process("document-" + documentId);
            }
        }

        private static void ActualProcessingOfTheRequest() { Thread.Sleep(new Random().Next(1000)); }

        private void Process(string documentId)
        {
            using (_timer.NewContext(documentId))
            {
                ActualProcessingOfTheRequest();
            }
        }
    }
}