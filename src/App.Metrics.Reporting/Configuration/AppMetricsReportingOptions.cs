// <copyright file="AppMetricsReportingOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace App.Metrics.Reporting.Configuration
{
    [ExcludeFromCodeCoverage]
    public sealed class AppMetricsReportingOptions
    {
        /// <summary>
        ///     Gets or sets a value indicating whether [reporting enabled].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [reporting enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool ReportingEnabled { get; set; } = true;
    }
}
