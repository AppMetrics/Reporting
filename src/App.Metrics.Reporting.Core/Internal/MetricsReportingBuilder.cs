// <copyright file="MetricsReportingBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Reporting.Internal
{
    public class MetricsReportingBuilder : IMetricsReportingBuilder
    {
        public MetricsReportingBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }
    }
}