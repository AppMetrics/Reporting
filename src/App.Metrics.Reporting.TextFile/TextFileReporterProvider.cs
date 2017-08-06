// <copyright file="TextFileReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using Microsoft.Extensions.Options;

namespace App.Metrics.Reporting.TextFile
{
    public class TextFileReporterProvider : IReporterProvider
    {
        private readonly IOptions<MetricsReportingTextFileOptions> _textFileOptionsAccessor;

        public TextFileReporterProvider(
            IOptions<MetricsReportingOptions> optionsAccessor,
            IOptions<MetricsReportingTextFileOptions> textFileOptionsAccessor)
        {
            _textFileOptionsAccessor = textFileOptionsAccessor;
            Filter = optionsAccessor.Value.Filter;
            ReportInterval = textFileOptionsAccessor.Value.ReportInterval;
        }

        public IFilterMetrics Filter { get; }

        public TimeSpan ReportInterval { get; }

        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var stream = new MemoryStream())
            {
                await _textFileOptionsAccessor.Value.MetricsOutputFormatter.WriteAsync(stream, metricsData, _textFileOptionsAccessor.Value.TextFileEncoding, cancellationToken);

                var output = Encoding.UTF8.GetString(stream.ToArray());

                if (_textFileOptionsAccessor.Value.AppendMetricsToTextFile)
                {
                    File.AppendAllText(_textFileOptionsAccessor.Value.OutputPathAndFileName, output, _textFileOptionsAccessor.Value.TextFileEncoding);
                }
                else
                {
                    File.WriteAllText(_textFileOptionsAccessor.Value.OutputPathAndFileName, output, _textFileOptionsAccessor.Value.TextFileEncoding);
                }
            }

            return true;
        }
    }
}