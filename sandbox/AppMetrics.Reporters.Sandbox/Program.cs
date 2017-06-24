﻿// <copyright file="Program.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Builder;
using App.Metrics.Core.Scheduling;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Health;
using App.Metrics.Reporting.Console;
using App.Metrics.Reporting.Http;
using App.Metrics.Reporting.Http.Client;
using App.Metrics.Reporting.TextFile;
using AppMetrics.Reporters.Sandbox.Metrics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppMetrics.Reporters.Sandbox
{
    public class Program
    {
        public static void Main()
        {
            var cpuUsage = new CpuUsage();
            cpuUsage.Start();

            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ConfigureMetrics(serviceCollection);

            var process = Process.GetCurrentProcess();

            var provider = serviceCollection.BuildServiceProvider();

            var application = new Application(provider);
            var scheduler = new DefaultTaskScheduler();

            var simpleMetrics = new SampleMetrics(application.Metrics);
            var setCounterSample = new SetCounterSample(application.Metrics);
            var setMeterSample = new SetMeterSample(application.Metrics);
            var userValueHistogramSample = new UserValueHistogramSample(application.Metrics);
            var userValueTimerSample = new UserValueTimerSample(application.Metrics);

            var cancellationTokenSource = new CancellationTokenSource();

            // cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));

            var task = scheduler.Interval(
                TimeSpan.FromMilliseconds(300),
                TaskCreationOptions.LongRunning,
                () =>
                {
                    using (application.Metrics.Measure.Apdex.Track(AppMetricsRegistry.ApdexScores.AppApdex))
                    {
                        setCounterSample.RunSomeRequests();
                        setMeterSample.RunSomeRequests();
                        userValueHistogramSample.RunSomeRequests();
                        userValueTimerSample.RunSomeRequests();
                        simpleMetrics.RunSomeRequests();
                    }

                    application.Metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.Errors, () => 1);
                    application.Metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.PercentGauge, () => 1);
                    application.Metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.ApmGauge, () => 1);
                    application.Metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.ParenthesisGauge, () => 1);
                    application.Metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.GaugeWithNoValue, () => double.NaN);

                    application.Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.CpuUsageTotal,
                        () =>
                        {
                            cpuUsage.CallCpu();
                            return cpuUsage.CpuUsageTotal;
                        });
                    application.Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPagedMemorySizeGauge,
                        () => process.PagedMemorySize64);
                    application.Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPeekPagedMemorySizeGauge,
                        () => process.PeakPagedMemorySize64);
                    application.Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPeekVirtualMemorySizeGauge,
                        () => process.PeakVirtualMemorySize64);
                    application.Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPeekWorkingSetSizeGauge,
                        () => process.WorkingSet64);
                    application.Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPrivateMemorySizeGauge,
                        () => process.PrivateMemorySize64);
                    application.Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessVirtualMemorySizeGauge,
                        () => process.VirtualMemorySize64);
                    application.Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.SystemNonPagedMemoryGauge,
                        () => process.NonpagedSystemMemorySize64);
                    application.Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.SystemPagedMemorySizeGauge,
                        () => process.PagedSystemMemorySize64);
                },
                cancellationTokenSource.Token);

            Task.Run(
                () =>
                {
                    application.Reporter.RunReports(application.Metrics, cancellationTokenSource.Token);

                    Console.WriteLine("Report Cancelled...");
                },
                cancellationToken: cancellationTokenSource.Token);

            var host = new WebHostBuilder().UseKestrel().UseStartup<Startup>().Build();

            host.Run();
        }

        private static void ConfigureMetrics(IServiceCollection services)
        {
            services.AddMetrics(
                         options =>
                         {
                             options.ReportingEnabled = true;
                             options.GlobalTags.Add("env", "stage");
                         }).
                     AddHealthChecks(
                         factory =>
                         {
                             factory.RegisterProcessPrivateMemorySizeHealthCheck("Private Memory Size", 200);
                             factory.RegisterProcessVirtualMemorySizeHealthCheck("Virtual Memory Size", 200);
                             factory.RegisterProcessPhysicalMemoryHealthCheck("Working Set", 200);

                             factory.Register("DatabaseConnected", () => Task.FromResult("Database Connection OK"));
                             factory.Register(
                                 "DiskSpace",
                                 () =>
                                 {
                                     var freeDiskSpace = GetFreeDiskSpace();

                                     return Task.FromResult(
                                         freeDiskSpace <= 512
                                             ? HealthCheckResult.Unhealthy("Not enough disk space: {0}", freeDiskSpace)
                                             : HealthCheckResult.Unhealthy("Disk space ok: {0}", freeDiskSpace));
                                 });
                         }).
                     AddReporting(
                         factory =>
                         {
                             // factory.AddConsole(
                             //     new ConsoleReporterSettings
                             //     {
                             //         ReportInterval = TimeSpan.FromSeconds(5),
                             //     },
                             //     new CustomMetricPayloadBuilder());

                             factory.AddConsole(
                                 new ConsoleReporterSettings
                                 {
                                     ReportInterval = TimeSpan.FromSeconds(20),
                                 },
                                 new LoggerFactory(),
                                 new LineProtocolPayloadBuilder());

                             factory.AddTextFile(
                                 new TextFileReporterSettings
                                 {
                                     ReportInterval = TimeSpan.FromSeconds(5),
                                     FileName = @"C:\metrics\sample.txt"
                                 },
                                 new LoggerFactory(),
                                 new AsciiMetricPayloadBuilder());

                             // factory.AddTextFile(
                             //     new TextFileReporterSettings
                             //     {
                             //         ReportInterval = TimeSpan.FromSeconds(5),
                             //         FileName = @"C:\metrics\sample.txt"
                             //     },
                             //     new LineProtocolPayloadBuilder());

                             factory.AddHttp(
                                 new HttpReporterSettings
                                 {
                                     HttpSettings = new HttpSettings(new Uri("http://localhost:5000/metrics-receive")),
                                     ReportInterval = TimeSpan.FromSeconds(5),
                                     HttpPolicy = new HttpPolicy
                                                  {
                                                      BackoffPeriod = TimeSpan.FromSeconds(30),
                                                      FailuresBeforeBackoff = 5,
                                                      Timeout = TimeSpan.FromSeconds(3)
                                                  }
                                 },
                                 new LoggerFactory(),
                                 new AsciiMetricPayloadBuilder());
                         });
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddLogging();

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();
        }

        private static int GetFreeDiskSpace() { return 1024; }
    }
}