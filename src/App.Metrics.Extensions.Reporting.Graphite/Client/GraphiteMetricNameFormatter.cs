using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using App.Metrics.Tagging;

namespace App.Metrics.Extensions.Reporting.Graphite.Client
{
    public class GraphiteMetricNameFormatter : IGraphiteMetricNameFormatter
    {
        private delegate string RenderToken(Dictionary<string, string> context);

        private readonly List<RenderToken> _tokens;

        private static RenderToken Const(string value) => _ => value;

        private static RenderToken Variable(string name)
            => context => context.TryGetValue(name, out var result) ? result : string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphiteMetricNameFormatter"/> class.
        /// default format is {type}.{context}.{nameWithUnit}
        /// </summary>
        public GraphiteMetricNameFormatter()
        {
            _tokens = new List<RenderToken>(3)
            {
                Variable("type"),
                Variable("context"),
                Variable("nameWithUnit")
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphiteMetricNameFormatter"/> class.
        /// available place holders: {type} {context} {name} {unit} {nameWithUnit} {tag:[tagkey]}
        /// </summary>
        /// <param name="format">the format of the metrics</param>
        /// <example>"prefix.{type}.{tag:host}.{context}.{tag:route}.{nameWithUnit}.{tag:http_status_code}.suffix"</example>
        public GraphiteMetricNameFormatter(string format)
        {
            _tokens = CreateTemplate(format);
        }

        /// <inheritdoc />
        public string Format(string context, MetricTags tags, GraphiteMetricName name)
        {
            var tagsDictionary = tags.Keys.Zip(tags.Values, (key, value) => new { key, value })
                .ToDictionary(x => $"tag:{x.key}", x => x.value, StringComparer.OrdinalIgnoreCase);

            var unit = GraphiteMetricName.Escape(name.Unit.Name);

            var escapedName = string.Join(".", name.Folder.Select(x => GraphiteMetricName.Escape(x)).Where(x => !string.IsNullOrEmpty(x)));

            string nameWithUnit;
            if (string.IsNullOrEmpty(escapedName))
            {
                nameWithUnit = unit;
            }
            else if (string.IsNullOrEmpty(unit) || escapedName.EndsWith(unit, StringComparison.OrdinalIgnoreCase))
            {
                nameWithUnit = escapedName;
            }
            else
            {
                nameWithUnit = $"{escapedName}-{unit}";
            }

            tagsDictionary["type"] = name.Type.ToString();
            tagsDictionary["name"] = escapedName;
            tagsDictionary["unit"] = unit;
            tagsDictionary["nameWithUnit"] = nameWithUnit;
            tagsDictionary["context"] = context;

            return Render(tagsDictionary);
        }

        private static List<RenderToken> CreateTemplate(string template)
        {
            var templateItems = template.Split('{', '}');
            var result = new List<RenderToken>(templateItems.Length);
            var isVar = template.StartsWith("{", StringComparison.Ordinal);

            foreach (var item in templateItems)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    result.Add(isVar ? Variable(item) : Const(item));
                }

                isVar = !isVar;
            }

            return result;
        }

        private string Render(Dictionary<string, string> context)
        {
            var builder = new StringBuilder();
            foreach (var token in _tokens)
            {
                builder.Append(GraphiteMetricName.Escape(token(context), true));
            }

            return Regex.Replace(builder.ToString().Trim('.'), "[.]{2,}", ".");
        }
    }
}
