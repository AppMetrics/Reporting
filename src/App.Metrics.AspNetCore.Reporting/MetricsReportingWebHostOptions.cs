// <copyright file="MetricsReportingWebHostOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Reporting;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore.Reporting
{
    /// <summary>
    ///     Provides programmatic configuration for metrics reporting in the App Metrics framework.
    /// </summary>
    public class MetricsReportingWebHostOptions
    {
        public MetricsReportingWebHostOptions() { ReportingOptions = options => { }; }

        /// <summary>
        ///     Gets or sets <see cref="Action{IMetricsReportingBuilder}" /> to configure the provided <see cref="IMetricsReportingBuilder" /> allowing
        ///     reporters to be configured.
        /// </summary>
        public Action<IMetricsReportingBuilder> ReportingBuilder { get; set; }

        /// <summary>
        ///     Gets or sets <see cref="Action{MetricsReportingOptions}" /> to configure the provided
        ///     <see cref="MetricsReportingOptions" />.
        /// </summary>
        public Action<MetricsReportingOptions> ReportingOptions { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="EventHandler" /> registered with an exception is thrown in one of the registered
        ///     reproter providers.
        /// </summary>
        public EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskExceptionHandler { get; set; }
    }
}