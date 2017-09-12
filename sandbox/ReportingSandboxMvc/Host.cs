// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.AspNetCore.Reporting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ReportingSandboxMvc
{
    public static class Host
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                            .UseMetrics()
                            .UseMetricsReporting(ConfigureMetricsReportingOptions())
                            .UseStartup<Startup>().Build();
        }

        public static void Main(string[] args) { BuildWebHost(args).Run(); }

        private static Action<WebHostBuilderContext, MetricsReportingWebHostOptions> ConfigureMetricsReportingOptions()
        {
            return (context, options) =>
            {
                options.ReportingBuilder = builder =>
                {
                    builder
                        .ReportMetrics.ToConsole()
                        .ReportMetrics.ToTextFile(textFileOptions => textFileOptions.OutputPathAndFileName = @"C:\metrics\sample.txt");
                };
                options.UnobservedTaskExceptionHandler = (sender, args) =>
                {
                    Trace.WriteLine(args.Exception);
                };
            };
        }
    }
}