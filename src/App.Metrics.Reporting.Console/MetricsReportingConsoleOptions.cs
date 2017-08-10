// <copyright file="MetricsReportingConsoleOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters;

namespace App.Metrics.Reporting.Console
{
    public class MetricsReportingConsoleOptions
    {
        public MetricsReportingConsoleOptions()
        {
            ReportInterval = TimeSpan.FromSeconds(10);
        }

        public IMetricsOutputFormatter MetricsOutputFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the flush metrics interval
        /// </summary>
        /// <remarks>
        ///     This <see cref="TimeSpan" /> will apply to all configured reporters unless overriden by a specific reporters
        ///     options.
        /// </remarks>
        /// <value>
        ///     The <see cref="TimeSpan" /> to wait between reporting metrics
        /// </value>
        public TimeSpan ReportInterval { get; set; }
    }
}