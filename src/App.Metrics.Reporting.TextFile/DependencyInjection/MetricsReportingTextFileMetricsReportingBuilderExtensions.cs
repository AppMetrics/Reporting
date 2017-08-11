// <copyright file="MetricsReportingTextFileMetricsReportingBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Reporting.TextFile;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsReportingTextFileMetricsReportingBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics text file reporting metrics services to the specified <see cref="IMetricsReportingBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingBuilder" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure the App Metrics text file reporting services.
        /// </returns>
        public static IMetricsReportingBuilder AddTextFile(this IMetricsReportingBuilder builder)
        {
            builder.Services.AddTextFileCore();

            return builder;
        }

        /// <summary>
        ///     Adds App Metrics text file reporting metrics services to the specified <see cref="IMetricsReportingBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action" /> to configure the provided <see cref="MetricsReportingTextFileOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsReportingBuilder" /> that can be used to further configure the App Metrics text file reporting services.
        /// </returns>
        public static IMetricsReportingBuilder AddTextFile(
            this IMetricsReportingBuilder builder,
            Action<MetricsReportingTextFileOptions> setupAction)
        {
            var reportingBuilder = builder.AddTextFile();

            builder.Services.Configure(setupAction);

            return reportingBuilder;
        }

        /// <summary>
        ///     Adds App Metrics text file reporting metrics services to the specified <see cref="IMetricsReportingCoreBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingCoreBuilder" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingCoreBuilder" /> that can be used to further configure the App Metrics text file reporting services.
        /// </returns>
        public static IMetricsReportingCoreBuilder AddTextFile(this IMetricsReportingCoreBuilder builder)
        {
            builder.Services.AddTextFileCore();

            return builder;
        }

        /// <summary>
        ///     Adds App Metrics text file reporting metrics services to the specified <see cref="IMetricsReportingCoreBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingCoreBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action" /> to configure the provided <see cref="MetricsReportingTextFileOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsReportingCoreBuilder" /> that can be used to further configure the App Metrics text file reporting services.
        /// </returns>
        public static IMetricsReportingCoreBuilder AddTextFile(
            this IMetricsReportingCoreBuilder builder,
            Action<MetricsReportingTextFileOptions> setupAction)
        {
            var reportingBuilder = builder.AddTextFile();

            builder.Services.Configure(setupAction);

            return reportingBuilder;
        }
    }
}