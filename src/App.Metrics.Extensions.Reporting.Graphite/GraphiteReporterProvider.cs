using System;
using App.Metrics.Abstractions.Filtering;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Extensions.Reporting.Graphite.Client;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.Graphite
{
    public class GraphiteReporterProvider : IReporterProvider
    {
        private readonly GraphiteReporterSettings _settings;

        public GraphiteReporterProvider(GraphiteReporterSettings settings, IFilterMetrics filter)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;
            Filter = filter;
        }

        /// <inheritdoc />
        public IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory)
        {
            GraphiteSender sender;
            switch (_settings.ConnectionType)
            {
                case ConnectionType.Udp:
                    sender = new UdpGraphiteSender(loggerFactory, _settings.Host, _settings.Port);
                    break;
                case ConnectionType.Tcp:
                    sender = new TcpGraphiteSender(loggerFactory, _settings.Host, _settings.Port);
                    break;

                default:
                    throw new InvalidOperationException("Unknown ConnectionType");
            }

            return new GraphiteReporter(sender, _settings.NameFormatter, name, _settings.ReportInterval, loggerFactory);
        }

        /// <inheritdoc />
        public IFilterMetrics Filter { get; }
    }
}