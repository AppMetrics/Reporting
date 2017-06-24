// <copyright file="ConsoleReporterSettings.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Reporting.Console
{
    public class ConsoleReporterSettings : IReporterSettings
    {
        /// <inheritdoc />
        public MetricValueDataKeys DataKeys { get; set; }

        public Func<string, string, string> MetricNameFormatter { get; set; }

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; set; } = TimeSpan.FromSeconds(30);
    }
}