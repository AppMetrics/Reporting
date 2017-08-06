// <copyright file="MetricsReportingHttpServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Reporting;
using App.Metrics.Reporting.Http;
using App.Metrics.Reporting.Http.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Extension methods for setting up essential App Metrics HTTP reporting services in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class MetricsReportingHttpServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpCore(this IServiceCollection services)
        {
            AddHttpReportingServices(services);

            return services;
        }

        internal static void AddHttpReportingServices(IServiceCollection services)
        {
            //
            // Options
            //
            var optionsSetupDescriptor = ServiceDescriptor.Transient<IConfigureOptions<MetricsReportingHttpOptions>, MetricsReportingHttpOptionsSetup>();
            services.TryAddEnumerable(optionsSetupDescriptor);

            //
            // HTTP Reporting Infrastructure
            //
            var consoleReportProviderDescriptor = ServiceDescriptor.Transient<IReporterProvider, HttpReporterProvider>();
            services.TryAddEnumerable(consoleReportProviderDescriptor);
        }
    }
}
