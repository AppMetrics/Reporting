using System;
using App.Metrics.Extensions.Reporting.Graphite.Client;

namespace App.Metrics.Extensions.Reporting.Graphite.Extentions
{
    internal static class GraphiteSenderExtentions
    {
        public static void Send(this GraphiteSender graphiteSender, string name, string value)
        {
            graphiteSender.Send(DateTime.UtcNow, name, value);
        }
    }
}