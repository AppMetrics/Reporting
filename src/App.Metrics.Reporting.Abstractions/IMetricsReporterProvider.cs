// <copyright file="IMetricsReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;

namespace App.Metrics.Reporting
{
    public interface IMetricsReporterProvider
    {
        /// <summary>
        ///     Gets the <see cref="IFilterMetrics" /> to use for just this reporter provider.
        /// </summary>
        /// <value>
        ///     The <see cref="IFilterMetrics" /> to use for this reporter provider.
        /// </value>
        IFilterMetrics Filter { get; }

        /// <summary>
        ///     Gets the flush metrics interval
        /// </summary>
        /// <remarks>
        ///     This <see cref="TimeSpan" /> will apply to all configured reporters unless overriden by a specific reporters
        ///     options.
        /// </remarks>
        /// <value>
        ///     The <see cref="TimeSpan" /> to wait between reporting metrics
        /// </value>
        TimeSpan ReportInterval { get; }

        /// <summary>
        /// Flushes the current metrics snapshot using the configured output formatter.
        /// </summary>
        /// <param name="metricsData">The current snapshot of metrics.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if metrics were successfully flushed, false otherwise.</returns>
        Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default);
    }
}