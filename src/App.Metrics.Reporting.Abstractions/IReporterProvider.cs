// <copyright file="IReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;

namespace App.Metrics.Reporting
{
    public interface IReporterProvider
    {
        IFilterMetrics Filter { get; }

        TimeSpan ReportInterval { get; }

        Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default(CancellationToken));
    }
}