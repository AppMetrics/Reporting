// <copyright file="MetricsReportingBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Reporting.Internal;
using App.Metrics.Reporting.Internal.NoOp;
using App.Metrics.Scheduling;

namespace App.Metrics.Reporting.Builder
{
    public class MetricsReportingBuilder : IMetricsReportingBuilder
    {
        private readonly List<IMetricsReporterProvider> _reporterProviders = new List<IMetricsReporterProvider>();
        private readonly IScheduler _scheduler = new DefaultTaskScheduler();
        private MetricsReportingOptions _options = new MetricsReportingOptions();

        public MetricsReportingBuilder(IMetricsRoot metrics)
        {
            Metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
        }

        public IMetricsRoot Metrics { get; }

        /// <inheritdoc />
        public MetricsReportingOptionsBuilder Options
        {
            get
            {
                return new MetricsReportingOptionsBuilder(
                    this,
                    options => { _options = options; });
            }
        }

        /// <inheritdoc />
        public MetricsReporterProviderBuilder ReportMetrics => new MetricsReporterProviderBuilder(
            this,
            reporterProvider => { _reporterProviders.Add(reporterProvider); });

        /// <inheritdoc />
        public IMetricsReporter Build()
        {
            // TODO: Or !MetricsRoot.Options.Enabled ||
            if (!_reporterProviders.Any() || !_options.Enabled)
            {
                return new NoOpMetricsReporter();
            }

            return new DefaultMetricsReporter(_reporterProviders, _scheduler);
        }
    }
}