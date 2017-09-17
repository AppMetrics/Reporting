// <copyright file="TextFileMetricsReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;

namespace App.Metrics.Reporting.TextFile
{
    public class TextFileMetricsReporter : IReportMetrics
    {
        private static readonly int _defaultBufferSize = 4096;
        private readonly IMetricsOutputFormatter _defaultMetricsOutputFormatter = new MetricsTextOutputFormatter();
        private readonly bool _appendMode;
        private readonly string _output = @".\metrics.txt";

        // ReSharper disable UnusedMember.Global
        public TextFileMetricsReporter()
            // ReSharper restore UnusedMember.Global
        {
        }

        public TextFileMetricsReporter(MetricsReportingTextFileOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.MetricsOutputFormatter != null)
            {
                Formatter = options.MetricsOutputFormatter;
            }

            if (options.FlushInterval > TimeSpan.Zero)
            {
                FlushInterval = options.FlushInterval;
            }

            _appendMode = options.AppendMetricsToTextFile;
            _output = options.OutputPathAndFileName;
            Filter = options.Filter;

            var fileInfo = new FileInfo(options.OutputPathAndFileName);

            if (!fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }
        }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; set; }

        /// <inheritdoc />
        public TimeSpan FlushInterval { get; set; }

        /// <inheritdoc />
        public IMetricsOutputFormatter Formatter { get; set; }

        /// <inheritdoc />
        public async Task<bool> FlushAsync(MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = Formatter ?? _defaultMetricsOutputFormatter;

                await formatter.WriteAsync(stream, metricsData, cancellationToken);

                var outputStream = stream.ToArray();

                if (_appendMode)
                {
                    using (var sourceStream = new FileStream(
                        _output,
                        FileMode.Append,
                        FileAccess.Write,
                        FileShare.None,
                        bufferSize: _defaultBufferSize,
                        useAsync: true))
                    {
                        sourceStream.Seek(0, SeekOrigin.End);
                        await sourceStream.WriteAsync(outputStream, 0, outputStream.Length, cancellationToken);
                    }
                }
                else
                {
                    using (var sourceStream = new FileStream(
                        _output,
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