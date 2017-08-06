// <copyright file="MetricsReportingServicesHelper.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Reporting.DependencyInjection.Internal
{
    public static class MetricsReportingServicesHelper
    {
        /// <summary>
        ///     Throws InvalidOperationException when MetricsMarkerService is not present
        ///     in the list of services.
        /// </summary>
        /// <param name="services">The list of services.</param>
        public static void ThrowIfMetricsNotRegistered(IServiceProvider services)
        {
            if (services.GetService(typeof(MetricsReportingMarkerService)) == null)
            {
                throw new InvalidOperationException(
                    "IServiceCollection.AddMetricsReportingCore()\nIServiceCollection.AddMetricsReporting()\n");
            }
        }
    }
}