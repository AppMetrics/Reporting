// <copyright file="ReportSchedulerHostedService.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Reporting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Reporting.Internal.Infrastructure
{
    public class ReportSchedulerHostedService : HostedService
    {
        private static readonly TimeSpan WaitBetweenReportRunChecks = TimeSpan.FromMilliseconds(500);
        private readonly ILogger<ReportSchedulerHostedService> _logger;
        private readonly IMetrics _metrics;
        private readonly List<SchedulerTaskWrapper> _scheduledReporterProviders = new List<SchedulerTaskWrapper>();

        public ReportSchedulerHostedService(
            ILogger<ReportSchedulerHostedService> logger,
            IMetrics metrics,
            IEnumerable<IReporterProvider> reporterProviders)
        {
            _logger = logger;
            _metrics = metrics;

            var referenceTime = DateTime.UtcNow;

            foreach (var provider in reporterProviders)
            {
                _scheduledReporterProviders.Add(
                    new SchedulerTaskWrapper
                    {
                        Interval = provider.ReportInterval,
                        Provider = provider,
                        NextRunTime = referenceTime
                    });
            }
        }

        public event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await ExecuteOnceAsync(cancellationToken);

                _logger.LogTrace($"Delaying for {WaitBetweenReportRunChecks}");
                await Task.Delay(WaitBetweenReportRunChecks, cancellationToken);
            }
        }

        private async Task ExecuteOnceAsync(CancellationToken cancellationToken)
        {
            var taskFactory = new TaskFactory(TaskScheduler.Current);
            var referenceTime = DateTime.UtcNow;

            foreach (var providerTask in _scheduledReporterProviders)
            {
                if (!providerTask.ShouldRun(referenceTime))
                {
                    _logger.LogTrace($"Skipping {providerTask.Provider.GetType().FullName}, next run in {providerTask.NextRunTime.Subtract(referenceTime).Milliseconds} ms");
                    continue;
                }

                providerTask.Increment();

                await taskFactory.StartNew(
                    async () =>
                    {
                        try
                        {
                            _logger.LogTrace($"Executing provider {providerTask.Provider.GetType().FullName} FlushAsync");
                            await providerTask.Provider.FlushAsync(
                                _metrics.Snapshot.Get(providerTask.Provider.Filter),
                                cancellationToken);
                            _logger.LogTrace($"Executed provider {providerTask.Provider.GetType().FullName} FlushAsync");
                        }
                        catch (Exception ex)
                        {
                            var args = new UnobservedTaskExceptionEventArgs(
                                ex as AggregateException ?? new AggregateException(ex));

                            _logger.LogError(5000, ex, "Reporter Provider Flush Failed");

                            UnobservedTaskException?.Invoke(this, args);

                            if (!args.Observed)
                            {
                                throw;
                            }
                        }
                    },
                    cancellationToken);
            }
        }

        private class SchedulerTaskWrapper
        {
            public TimeSpan Interval { get; set; }

            public DateTime LastRunTime { get; set; }

            public DateTime NextRunTime { get; set; }

            public IReporterProvider Provider { get; set; }

            public void Increment()
            {
                LastRunTime = NextRunTime;
                NextRunTime = DateTime.UtcNow.Add(Interval);
            }

            public bool ShouldRun(DateTime currentTime) { return NextRunTime < currentTime && LastRunTime != NextRunTime; }
        }
    }
}