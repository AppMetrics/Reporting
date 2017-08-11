// <copyright file="TextFileReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
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
            Filter = textFileOptionsAccessor.Value.Filter ?? optionsAccessor.Value.Filter;
            ReportInterval = textFileOptionsAccessor.Value.ReportInterval;

            var fileInfo = new FileInfo(_textFileOptionsAccessor.Value.OutputPathAndFileName);

            if (!fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }
        }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; }

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; }

        /// <inheritdoc />
        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var stream = new MemoryStream())
            {
                await _textFileOptionsAccessor.Value.MetricsOutputFormatter.WriteAsync(stream, metricsData, cancellationToken);
                var outputStream = stream.ToArray();

                if (_textFileOptionsAccessor.Value.AppendMetricsToTextFile)
                {
                    using (var sourceStream = new FileStream(
                        _textFileOptionsAccessor.Value.OutputPathAndFileName,
                        FileMode.Append,
                        FileAccess.Write,
                        FileShare.None,
                        bufferSize: 4096,
                        useAsync: true))
                    {
                        sourceStream.Seek(0, SeekOrigin.End);
                        await sourceStream.WriteAsync(outputStream, 0, outputStream.Length, cancellationToken);
                    }
                }
                else
                {
                    using (var sourceStream = new FileStream(
                        _textFileOptionsAccessor.Value.OutputPathAndFileName,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None,
                        bufferSize: 4096,
                        useAsync: true))
                    {
                        sourceStream.Seek(0, SeekOrigin.End);
                        await sourceStream.WriteAsync(outputStream, 0, outputStream.Length, cancellationToken);
                    }
                }
            }

            return true;
        }
    }
}