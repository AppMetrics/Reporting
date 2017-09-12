// <copyright file="TextFileMetricsReporterProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;

namespace App.Metrics.Reporting.TextFile
{
    public class TextFileMetricsReporterProvider : IMetricsReporterProvider
    {
        private readonly MetricsReportingTextFileOptions _textFileOptions;

        public TextFileMetricsReporterProvider(
            MetricsReportingTextFileOptions textFileOptions)
        {
            _textFileOptions = textFileOptions;
            Filter = textFileOptions.Filter;
            ReportInterval = textFileOptions.ReportInterval;

            var fileInfo = new FileInfo(_textFileOptions.OutputPathAndFileName);

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
        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            using (var stream = new MemoryStream())
            {
                await _textFileOptions.MetricsOutputFormatter.WriteAsync(stream, metricsData, cancellationToken);
                var outputStream = stream.ToArray();

                if (_textFileOptions.AppendMetricsToTextFile)
                {
                    using (var sourceStream = new FileStream(
                        _textFileOptions.OutputPathAndFileName,
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
                        _textFileOptions.OutputPathAndFileName,
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