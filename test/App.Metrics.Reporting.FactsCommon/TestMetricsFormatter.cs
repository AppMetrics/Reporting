// <copyright file="TestMetricsFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters;

namespace App.Metrics.Reporting.FactsCommon
{
    public class TestMetricsFormatter : IMetricsOutputFormatter
    {
        public TestMetricsFormatter()
        {
            MediaType = new MetricsMediaTypeValue("test", "test", "v1", "format");
        }

        public Task WriteAsync(Stream output, MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public MetricsMediaTypeValue MediaType { get; }
    }
}
