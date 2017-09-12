// <copyright file="MetricsReportingOptionsBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Builder;
using App.Metrics.Reporting.Internal;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring the <see cref="MetricsReportingOptions"/>.
    /// </summary>
    public class MetricsReportingOptionsBuilder
    {
        private readonly IMetricsReportingBuilder _metricsReportingBuilder;
        private readonly Action<MetricsReportingOptions> _setupAction;

        internal MetricsReportingOptionsBuilder(
            IMetricsReportingBuilder metricsReportingBuilder,
            Action<MetricsReportingOptions> options)
        {
            _metricsReportingBuilder = metricsReportingBuilder ?? throw new ArgumentNullException(nameof(metricsReportingBuilder));
            _setupAction = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="MetricsReportingOptions"/> instance for App Metrics Reporting configuration.
        ///     </para>
        /// </summary>
        /// <param name="options">An <see cref="MetricsReportingOptions"/> instance used to configure core App Metrics options.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure App Metrics Reporting.
        /// </returns>
        public IMetricsReportingBuilder Configure(MetricsReportingOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _setupAction(options);

            return _metricsReportingBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="MetricsReportingOptions"/> instance for App Metrics Reporting configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="MetricsReportingOptions"/>s property names.
        ///     </para>
        /// </summary>
        /// <param name="optionValues">Key value pairs for configuring App Metrics Reporting.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure App Metrics Reporting.
        /// </returns>
        public IMetricsReportingBuilder Configure(IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            var options = new KeyValuePairMetricsOptions(optionValues).AsOptions();

            _setupAction(options);

            return _metricsReportingBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="MetricsReportingOptions"/> instance for App Metrics Reporting configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="MetricsReportingOptions"/>s property names. Any make key will override the <see cref="MetricsReportingOptions"/> value configured.
        ///     </para>
        /// </summary>
        /// <param name="options">An <see cref="MetricsOptions"/> instance used to configure App Metrics Reporting options.</param>
        /// <param name="optionValues">Key value pairs for configuring App Metrics Reporting.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure App Metrics Reporting.
        /// </returns>
        public IMetricsReportingBuilder Configure(MetricsReportingOptions options, IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            _setupAction(new KeyValuePairMetricsOptions(options, optionValues).AsOptions());

            return _metricsReportingBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="MetricsReportingOptions"/> instance for App Metrics Reporting configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="MetricsReportingOptions"/>s property names. Any make key will override the <see cref="MetricsReportingOptions"/> value configured.
        ///     </para>
        /// </summary>
        /// <param name="setupAction">An <see cref="MetricsReportingOptions"/> setup action used to configure core App Metrics Reporting options.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure App Metrics Reporting.
        /// </returns>
        public IMetricsReportingBuilder Configure(Action<MetricsReportingOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            var options = new MetricsReportingOptions();

            setupAction(options);

            _setupAction(options);

            return _metricsReportingBuilder;
        }
    }
}
