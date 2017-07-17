// <copyright file="AppMetricsReportingAppMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using App.Metrics;
using App.Metrics.Core.Internal.NoOp;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsReportingAppMetricsBuilderExtensions
    {
        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IServiceCollection AddMetricsReporting(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<IReportFactory> setupAction)
        {
            services.Configure<AppMetricsReportingOptions>(configuration);

            return services.AddMetricsReporting(setupAction);
        }

        [ExcludeFromCodeCoverage] // DEVNOTE: No need to test Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions
        public static IServiceCollection AddMetricsReporting(
            this IServiceCollection services,
            Action<AppMetricsReportingOptions> setupOptionsAction,
            Action<IReportFactory> setupAction)
        {
            services.Configure(setupOptionsAction);

            return services.AddMetricsReporting(setupAction);
        }

        public static IServiceCollection AddMetricsReporting(this IServiceCollection services, Action<IReportFactory> setupAction)
        {
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppMetricsReportingOptions>>().Value);

            services.AddSingleton<IReportFactory>(
                provider =>
                {
                    var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                    var metrics = provider.GetRequiredService<IMetrics>();
                    var options = provider.GetRequiredService<AppMetricsReportingOptions>();

                    if (!options.ReportingEnabled || setupAction == null)
                    {
                        return new NoOpReportFactory();
                    }

                    var factory = new ReportFactory(metrics, loggerFactory);
                    setupAction.Invoke(factory);
                    return factory;
                });

            return services;
        }
    }
}
