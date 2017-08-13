// <copyright file="MetricsAspNetReportingWebHostBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.AspNetCore.Reporting;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Reporting.Internal;
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
                    EnsureRequiredMetricsServices(services);

                    var metricsReportingOptions = new MetricsReportingWebHostOptions();
                    setupAction.Invoke(metricsReportingOptions);

                    var reportingCoreBuilder = services.AddMetricsReportingCore(
                        context.Configuration.GetSection("MetricsReportingOptions"),
                        metricsReportingOptions.ReportingOptions);

                    var reportingBuilder = new MetricsReportingBuilder(reportingCoreBuilder.Services);

                    metricsReportingOptions.ReportingBuilder.Invoke(reportingBuilder);

                    reportingBuilder.AddHostedServiceScheduling(metricsReportingOptions.UnobservedTaskExceptionHandler);
                });
        }

        private static void EnsureRequiredMetricsServices(IServiceCollection services)
        {
            // Verify if AddMetrics was adding before using reporting.
            // We use the MetricsMarkerService to make sure if all the services were added.
            AppMetricsServicesHelper.ThrowIfMetricsNotRegistered(services);
        }
    }
}