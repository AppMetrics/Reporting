// <copyright file="MetricsReportingMetricsReportingCoreOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.Options;

namespace App.Metrics.Reporting.Internal
{
    public class MetricsReportingMetricsReportingCoreOptionsSetup : IConfigureOptions<MetricsReportingOptions>
    {
        /// <inheritdoc />
        public void Configure(MetricsReportingOptions options)
        {
        }
    }
}