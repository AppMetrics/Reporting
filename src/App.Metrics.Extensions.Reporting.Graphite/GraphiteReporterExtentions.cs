using App.Metrics.Abstractions.Filtering;
using App.Metrics.Extensions.Reporting.Graphite.Client;
using App.Metrics.Reporting.Abstractions;

namespace App.Metrics.Extensions.Reporting.Graphite
{
    public static class GraphiteReporterExtentions
    {
        public static IReportFactory AddGraphite(
            this IReportFactory factory,
            GraphiteReporterSettings settings,
            IFilterMetrics filter = null)
        {
            factory.AddProvider(new GraphiteReporterProvider(settings, filter));
            return factory;
        }

        public static IReportFactory AddGraphite(
            this IReportFactory factory,
            string host,
            int port,
            ConnectionType connectionType = ConnectionType.Tcp,
            string metricTemplate = null,
            IFilterMetrics filter = null)
        {
            var settings = new GraphiteReporterSettings
            {
                Host = host,
                Port = port,
                ConnectionType = connectionType,
                NameFormatter = string.IsNullOrEmpty(metricTemplate) ? 
                    new DefaultGraphiteNameFormatter(): new DefaultGraphiteNameFormatter(metricTemplate)
            };

            factory.AddGraphite(settings, filter);
            return factory;
        }
    }
}