// <copyright file="TextFileReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading.Tasks;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Abstractions;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.TextFile
{
    public class TextFileReporterProvider<TPayload> : IReporterProvider
    {
        private readonly IMetricPayloadBuilder<TPayload> _payloadBuilder;
        private readonly TextFileReporterSettings _settings;

        public TextFileReporterProvider(TextFileReporterSettings settings, IMetricPayloadBuilder<TPayload> payloadBuilder, IFilterMetrics fitler)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _payloadBuilder = payloadBuilder ?? throw new ArgumentNullException(nameof(payloadBuilder));
            Filter = fitler;
        }

        public IFilterMetrics Filter { get; }

        public IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory)
        {
            var file = new FileInfo(_settings.FileName);
            file.Directory?.Create();

            return new ReportRunner<TPayload>(
                p =>
                {
                    File.WriteAllText(_settings.FileName, p.PayloadFormatted());

                    return AppMetricsTaskCache.SuccessTask;
                },
                _payloadBuilder,
                _settings.ReportInterval,
                name,
                loggerFactory);
        }
    }
}