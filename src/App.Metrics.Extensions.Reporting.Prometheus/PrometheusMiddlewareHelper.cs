// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;

namespace App.Metrics.Extensions.Reporting.Prometheus
{
    public static class PrometheusMiddlewareHelper
    {
        public static IApplicationBuilder UsePrometheusMetrics(this IApplicationBuilder app)
        {
            app.Map("/metrics", x => x.UseMiddleware<PrometheusMetricsMiddleware>());
            return app;
        }
    }
}