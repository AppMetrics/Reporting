using System;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Extensions.Reporting.Graphite.Client;

namespace App.Metrics.Extensions.Reporting.Graphite
{
    public class GraphiteReporterSettings : IReporterSettings
    {
        /// <inheritdoc />
        public TimeSpan ReportInterval { get; set; } = TimeSpan.FromSeconds(10);

        public string Host { get; set; }

        public int Port { get; set; }

        public ConnectionType ConnectionType { get; set; }

        public IGraphiteMetricNameFormatter MetricNameFormatter { get; set; } = new GraphiteMetricNameFormatter();
    }

    public enum ConnectionType
    {
        /// <summary>
        /// Tcp (Recommended)
        /// </summary>
        Tcp,

       /// <summary>
       /// Udp
       /// </summary>
       ///
        Udp
    }
}
