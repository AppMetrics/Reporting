// <copyright file="MetricsReportingHttpMetricsReportingBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Reporting.Http;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsReportingHttpMetricsReportingBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics HTTP reporting metrics services to the specified <see cref="IMetricsReportingBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingBuilder" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure the App Metrics HTTP reporting services.
        /// </returns>
        public static IMetricsReportingBuilder AddHttp(this IMetricsReportingBuilder builder)
        {
            builder.Services.AddHttpCore();

            return builder;
        }

        /// <summary>
        ///     Adds App Metrics HTTP reporting metrics services to the specified <see cref="IMetricsReportingBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action" /> to configure the provided <see cref="MetricsReportingHttpOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure the App Metrics HTTP reporting services.
        /// </returns>
        public static IMetricsReportingBuilder AddHttp(
            this IMetricsReportingBuilder builder,
            Action<MetricsReportingHttpOptions> setupAction)
        {
            var reportingBuilder = builder.AddHttp();

            builder.Services.Configure(setupAction);

            return reportingBuilder;
        }

        /// <summary>
        ///     Adds App Metrics HTTP reporting metrics services to the specified <see cref="IMetricsReportingCoreBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingCoreBuilder" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingCoreBuilder" /> that can be used to further configure the App Metrics HTTP reporting services.
        /// </returns>
        public static IMetricsReportingCoreBuilder AddHttp(this IMetricsReportingCoreBuilder builder)
        {
            builder.Services.AddHttpCore();

            return builder;
        }

        /// <summary>
        ///     Adds App Metrics HTTP reporting metrics services to the specified <see cref="IMetricsReportingCoreBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingCoreBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action" /> to configure the provided <see cref="MetricsReportingHttpOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsReportingCoreBuilder" /> that can be used to further configure the App Metrics HTTP reporting services.
        /// </returns>
        public static IMetricsReportingCoreBuilder AddHttp(
            this IMetricsReportingCoreBuilder builder,
            Action<MetricsReportingHttpOptions> setupAction)
        {
            var reportingBuilder = builder.AddHttp();

            builder.Services.Configure(setupAction);

            return reportingBuilder;
        }
    }
}