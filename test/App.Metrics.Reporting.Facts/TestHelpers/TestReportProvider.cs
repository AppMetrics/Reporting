// <copyright file="TestReportProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filtering;
using App.Metrics.Filters;

namespace App.Metrics.Reporting.Facts.TestHelpers
{
    public class TestReportProvider : IReporterProvider
    {
        private readonly bool _pass;
        private readonly Exception _throwEx;

        public TestReportProvider(bool pass = true, Exception throwEx = null)
        {
            _pass = throwEx == null && pass;
            _throwEx = throwEx;
            ReportInterval = TimeSpan.FromMilliseconds(10);
            Filter = new NoOpMetricsFilter();
        }

        public TestReportProvider(TimeSpan interval, bool pass = true, Exception throwEx = null)
        {
            ReportInterval = interval;
            _pass = throwEx == null && pass;
            _throwEx = throwEx;
            ReportInterval = TimeSpan.FromMilliseconds(10);
            Filter = new NoOpMetricsFilter();
        }

        // ReSharper disable UnassignedGetOnlyAutoProperty
        public IFilterMetrics Filter { get; }

        public TimeSpan ReportInterval { get; }

        public Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(true);
        }

        // ReSharper restore UnassignedGetOnlyAutoProperty
    }
}