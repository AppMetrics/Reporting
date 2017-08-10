// <copyright file="MetricsReportingOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Filtering;
using App.Metrics.Filters;

namespace App.Metrics.Reporting
{
    public class MetricsReportingOptions
    {
        public MetricsReportingOptions() { Filter = new NoOpMetricsFilter(); }

        /// <summary>
        ///     Gets or sets a value indicating whether [reporting enabled].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [reporting enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; } = true;

        /// <summary>
        ///    Gets or sets the <see cref="IFilterMetrics"/> which overrides the global metrics snapshot filter set on <see cref="MetricsOptions" /> for all reporters.
        /// </summary>
        /// <value>
        ///     The <see cref="IFilterMetrics" /> to use instead of the global metrics snapshot filter set on
        ///     <see cref="MetricsOptions" /> .
        /// </value>
        public IFilterMetrics Filter { get; set; }
    }
}