using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Metrics.Tagging;

namespace App.Metrics.Extensions.Reporting.Graphite.Client
{
    public class DefaultGraphiteNameFormatter : IGraphiteNameFormatter
    {
        private abstract class Token
        {
            protected readonly string Key;

            protected Token(string key)
            {
                Key = key;
            }

            public abstract string GetValue(Dictionary<string, string> context);
        }

        private class ConstToken : Token
        {
            public ConstToken(string key) : base(key) { }
            public override string GetValue(Dictionary<string, string> context) => Key;
        }

        private class VarToken : Token
        {
            public VarToken(string key) : base(key) { }
            public override string GetValue(Dictionary<string, string> context) => context.TryGetValue(Key, out var value) ? value : string.Empty;
        }

        private readonly List<Token> _tokens;

        public DefaultGraphiteNameFormatter()
        {
            _tokens = new List<Token>(3)
            {
                new VarToken("type"),
                new VarToken("context"),
                new VarToken("nameWithUnit")
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
                name.Folder.Select(x => x.Split('|').First())
                    .Select(x => GraphiteName.Escape(x))
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

            return BuildTemplate(tagsDictionary);
        }

        private static List<Token> CreateTemplate(string template)
        {
            var templateItems = template.Split('{', '}');
            var result = new List<Token>(templateItems.Length);
            var isVar = template.StartsWith("{", StringComparison.Ordinal);
            
            foreach (var item in templateItems)
            {
                result.Add(isVar ? (Token)new VarToken(item) : new ConstToken(item));
                isVar = !isVar;
            }

            return result;
        }

        private string BuildTemplate(Dictionary<string, string> context)
        {
            var builder = new StringBuilder();
            foreach (var token in _tokens)
            {
                builder.Append(token.GetValue(context));
            }
            return builder.ToString().Trim('.');
        }
    }
}