using System.Collections.Generic;
using App.Metrics.Tagging;

namespace App.Metrics.Extensions.Reporting.Graphite.Client
{
    public interface IGraphiteNameFormatter
    {
        string Format(string context, MetricTags tags, GraphiteName name);
    }
}