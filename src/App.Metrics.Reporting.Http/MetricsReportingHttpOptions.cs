// <copyright file="MetricsReportingHttpOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Text;
using App.Metrics.Formatters;
using App.Metrics.Reporting.Http.Client;

namespace App.Metrics.Reporting.Http
{
    public class MetricsReportingHttpOptions
    {
        public MetricsReportingHttpOptions()
        {
            DataKeys = new MetricValueDataKeys();
            ReportInterval = TimeSpan.FromSeconds(10);
            HttpSettings = new HttpSettings();
            HttpPolicy = new HttpPolicy();
        }

        public bool AppendMetricsToTextFile { get; set; } = false;

        public MetricValueDataKeys DataKeys { get; set; }

        /// <summary>
        ///     Gets or sets the HTTP policy settings which allows circuit breaker configuration to be adjusted
        /// </summary>
        /// <value>
        ///     The HTTP policy.
        /// </value>
        public HttpPolicy HttpPolicy { get; set; }

        /// <summary>
        ///     Gets or sets the HTTP client related settings.
        /// </summary>
        /// <value>
        ///     The HTTP client settings.
        /// </value>
        public HttpSettings HttpSettings { get; set; }

        /// <summary>
        ///     Gets or sets the inner HTTP message handler to be used with the HTTP Client.
        /// </summary>
        /// <value>
        ///     The inner HTTP message handler.
        /// </value>
        public HttpMessageHandler InnerHttpMessageHandler { get; set; }

        public Func<string, string, string> MetricNameFormatter { get; set; }

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

        public Encoding TextFileEncoding { get; set; } = Encoding.UTF8;
    }
}