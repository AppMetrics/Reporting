// <copyright file="MetricsReportingHttpOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Reporting.Http.Internal
{
    /// <summary>
    ///     Sets up default Text File Reporting options for <see cref="MetricsReportingHttpOptions"/>.
    /// </summary>
    public class MetricsReportingHttpOptionsSetup : IConfigureOptions<MetricsReportingHttpOptions>
    {
        private readonly MetricsOptions _metricsOptionsAccessor;

        public MetricsReportingHttpOptionsSetup(IOptions<MetricsOptions> metricsOptionsAccessor)
        {
            _metricsOptionsAccessor = metricsOptionsAccessor.Value ?? throw new ArgumentNullException(nameof(metricsOptionsAccessor));
        }

        public void Configure(MetricsReportingHttpOptions options)
        {
            if (options.MetricsOutputFormatter == null)
            {
                options.MetricsOutputFormatter = _metricsOptionsAccessor.DefaultOutputMetricsTextFormatter;
            }
        }
    }
}