// <copyright file="MetricsAspNetCoreMetricsReportingCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.AspNetCore.Reporting.Internal.Infrastructure;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics Reporting services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class MetricsAspNetCoreMetricsReportingCoreBuilderExtensions
    {
        /// <summary>
        ///     Adds reporter schduling using an <see cref="IHostedService"/> to the specified <see cref="IMetricsReportingBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingBuilder" /> to add services to.</param>
        /// <param name="services">THe <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>An <see cref="IMetricsReportingBuilder"/> that can be used to further configure the App Metrics Reporting services.</returns>
        public static IMetricsReportingBuilder AddHostedServiceSchedulingCore(
            this IMetricsReportingBuilder builder,
            IServiceCollection services)
        {
            AddReportScheduling(services);

            return builder;
        }

        /// <summary>
        ///     Adds reporter schduling using an <see cref="IHostedService"/> to the specified <see cref="IMetricsReportingBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingBuilder" /> to add services to.</param>
        /// /// <param name="services">THe <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="unobservedTaskExceptionHandler"><see cref="EventHandler"/> registered with an exception is thrown in one of the registered reproter providers.</param>
        /// <returns>An <see cref="IMetricsReportingBuilder"/> that can be used to further configure the App Metrics Reporting services.</returns>
        public static IMetricsReportingBuilder AddHostedServiceSchedulingCore(
            this IMetricsReportingBuilder builder,
            IServiceCollection services,
            EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler)
        {
            AddReportScheduling(services, unobservedTaskExceptionHandler);

            return builder;
        }

        private static void AddReportScheduling(IServiceCollection services, EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler = null)
        {
            if (unobservedTaskExceptionHandler == null)
            {
                services.AddSingleton<IHostedService, ReportSchedulerHostedService>();
                return;
            }

            services.AddSingleton<IHostedService, ReportSchedulerHostedService>(serviceProvider =>
            {
                var metrics = serviceProvider.GetRequiredService<IMetrics>();
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var reporterProviders = serviceProvider.GetRequiredService<IEnumerable<IMetricsReporterProvider>>();

                var instance = new ReportSchedulerHostedService(loggerFactory.CreateLogger<ReportSchedulerHostedService>(), metrics, reporterProviders);

                instance.UnobservedTaskException += unobservedTaskExceptionHandler;

                return instance;
            });
        }
    }
}
