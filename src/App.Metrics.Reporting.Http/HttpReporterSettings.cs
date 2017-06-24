// <copyright file="HttpReporterSettings.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using App.Metrics.Reporting.Http.Client;

namespace App.Metrics.Reporting.Http
{
    public class HttpReporterSettings : IReporterSettings
    {
        /// <inheritdoc />
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

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; set; } = TimeSpan.FromSeconds(30);
    }
}