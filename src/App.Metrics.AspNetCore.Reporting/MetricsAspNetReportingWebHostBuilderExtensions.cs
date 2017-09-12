// <copyright file="MetricsAspNetReportingWebHostBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.AspNetCore.Reporting;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Builder;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics Reporting AspNet Core services in an <see cref="IWebHostBuilder" />.
    /// </summary>
    public static class MetricsAspNetReportingWebHostBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics Reproting services and configuration the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsReportingWebHostOptions}" /> to configure the provided
        ///     <see cref="MetricsReportingWebHostOptions" />.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseMetricsReporting(
            this IWebHostBuilder hostBuilder,
            Action<MetricsReportingWebHostOptions> setupAction)
        {
            ConfigureMetricsReportingServices(hostBuilder, setupAction);

            return hostBuilder;
        }

        /// <summary>
        ///     Adds App Metrics Reproting services and configuration the
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{WebHostBuilderContext, MetricsReportingWebHostOptions}" /> to configure the provided
        ///     <see cref="MetricsReportingWebHostOptions" /> and provides the <see cref="WebHostBuilderContext" />.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> cannot be null
        /// </exception>
        public static IWebHostBuilder UseMetricsReporting(
            this IWebHostBuilder hostBuilder,
            Action<WebHostBuilderContext, MetricsReportingWebHostOptions> setupAction)
        {
            ConfigureMetricsReportingServices(hostBuilder, setupAction);

            return hostBuilder;
        }

        private static void AddMetricsReportingCoreServices(
            IServiceCollection services,
            WebHostBuilderContext context,
            MetricsReportingWebHostOptions metricsReportingOptions)
        {
            // TODO: Add options from config
            // var reportingCoreBuilder = services.AddMetricsReportingCore(
            //     context.Configuration.GetSection(nameof(MetricsReportingOptions)),
            //     metricsReportingOptions.ReportingOptions);

            var reportingBuilder = new MetricsReportingBuilder(null);

            metricsReportingOptions.ReportingBuilder.Invoke(reportingBuilder);

            reportingBuilder.AddHostedServiceScheduling(services, metricsReportingOptions.UnobservedTaskExceptionHandler);
        }

        private static void ConfigureMetricsReportingServices(
            IWebHostBuilder hostBuilder,
            Action<MetricsReportingWebHostOptions> setupAction)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var metricsReportingOptions = new MetricsReportingWebHostOptions();
                    setupAction.Invoke(metricsReportingOptions);

                    AddMetricsReportingCoreServices(services, context, metricsReportingOptions);
                });
        }

        private static void ConfigureMetricsReportingServices(
            IWebHostBuilder hostBuilder,
            Action<WebHostBuilderContext, MetricsReportingWebHostOptions> setupAction)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var metricsReportingOptions = new MetricsReportingWebHostOptions();
                    setupAction.Invoke(context, metricsReportingOptions);

                    AddMetricsReportingCoreServices(services, context, metricsReportingOptions);
                });
        }
    }
}