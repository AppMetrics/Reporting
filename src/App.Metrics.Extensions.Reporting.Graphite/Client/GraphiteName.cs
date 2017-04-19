using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace App.Metrics.Extensions.Reporting.Graphite.Client
{
    public struct GraphiteName
    {
        public MetricType Type { get; }
        public Unit Unit { get; }
        public IEnumerable<string> Folder { get; }

        public GraphiteName(MetricType type, Unit unit, IEnumerable<string> folder = null)
        {
            Type = type;
            Unit = unit;
            Folder = folder ?? Enumerable.Empty<string>();
        }

        public static GraphiteName From(MetricType type, Unit unit) => new GraphiteName(type, unit);
        public GraphiteName WithFolder(string folder) => new GraphiteName(Type, Unit, Folder.Append(folder));
        public GraphiteName WithFolder(params string[] folder) => WithFolder(string.Join("-", folder));
        public GraphiteName WithUnit(Unit unit) => new GraphiteName(Type, unit, Folder);
        public GraphiteName WithRate(TimeUnit rateUnit) => WithUnit($"{Unit.Name}-per-{rateUnit.Unit()}");

        private const RegexOptions REGEX_OPTIONS = RegexOptions.CultureInvariant | RegexOptions.Compiled;
        private static readonly Regex _invalid = new Regex(@"[^a-zA-Z0-9\-%&]+", REGEX_OPTIONS);
        private static readonly Regex _invalidAllowDots = new Regex(@"[^a-zA-Z0-9\-%&.]+", REGEX_OPTIONS);

        public static string Escape(string name, bool allowDots = false)
        {
            name = name ?? string.Empty;
            return (allowDots ? _invalidAllowDots : _invalid).Replace(name, "_").Trim('_');
        }
    }
}