using App.Metrics.Tagging;

namespace App.Metrics.Extensions.Reporting.Graphite.Client
{
    public interface IGraphiteMetricNameFormatter
    {
        /// <summary>
        /// Formats the metric name to be sent to the server
        /// </summary>
        /// <param name="context">context of metric</param>
        /// <param name="tags">tags of metric</param>
        /// <param name="name">graphite metric name</param>
        /// <returns>formatted name that will be sent to graphite</returns>
        string Format(string context, MetricTags tags, GraphiteMetricName name);
    }
}
