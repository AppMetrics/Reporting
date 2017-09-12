// <copyright file="KeyValuePairMetricsOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Reporting.Internal
{
    internal class KeyValuePairMetricsOptions
    {
        private const string MetricsReportingEnabledDirective = nameof(MetricsReportingOptions.Enabled);
        private readonly MetricsReportingOptions _options;

        private readonly Dictionary<string, string> _optionValues;

        public KeyValuePairMetricsOptions(MetricsReportingOptions options, IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            _options = options ?? throw new ArgumentNullException(nameof(options));

            _optionValues = optionValues.ToDictionary(o => o.Key, o => o.Value);
        }

        public KeyValuePairMetricsOptions(IEnumerable<KeyValuePair<string, string>> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _optionValues = options.ToDictionary(o => o.Key, o => o.Value);
        }

        public MetricsReportingOptions AsOptions()
        {
            var options = _options ?? new MetricsReportingOptions();

            foreach (var key in _optionValues.Keys)
            {
               if (string.Compare(key, MetricsReportingEnabledDirective, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    if (!bool.TryParse(_optionValues[key], out var metricsEnabled))
                    {
                        throw new InvalidCastException($"Attempted to bind {key} to {MetricsReportingEnabledDirective} but it's not a boolean");
                    }

                    options.Enabled = metricsEnabled;
                }
            }

            return options;
        }
    }
}