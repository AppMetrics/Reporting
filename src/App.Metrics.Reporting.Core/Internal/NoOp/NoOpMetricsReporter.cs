// <copyright file="NoOpMetricsReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        public void ScheduleReports(IMetrics metrics, CancellationToken cancellationToken = default) { }

        /// <inheritdoc />
        public IEnumerable<Task> RunReportsAsync(IMetrics metrics, CancellationToken cancellationToken = default) { return Enumerable.Empty<Task>(); }
    }
}