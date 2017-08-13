// <copyright file="MetricsReportingConsoleOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Reporting.Console.Internal
{
    /// <summary>
    ///     Sets up default conole reporting options for <see cref="MetricsReportingConsoleOptions"/>.
    /// </summary>
    public class MetricsReportingConsoleOptionsSetup : IConfigureOptions<MetricsReportingConsoleOptions>
    {
        private readonly MetricsOptions _metricsOptionsAccessor;

        public MetricsReportingConsoleOptionsSetup(IOptions<MetricsOptions> metricsOptionsAccessor)
        {
            _metricsOptionsAccessor = metricsOptionsAccessor.Value ?? throw new ArgumentNullException(nameof(metricsOptionsAccessor));
        }

        public void Configure(MetricsReportingConsoleOptions options)
        {
            if (options.MetricsOutputFormatter == null)
            {
                options.MetricsOutputFormatter = _metricsOptionsAccessor.DefaultOutputMetricsFormatter;
            }
        }
    }
}