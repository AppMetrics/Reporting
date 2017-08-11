// <copyright file="MetricsReportingApplicationBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for <see cref="IApplicationBuilder" /> to add App Metrics Reporting.
    /// </summary>
    public static class MetricsReportingApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds App Metrics Reporting to the <see cref="ApplicationLifetime"/>.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsReporting(this IApplicationBuilder app)
        {
            // Verify if AddMetrics was done before calling UseMetricsEndpoints
            // We use the MetricsMarkerService to make sure if all the services were added.
            AppMetricsServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);

            var options = app.ApplicationServices.GetRequiredService<IOptions<MetricsReportingOptions>>();

            if (!options.Value.Enabled)
            {
                return app;
            }

            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            var reporter = app.ApplicationServices.GetRequiredService<IMetricsReporter>();

            lifetime.ApplicationStarted.Register(() => { Task.Run(() => reporter.ScheduleReports(lifetime.ApplicationStopping), lifetime.ApplicationStopping); });

            return app;
        }
    }
}