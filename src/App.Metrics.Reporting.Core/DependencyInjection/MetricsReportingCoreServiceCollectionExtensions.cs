// <copyright file="MetricsReportingCoreServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Reporting;
using App.Metrics.Reporting.DependencyInjection.Internal;
using App.Metrics.Reporting.Internal;
using App.Metrics.Reporting.Internal.NoOp;
using App.Metrics.Scheduling;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics Reporting essential services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class MetricsReportingCoreServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds essential App Metrics Reporting services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingCoreBuilder" /> that can be used to further configure the App Metrics Reporting services.
        /// </returns>
        public static IMetricsReportingCoreBuilder AddMetricsReportingCore(this IServiceCollection services)
        {
            AddMetricsReportingCoreServices(services);

            return new MetricsReportingCoreBuilder(services);
        }

        /// <summary>
        ///     Adds essential App Metrics Reporting services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsReportingOptions" />.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingCoreBuilder" /> that can be used to further configure the App Metrics Reporting services.
        /// </returns>
        public static IMetricsReportingCoreBuilder AddMetricsReportingCore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var coreBuilder = services.AddMetricsReportingCore();

            services.Configure<MetricsReportingOptions>(configuration);

            return coreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics Reporting services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsReportingOptions" />.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsReportingOptions}" /> to configure the provided <see cref="MetricsReportingOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsReportingCoreBuilder" /> that can be used to further configure the App Metrics Reporting services.
        /// </returns>
        public static IMetricsReportingCoreBuilder AddMetricsReportingCore(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<MetricsReportingOptions> setupAction)
        {
            var coreBuilder = services.AddMetricsReportingCore();

            services.Configure<MetricsReportingOptions>(configuration);
            services.Configure(setupAction);

            return coreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics Reporting services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsReportingOptions}" /> to configure the provided <see cref="MetricsReportingOptions" />.
        /// </param>
        /// <param name="configuration">The <see cref="IConfiguration" /> from where to load <see cref="MetricsReportingOptions" />.</param>
        /// <returns>
        ///     An <see cref="IMetricsReportingCoreBuilder" /> that can be used to further configure the App Metrics Reporting services.
        /// </returns>
        public static IMetricsReportingCoreBuilder AddMetricsReportingCore(
            this IServiceCollection services,
            Action<MetricsReportingOptions> setupAction,
            IConfiguration configuration)
        {
            var coreBuilder = services.AddMetricsReportingCore();

            services.Configure(setupAction);
            services.Configure<MetricsReportingOptions>(configuration);

            return coreBuilder;
        }

        /// <summary>
        ///     Adds essential App Metrics Reporting services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        ///     An <see cref="Action{MetricsReportingOptions}" /> to configure the provided <see cref="MetricsReportingOptions" />.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsReportingCoreBuilder" /> that can be used to further configure the App Metrics Reporting services.
        /// </returns>
        public static IMetricsReportingCoreBuilder AddMetricsReportingCore(
            this IServiceCollection services,
            Action<MetricsReportingOptions> setupAction)
        {
            var coreBuilder = services.AddMetricsReportingCore();

            services.Configure(setupAction);

            return coreBuilder;
        }

        internal static void AddMetricsReportingCoreServices(IServiceCollection services)
        {
            //
            // Options
            //
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MetricsReportingOptions>, MetricsReportingMetricsReportingCoreOptionsSetup>());

            //
            // Reporting Infrastructure
            //
            services.AddSingleton<IScheduler, DefaultTaskScheduler>();
            services.AddSingleton<IMetricsReporter>(
                serviceProvider =>
                {
                    AppMetricsServicesHelper.ThrowIfMetricsNotRegistered(serviceProvider);

                    var optionsAccessor = serviceProvider.GetRequiredService<IOptions<MetricsReportingOptions>>();
                    var scheduler = serviceProvider.GetRequiredService<IScheduler>();
                    var metrics = serviceProvider.GetRequiredService<IMetrics>();
                    var reporterProviders = serviceProvider.GetService<IEnumerable<IReporterProvider>>();

                    if (!optionsAccessor.Value.Enabled || reporterProviders == null || !reporterProviders.Any())
                    {
                        return new NoOpMetricsReporter();
                    }

                    return new DefaultMetricsReporter(reporterProviders, metrics, scheduler);
                });

            //
            // Random Infrastructure
            //
            services.TryAddSingleton<MetricsReportingMarkerService, MetricsReportingMarkerService>();
        }
    }
}