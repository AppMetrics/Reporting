// <copyright file="Program.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.Json;
using App.Metrics.Reporting.Http.Client;
using App.Metrics.Scheduling;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReportingSandbox.Metrics;

namespace ReportingSandbox
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
                action: () =>
                {
                    application.MetricsReporter.ScheduleReports(cancellationTokenSource.Token);

                    Console.WriteLine("Report Cancelled...");
                },
                cancellationToken: cancellationTokenSource.Token);

            var host = new WebHostBuilder().UseKestrel().UseStartup<Startup>().Build();

            host.Run();
        }

        private static void ConfigureMetrics(IServiceCollection services)
        {
            var builder = services.AddMetrics(
                options =>
                {
                    options.GlobalTags.Remove("env");
                    options.GlobalTags.Add("env", "stage");
                });

            builder.AddMetricsReporting()
                .AddConsole(options =>
                   {
                       options.ReportInterval = TimeSpan.FromSeconds(3);
                   })
                .AddTextFile(
                       options =>
                       {
                           options.ReportInterval = TimeSpan.FromSeconds(5);
                           options.OutputPathAndFileName = @"C:\metrics\sample.txt";
                           options.AppendMetricsToTextFile = true;
                           options.MetricsOutputFormatter = new MetricsTextOutputFormatter();
                       })
                .AddHttp(
                       options =>
                       {
                           options.ReportInterval = TimeSpan.FromSeconds(5);
                           options.HttpSettings = new HttpSettings(new Uri("http://localhost:5000/metrics-receive"));
                           options.HttpPolicy = new HttpPolicy
                                                {
                                                    BackoffPeriod = TimeSpan.FromSeconds(30),
                                                    FailuresBeforeBackoff = 5,
                                                    Timeout = TimeSpan.FromSeconds(3)
                                                };
                           options.MetricsOutputFormatter = new MetricsJsonOutputFormatter();
                       });
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();
        }
    }
}