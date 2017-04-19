using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace App.Metrics.Extensions.Reporting.Graphite.Client
{
    public struct GraphiteMetricName
    {
        public MetricType Type { get; }

        public Unit Unit { get; }

        public IEnumerable<string> Folder { get; }

        public GraphiteMetricName(MetricType type, Unit unit, IEnumerable<string> folder = null)
        {
            Type = type;
            Unit = unit;
            Folder = folder ?? Enumerable.Empty<string>();
        }

        public static GraphiteMetricName From(MetricType type, Unit unit) => new GraphiteMetricName(type, unit);

        public GraphiteMetricName WithFolder(string folder) => new GraphiteMetricName(Type, Unit, Folder.Append(folder));

        public GraphiteMetricName WithFolder(params string[] folder) => WithFolder(string.Join("-", folder));

        public GraphiteMetricName WithUnit(Unit unit) => new GraphiteMetricName(Type, unit, Folder);

        public GraphiteMetricName WithRate(TimeUnit rateUnit) => WithUnit($"{Unit.Name}-per-{rateUnit.Unit()}");

        private const RegexOptions RegexOptions = System.Text.RegularExpressions.RegexOptions.CultureInvariant | System.Text.RegularExpressions.RegexOptions.Compiled;
        private static readonly Regex _invalid = new Regex(@"[^a-zA-Z0-9\-%&]+", RegexOptions);
        private static readonly Regex _invalidAllowDots = new Regex(@"[^a-zA-Z0-9\-%&.]+", RegexOptions);

        public static string Escape(string name, bool allowDots = false)
        {
            name = name ?? string.Empty;
            return (allowDots ? _invalidAllowDots : _invalid).Replace(name, "_").Trim('_');
        }
    }
}
