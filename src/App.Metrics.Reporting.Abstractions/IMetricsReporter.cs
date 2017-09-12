// <copyright file="IMetricsReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting
{
    public interface IMetricsReporter : IDisposable
    {
        void ScheduleReports(IMetrics metrics, CancellationToken cancellationToken = default);

        IEnumerable<Task> RunReportsAsync(IMetrics metrics, CancellationToken cancellationToken = default);
    }
}