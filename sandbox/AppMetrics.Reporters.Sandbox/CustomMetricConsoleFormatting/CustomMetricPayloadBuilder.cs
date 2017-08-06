// <copyright file="CustomMetricPayloadBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Metrics;

namespace AppMetrics.Reporters.Sandbox.CustomMetricConsoleFormatting
{
    public class CustomMetricPayloadBuilder : IMetricPayloadBuilder<CustomMetricPayload>
    {
        private CustomMetricPayload _payload;

        public MetricValueDataKeys DataKeys { get; } = new MetricValueDataKeys();

        public CustomMetricPayloadBuilder()
        {
            _payload = new CustomMetricPayload();
        }

        /// <inheritdoc />
        public void Clear()
        {
            _payload = null;
        }

        /// <inheritdoc />
        public void Init()
        {
            _payload = new CustomMetricPayload();
        }

        /// <inheritdoc />
        public void Pack(string context, string name, object value, MetricTags tags)
        {
            _payload.Add(new CustomMetricPoint($"{context}.{name}", new Dictionary<string, object> { { "value", value } }, tags));
        }

        /// <inheritdoc />
        public void Pack(string context, string name, IEnumerable<string> columns, IEnumerable<object> values, MetricTags tags)
        {
            var fields = columns.Zip(values, (column, data) => new { column, data }).ToDictionary(pair => pair.column, pair => pair.data);

            _payload.Add(new CustomMetricPoint($"{context}.{name}", fields, tags));
        }

        /// <inheritdoc />
        public CustomMetricPayload Payload()
        {
            return _payload;
        }

        /// <inheritdoc />
        public string PayloadFormatted()
        {
            var result = new StringWriter();
            _payload.Format(result);
            return result.ToString();
        }
    }
}