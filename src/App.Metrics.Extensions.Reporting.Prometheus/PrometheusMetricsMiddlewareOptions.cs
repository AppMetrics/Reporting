// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Extensions.Reporting.Prometheus
{
    public class PrometheusMetricsMiddlewareOptions
    {
        public bool UseProtobuf { get; set; } = false;
    }
}