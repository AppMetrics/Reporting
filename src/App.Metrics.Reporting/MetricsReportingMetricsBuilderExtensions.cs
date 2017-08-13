// <copyright file="MetricsReportingMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Internal;
using Microsoft.Extensions.Configuration;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics Reporting services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class MetricsReportingMetricsBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Reporting services to the specified <see cref="IMetricsBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <returns>An <see cref="IMetricsReportingBuilder"/> that can be used to further configure the App Metrics Reporting services.</returns>
        public static IMetricsReportingBuilder AddMetricsReporting(this IMetricsBuilder builder)
        {
            builder.Services.AddMetricsReportingCore();

            return new MetricsReportingBuilder(builder.Services);
        }

        /// <summary>
        ///     Adds App Metrics Reporting services to the specified <see cref="IMetricsBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsReportingOptions" />.</param>
        /// <returns>An <see cref="IMetricsReportingBuilder"/> that can be used to further configure the App Metrics Reporting services.</returns>
        public static IMetricsReportingBuilder AddMetricsReporting(
            this IMetricsBuilder builder,
            IConfiguration configuration)
        {
            var reportingBuilder = builder.AddMetricsReporting();

            builder.Services.Configure<MetricsReportingOptions>(configuration);

            return reportingBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Reporting services to the specified <see cref="IMetricsBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsReportingOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsReportingOptions}" /> to configure the provided <see cref="MetricsReportingOptions" />.
        /// </param>
        /// <returns>An <see cref="IMetricsReportingBuilder"/> that can be used to further configure the App Metrics Reporting services.</returns>
        public static IMetricsReportingBuilder AddMetricsReporting(
            this IMetricsBuilder builder,
            IConfiguration configuration,
            Action<MetricsReportingOptions> setupAction)
        {
            var reportingBuilder = builder.AddMetricsReporting();

            builder.Services.Configure<MetricsReportingOptions>(configuration);
            builder.Services.Configure(setupAction);

            return reportingBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Reporting services to the specified <see cref="IMetricsBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsReportingOptions}" /> to configure the provided <see cref="MetricsReportingOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsReportingOptions" />.</param>
        /// <returns>An <see cref="IMetricsReportingBuilder"/> that can be used to further configure the App Metrics Reporting services.</returns>
        public static IMetricsReportingBuilder AddMetricsReporting(
            this IMetricsBuilder builder,
            Action<IMetricsReportingBuilder> setupAction,
            IConfiguration configuration)
        {
            var reportingBuilder = builder.AddMetricsReporting();

            builder.Services.Configure(setupAction);
            builder.Services.Configure<MetricsReportingOptions>(configuration);

            return reportingBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Reporting services to the specified <see cref="IMetricsBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsReportingOptions}" /> to configure the provided <see cref="MetricsReportingOptions" />.
        /// </param>
        /// <returns>An <see cref="MetricsReportingOptions"/> that can be used to further configure the App Metrics Reporting services.</returns>
        public static IMetricsReportingBuilder AddMetricsReporting(
            this IMetricsBuilder builder,
            Action<MetricsReportingOptions> setupAction)
        {
            var reportingBuilder = builder.AddMetricsReporting();

            builder.Services.Configure(setupAction);

            return reportingBuilder;
        }
    }
}