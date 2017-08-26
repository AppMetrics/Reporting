// <copyright file="DefaultMetricsReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Counter;
using App.Metrics.Logging;
using App.Metrics.Scheduling;

namespace App.Metrics.Reporting.Internal
{
    public class DefaultMetricsReporter : IMetricsReporter
    {
        private static readonly ILog Logger = LogProvider.For<DefaultMetricsReporter>();
        private readonly CounterOptions _failedCounter;
        private readonly IEnumerable<IReporterProvider> _reporterProviders;

        private readonly IMetrics _metrics;
        private readonly IScheduler _scheduler;

        private readonly CounterOptions _successCounter;

        public DefaultMetricsReporter(
            IEnumerable<IReporterProvider> reporterProviders,
            IMetrics metrics,
            IScheduler scheduler)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _reporterProviders = reporterProviders ?? throw new ArgumentNullException(nameof(reporterProviders));

            _successCounter = new CounterOptions
                              {
                                  Context = AppMetricsConstants.InternalMetricsContext,
                                  MeasurementUnit = Unit.Items,
                                  ResetOnReporting = true,
                                  Name = "report_success"
                              };

            _failedCounter = new CounterOptions
                             {
                                 Context = AppMetricsConstants.InternalMetricsContext,
                                 MeasurementUnit = Unit.Items,
                                 ResetOnReporting = true,
                                 Name = "report_failed"
                             };
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_scheduler != null)
            {
                using (_scheduler)
                {
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<Task> RunReportsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _reporterProviders.Select(provider => FlushMetrics(_metrics, cancellationToken, provider));
        }

        /// <inheritdoc />
        public void ScheduleReports(CancellationToken cancellationToken = default(CancellationToken))
        {
            var reportTasks = new List<Task>();

            foreach (var provider in _reporterProviders)
            {
                Logger.ReportRunning(provider);

                reportTasks.Add(ScheduleReport(_metrics, cancellationToken, provider).WithAggregateException());
            }

            try
            {
                Task.WaitAll(reportTasks.ToArray(), cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                Logger.ReportingCancelled(ex);
            }
            catch (AggregateException ex)
            {
                Logger.ReportingFailedDuringExecution(ex);
            }
            catch (ObjectDisposedException ex)
            {
                Logger.ReportingDisposedDuringExecution(ex);
            }
        }

        private Task ScheduleReport(
            IMetrics metrics,
            CancellationToken cancellationToken,
            IReporterProvider provider)
        {
            return _scheduler.Interval(
                provider.ReportInterval,
                TaskCreationOptions.LongRunning,
                async () => { await FlushMetrics(metrics, cancellationToken, provider); },
                cancellationToken);
        }

        private async Task FlushMetrics(IMetrics metrics, CancellationToken cancellationToken, IReporterProvider provider)
        {
            try
            {
                var result = await provider.FlushAsync(metrics.Snapshot.Get(provider.Filter), cancellationToken);

                if (result)
                {
                    _metrics.Measure.Counter.Increment(_successCounter, provider.GetType().Name);
                }
                else
                {
                    _metrics.Measure.Counter.Increment(_failedCounter, provider.GetType().Name);
                    Logger.ReportFailed(provider);
                }
            }
            catch (Exception ex)
            {
                _metrics.Measure.Counter.Increment(_failedCounter, provider.GetType().Name);
                Logger.ReportFailed(provider, ex);
            }
        }
    }
}