// <copyright file="IMetricsReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting
{
    public interface IMetricsReporter : IDisposable
    {
        void ScheduleReports(CancellationToken cancellationToken = default(CancellationToken));

        Task RunReportsAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}