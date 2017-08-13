// <copyright file="MetricsReportingConsoleOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using Microsoft.Extensions.Options;

namespace App.Metrics.Reporting.Console.Internal
{
    /// <summary>
    ///     Sets up default Console Reporting options for <see cref="MetricsReportingConsoleOptions"/>.
    /// </summary>
    public class MetricsReportingConsoleOptionsSetup : IConfigureOptions<MetricsReportingConsoleOptions>
    {
        private readonly MetricsOptions _metricsOptionsAccessor;

        public MetricsReportingConsoleOptionsSetup(IOptions<MetricsOptions> metricsOptionsAccessor)
        {
            _metricsOptionsAccessor = metricsOptionsAccessor.Value ?? throw new ArgumentNullException(nameof(metricsOptionsAccessor));
        }

        /// <inheritdoc/>
        public void Configure(MetricsReportingConsoleOptions options)
        {
            if (options.MetricsOutputFormatter == default(IMetricsOutputFormatter))
            {
                var textFormatter = _metricsOptionsAccessor.OutputMetricsFormatters.GetType<MetricsTextOutputFormatter>();

                options.MetricsOutputFormatter = textFormatter == default(IMetricsOutputFormatter)
                    ? _metricsOptionsAccessor.DefaultOutputMetricsFormatter
                    : textFormatter;
            }
        }
    }
}