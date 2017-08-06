// <copyright file="CustomMetricsOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Formatters;

namespace AppMetrics.Reporters.Sandbox.CustomMetricConsoleFormatting
{
    public class CustomMetricsOutputFormatter : IMetricsOutputFormatter
    {
        public MetricsMediaTypeValue MediaType => new MetricsMediaTypeValue("text", "vnd.appmetrics.custom", "v1", "plain");

        public Task WriteAsync(
            Stream output,
            MetricsDataValueSource metricsData,
            Encoding encoding,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var payloadBuilder = new CustomMetricPayloadBuilder();
            var formatter = new MetricDataValueSourceFormatter();

            formatter.Build(metricsData, payloadBuilder);

            var bytes = encoding.GetBytes(payloadBuilder.PayloadFormatted());

            return output.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        }
    }
}
