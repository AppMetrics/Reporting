// <copyright file="MetricsReportingOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Filtering;
using App.Metrics.Filters;

namespace App.Metrics.Reporting
{
    public class MetricsReportingOptions
    {
        public MetricsReportingOptions()
        {
            Filter = new NoOpMetricsFilter();
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [reporting enabled].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [reporting enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; } = true;

        public IFilterMetrics Filter { get; }
    }
}