// <copyright file="NoOpReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace App.Metrics.Reporting.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    internal class NoOpReporter : IReporter
    {
        /// <inheritdoc />
        public void Dispose() { }

        /// <inheritdoc />
        public void RunReports(IMetrics context, CancellationToken token) { }
    }
}