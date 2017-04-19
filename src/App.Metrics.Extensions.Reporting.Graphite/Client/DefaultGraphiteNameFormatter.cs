using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using App.Metrics.Tagging;

namespace App.Metrics.Extensions.Reporting.Graphite.Client
{
    public class DefaultGraphiteNameFormatter : IGraphiteNameFormatter
    {
        private delegate string RenderToken(Dictionary<string, string> context);

        private readonly List<RenderToken> _tokens;

        private static RenderToken Const(string value) => _=>value;

        private static RenderToken Variable(string name)
            => context => context.TryGetValue(name, out var result) ? result : string.Empty;

        public DefaultGraphiteNameFormatter()
        {
            _tokens = new List<RenderToken>(3)
            {
                Variable("type"),
                Variable("context"),
                Variable("nameWithUnit")
            };
        }

        public DefaultGraphiteNameFormatter(string format)
        {
            _tokens = CreateTemplate(format);
        }

        public string Format(string context, MetricTags tags, GraphiteName name)
        {
            var tagsDictionary = tags.Keys.Zip(tags.Values, (key, value) => new {key, value})
                .ToDictionary(x => $"tag:{x.key}", x => x.value, StringComparer.OrdinalIgnoreCase);

            var unit = GraphiteName.Escape(name.Unit.Name);

            var escapedName = string.Join(".",
                name.Folder.Select(x => GraphiteName.Escape(x))
                    .Where(x => !string.IsNullOrEmpty(x)));

            string nameWithUnit;
            if (string.IsNullOrEmpty(escapedName)) nameWithUnit = unit;
            else if (string.IsNullOrEmpty(unit) || escapedName.EndsWith(unit, StringComparison.OrdinalIgnoreCase)) nameWithUnit = escapedName;
            else nameWithUnit = $"{escapedName}-{unit}";

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
                builder.Append(GraphiteName.Escape(token(context), true));
            }
            return Regex.Replace(builder.ToString().Trim('.'), "[.]{2,}", ".");
        }
    }
}