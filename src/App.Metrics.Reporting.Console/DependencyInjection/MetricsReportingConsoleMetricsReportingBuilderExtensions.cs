// <copyright file="MetricsReportingConsoleMetricsReportingBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Reporting.Console;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsReportingConsoleMetricsReportingBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics console reporting metrics services to the specified <see cref="IMetricsReportingBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingBuilder" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure the App Metrics console reporting services.
        /// </returns>
        public static IMetricsReportingBuilder AddConsole(this IMetricsReportingBuilder builder)
        {
            builder.Services.AddConsoleCore();

            return builder;
        }

        /// <summary>
        ///     Adds App Metrics console reporting metrics services to the specified <see cref="IMetricsReportingBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action" /> to configure the provided <see cref="MetricsReportingConsoleOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure the App Metrics console reporting services.
        /// </returns>
        public static IMetricsReportingBuilder AddConsole(
            this IMetricsReportingBuilder builder,
            Action<MetricsReportingConsoleOptions> setupAction)
        {
            var reportingBuilder = builder.AddConsole();

            builder.Services.Configure(setupAction);

            return reportingBuilder;
        }

        /// <summary>
        ///     Adds App Metrics console reporting metrics services to the specified <see cref="IMetricsReportingCoreBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingCoreBuilder" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingCoreBuilder" /> that can be used to further configure the App Metrics console reporting services.
        /// </returns>
        public static IMetricsReportingCoreBuilder AddConsole(this IMetricsReportingCoreBuilder builder)
        {
            builder.Services.AddConsoleCore();

            return builder;
        }

        /// <summary>
        ///     Adds App Metrics console reporting metrics services to the specified <see cref="IMetricsReportingCoreBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingCoreBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action" /> to configure the provided <see cref="MetricsReportingConsoleOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsReportingCoreBuilder" /> that can be used to further configure the App Metrics console reporting services.
        /// </returns>
        public static IMetricsReportingCoreBuilder AddConsole(
            this IMetricsReportingCoreBuilder builder,
            Action<MetricsReportingConsoleOptions> setupAction)
        {
            var reportingBuilder = builder.AddConsole();

            builder.Services.Configure(setupAction);

            return reportingBuilder;
        }
    }
}