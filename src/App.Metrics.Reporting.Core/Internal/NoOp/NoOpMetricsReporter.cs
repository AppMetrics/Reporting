// <copyright file="NoOpMetricsReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    internal class NoOpMetricsReporter : IMetricsReporter
    {
        /// <inheritdoc />
        public void Dispose() { }

        /// <inheritdoc />
        public void ScheduleReports(CancellationToken cancellationToken = default(CancellationToken)) { }

        /// <inheritdoc />
        public Task RunReportsAsync(CancellationToken cancellationToken = default(CancellationToken)) { return Task.CompletedTask; }
    }
}