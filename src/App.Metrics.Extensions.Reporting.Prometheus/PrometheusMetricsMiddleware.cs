// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace App.Metrics.Extensions.Reporting.Prometheus
{
    public class PrometheusMetricsMiddleware
    {
        private const string MetricsContentTypeText = "text/plain";
        private const string MetricsContentType = "application/vnd.google.protobuf; proto=io.prometheus.client.MetricFamily; encoding=delimited";

        private readonly IMetrics _metrics;
        private readonly IOptions<PrometheusMetricsMiddlewareOptions> _options;

        public PrometheusMetricsMiddleware(RequestDelegate next, IMetrics metrics, IOptions<PrometheusMetricsMiddlewareOptions> options)
        {
            _metrics = metrics;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            if (_options.Value.UseProtobuf)
            {
                context.Response.ContentType = MetricsContentType;
                var bodyData = ProtoFormatter.Format(_metrics.GetPrometheusMetricsSnapshot());
                await context.Response.Body.WriteAsync(bodyData, 0, bodyData.Length);
            }
            else
            {
                context.Response.ContentType = MetricsContentTypeText;
                await context.Response.WriteAsync(AsciiFormatter.Format(_metrics.GetPrometheusMetricsSnapshot()));
            }
        }
    }
}
