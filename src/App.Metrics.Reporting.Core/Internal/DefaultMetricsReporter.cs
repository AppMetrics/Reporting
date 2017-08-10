// <copyright file="DefaultMetricsReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Counter;
using App.Metrics.Scheduling;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting.Internal
{
    public class DefaultMetricsReporter : IMetricsReporter
    {
        private readonly CounterOptions _failedCounter;
        private readonly ILogger<DefaultMetricsReporter> _logger;
        private readonly IEnumerable<IReporterProvider> _reporterProviders;

        private readonly IMetrics _metrics;
        private readonly IScheduler _scheduler;

        private readonly CounterOptions _successCounter;

        public DefaultMetricsReporter(
            IEnumerable<IReporterProvider> reporterProviders,
            IMetrics metrics,
            IScheduler scheduler,
            ILogger<DefaultMetricsReporter> logger)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _reporterProviders = reporterProviders ?? throw new ArgumentNullException(nameof(reporterProviders));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

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

        public Task RunReportsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var reportTasks = new List<Task>();

            foreach (var provider in _reporterProviders)
            {
                _logger.ReportRunning(provider);

                reportTasks.Add(ScheduleReport(_metrics, cancellationToken, provider).WithAggregateException());
            }

            try
            {
                return Task.WhenAll(reportTasks.ToArray());
            }
            catch (OperationCanceledException ex)
            {
                _logger.ReportingCancelled(ex);
            }
            catch (AggregateException ex)
            {
                _logger.ReportingFailedDuringExecution(ex);
            }
            catch (ObjectDisposedException ex)
            {
                _logger.ReportingDisposedDuringExecution(ex);
            }

            return Task.CompletedTask;
        }

        public void ScheduleReports(CancellationToken cancellationToken = default(CancellationToken))
        {
            var reportTasks = new List<Task>();

            foreach (var provider in _reporterProviders)
            {
                _logger.ReportRunning(provider);

                reportTasks.Add(ScheduleReport(_metrics, cancellationToken, provider).WithAggregateException());
            }

            try
            {
                Task.WaitAll(reportTasks.ToArray(), cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                _logger.ReportingCancelled(ex);
            }
            catch (AggregateException ex)
            {
                _logger.ReportingFailedDuringExecution(ex);
            }
            catch (ObjectDisposedException ex)
            {
                _logger.ReportingDisposedDuringExecution(ex);
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
                    _logger.ReportFailed(provider);
                }
            }
            catch (Exception ex)
            {
                _metrics.Measure.Counter.Increment(_failedCounter, provider.GetType().Name);
                _logger.ReportFailed(provider, ex);
            }
        }
    }
}