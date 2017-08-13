// <copyright file="MetricsAspNetCoreMetricsReportingBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Reporting.Internal;
using Microsoft.Extensions.Hosting;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics Reporting services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class MetricsAspNetCoreMetricsReportingBuilderExtensions
    {
        /// <summary>
        ///     Adds reporter schduling using an <see cref="IHostedService"/> to the specified <see cref="IMetricsReportingBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingBuilder" /> to add services to.</param>
        /// <returns>An <see cref="IMetricsReportingBuilder"/> that can be used to further configure the App Metrics Reporting services.</returns>
        public static IMetricsReportingBuilder AddHostedServiceScheduling(this IMetricsReportingBuilder builder)
        {
            var coreBuilder = new MetricsReportingCoreBuilder(builder.Services);

            coreBuilder.AddHostedServiceSchedulingCore();

            return builder;
        }

        /// <summary>
        ///     Adds reporter schduling using an <see cref="IHostedService"/> to the specified <see cref="IMetricsReportingBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsReportingBuilder" /> to add services to.</param>
        /// <param name="unobservedTaskExceptionHandler"><see cref="EventHandler"/> registered with an exception is thrown in one of the registered reproter providers.</param>
        /// <returns>An <see cref="IMetricsReportingBuilder"/> that can be used to further configure the App Metrics Reporting services.</returns>
        public static IMetricsReportingBuilder AddHostedServiceScheduling(this IMetricsReportingBuilder builder, EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler)
        {
            var coreBuilder = new MetricsReportingCoreBuilder(builder.Services);

            coreBuilder.AddHostedServiceSchedulingCore();

            return builder;
        }
    }
}
