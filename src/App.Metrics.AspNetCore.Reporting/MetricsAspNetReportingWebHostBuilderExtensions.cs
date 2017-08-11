// <copyright file="MetricsAspNetReportingWebHostBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Reporting.Internal;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
    // ReSharper restore CheckNamespace
{
    public static class MetricsAspNetReportingWebHostBuilderExtensions
    {
        /// <summary>
        ///     Runs the configured App Metrics Reporting options once the application has started.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="setupAction">Allows configuration of reporters via the <see cref="IMetricsReportingBuilder"/></param>
        /// <returns>
        ///     A reference to this instance after the operation has completed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />
        /// </exception>
        public static IWebHostBuilder UseMetricsReporting(
            this IWebHostBuilder hostBuilder,
            Action<IMetricsReportingBuilder> setupAction)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            ConfigureCoreServices(hostBuilder, setupAction);

            return hostBuilder;
        }

        /// <summary>
        ///     Runs the configured App Metrics Reporting options once the application has started.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />.</param>
        /// <param name="setupAction">Allows configuration of reporters via the <see cref="IMetricsReportingBuilder"/></param>
        /// <param name="unobservedTaskExceptionHandler"><see cref="EventHandler"/> registered with an exception is thrown in one of the registered reproter providers.</param>
        /// <returns>
        ///     A reference to this instance after the operation has completed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" />
        /// </exception>
        public static IWebHostBuilder UseMetricsReporting(
            this IWebHostBuilder hostBuilder,
            Action<IMetricsReportingBuilder> setupAction,
            EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            ConfigureCoreServices(hostBuilder, setupAction, unobservedTaskExceptionHandler);

            return hostBuilder;
        }

        private static void ConfigureCoreServices(IWebHostBuilder hostBuilder, Action<IMetricsReportingBuilder> setupAction, EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler = null)
        {
            hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddMetricsReportingCore(context.Configuration.GetSection("MetricsReportingOptions"));

                    var builder = new MetricsReportingBuilder(services);

                    setupAction.Invoke(builder);

                    builder.AddHostedServiceScheduling(unobservedTaskExceptionHandler);
                });
        }
    }
}