// <copyright file="TextFileReporterSettings.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Text;

namespace App.Metrics.Reporting.TextFile
{
    public class TextFileReporterSettings : IReporterSettings
    {
        /// <inheritdoc />
        public MetricValueDataKeys DataKeys { get; set; }

        public string FileName { get; set; }

        public Func<string, string, string> MetricNameFormatter { get; set; }

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; set; } = TimeSpan.FromSeconds(5);

        public bool AppendMetricsToTextFile { get; set; } = false;

        public Encoding TextFileEncoding { get; set; } = Encoding.UTF8;
    }
}