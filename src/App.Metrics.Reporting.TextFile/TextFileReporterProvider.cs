// <copyright file="TextFileReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading.Tasks;
using App.Metrics.Core.Filtering;
using App.Metrics.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace App.Metrics.Reporting.TextFile
{
    public class TextFileReporterProvider<TPayload> : IReporterProvider
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMetricPayloadBuilder<TPayload> _payloadBuilder;
        private readonly TextFileReporterSettings _settings;

        public TextFileReporterProvider(
            TextFileReporterSettings settings,
            ILoggerFactory loggerFactory,
            IMetricPayloadBuilder<TPayload> payloadBuilder,
            IFilterMetrics filter)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _payloadBuilder = payloadBuilder ?? throw new ArgumentNullException(nameof(payloadBuilder));

            _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
            Filter = filter ?? new NoOpMetricsFilter();
        }

        public IFilterMetrics Filter { get; }

        public IMetricReporter CreateMetricReporter(string name)
        {
            var file = new FileInfo(_settings.FileName);
            file.Directory?.Create();

            return new ReportRunner<TPayload>(
                p =>
                {
                    File.WriteAllText(_settings.FileName, p.PayloadFormatted());

                    return Task.FromResult(true);
                },
                _payloadBuilder,
                _settings.ReportInterval,
                name,
                _loggerFactory);
        }
    }
}