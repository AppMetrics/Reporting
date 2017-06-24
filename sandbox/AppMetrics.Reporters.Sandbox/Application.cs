// <copyright file="Application.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Reporting;
using Microsoft.Extensions.DependencyInjection;

namespace AppMetrics.Reporters.Sandbox
{
    public class Application
    {
        public Application(IServiceProvider provider)
        {
            Metrics = provider.GetRequiredService<IMetrics>();

            var reporterFactory = provider.GetRequiredService<IReportFactory>();
            Reporter = reporterFactory.CreateReporter();
        }

        public IMetrics Metrics { get; }

        public IReporter Reporter { get; }
    }
}