// <copyright file="MetricsReportingTextFileOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using App.Metrics.Formatters;

namespace App.Metrics.Reporting.TextFile
{
    public class MetricsReportingTextFileOptions
    {
        public MetricsReportingTextFileOptions()
        {
            ReportInterval = TimeSpan.FromSeconds(10);
            Filter = new NoOpMetricsFilter();
        }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to [append metrics when writing to file].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [append metrics]; otherwise, <c>false</c>.
        /// </value>
        public bool AppendMetricsToTextFile { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IFilterMetrics" /> to use for just this reporter.
        /// </summary>
        /// <value>
        ///     The <see cref="IFilterMetrics" /> to use for this reporter.
        /// </value>
        public IFilterMetrics Filter { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </summary>
        /// <value>
        ///     The <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </value>
        public IMetricsOutputFormatter MetricsOutputFormatter { get; set; }

        public string OutputPathAndFileName { get; set; }

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