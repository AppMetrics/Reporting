// <copyright file="Program.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Builder;
using App.Metrics.Reporting.Http.Client;
using App.Metrics.Scheduling;
using Microsoft.AspNetCore.Hosting;
using ReportingSandbox.Metrics;
using Serilog;

namespace ReportingSandbox
{
    public static class Program
    {
        private static IMetricsRoot Metrics { get; set; }

        private static IMetricsReporter MetricsReporter { get; set; }

        public static void Main()
        {
            var cpuUsage = new CpuUsage();
            cpuUsage.Start();

            Init();

            var process = Process.GetCurrentProcess();
            var scheduler = new DefaultTaskScheduler();

            var simpleMetrics = new SampleMetrics(Metrics);
            var setCounterSample = new SetCounterSample(Metrics);
            var setMeterSample = new SetMeterSample(Metrics);
            var userValueHistogramSample = new UserValueHistogramSample(Metrics);
            var userValueTimerSample = new UserValueTimerSample(Metrics);

            var cancellationTokenSource = new CancellationTokenSource();

            // cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));

            ScheduleSomeSampleMetrics(scheduler, setCounterSample, setMeterSample, userValueHistogramSample, userValueTimerSample, simpleMetrics, cpuUsage, process, cancellationTokenSource);

            ScheduleMetricsReporting(cancellationTokenSource);

            ListenForMetricsSentOverHttp();
        }

        private static void ScheduleMetricsReporting(CancellationTokenSource cancellationTokenSource)
        {
            Task.Run(
                () =>
                {
                    MetricsReporter.ScheduleReports(Metrics, cancellationTokenSource.Token);

                    Console.WriteLine("Report Cancelled...");
                },
                cancellationTokenSource.Token);
        }

        private static void ListenForMetricsSentOverHttp()
        {
            var host = new WebHostBuilder().UseKestrel().UseStartup<Startup>().Build();
            host.Run();
        }

        private static void ScheduleSomeSampleMetrics(
            IScheduler scheduler,
            SetCounterSample setCounterSample,
            SetMeterSample setMeterSample,
            UserValueHistogramSample userValueHistogramSample,
            UserValueTimerSample userValueTimerSample,
            SampleMetrics simpleMetrics,
            CpuUsage cpuUsage,
            Process process,
            CancellationTokenSource cancellationTokenSource)
        {
            var task = scheduler.Interval(
                TimeSpan.FromMilliseconds(300),
                TaskCreationOptions.LongRunning,
                () =>
                {
                    using (Metrics.Measure.Apdex.Track(AppMetricsRegistry.ApdexScores.AppApdex))
                    {
                        setCounterSample.RunSomeRequests();
                        setMeterSample.RunSomeRequests();
                        userValueHistogramSample.RunSomeRequests();
                        userValueTimerSample.RunSomeRequests();
                        simpleMetrics.RunSomeRequests();
                    }

                    Metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.Errors, () => 1);
                    Metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.PercentGauge, () => 1);
                    Metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.ApmGauge, () => 1);
                    Metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.ParenthesisGauge, () => 1);
                    Metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.GaugeWithNoValue, () => double.NaN);

                    Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.CpuUsageTotal,
                        () =>
                        {
                            cpuUsage.CallCpu();
                            return cpuUsage.CpuUsageTotal;
                        });
                    Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPagedMemorySizeGauge,
                        () => process.PagedMemorySize64);
                    Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPeekPagedMemorySizeGauge,
                        () => process.PeakPagedMemorySize64);
                    Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPeekVirtualMemorySizeGauge,
                        () => process.PeakVirtualMemorySize64);
                    Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPeekWorkingSetSizeGauge,
                        () => process.WorkingSet64);
                    Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPrivateMemorySizeGauge,
                        () => process.PrivateMemorySize64);
                    Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessVirtualMemorySizeGauge,
                        () => process.VirtualMemorySize64);
                    Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.SystemNonPagedMemoryGauge,
                        () => process.NonpagedSystemMemorySize64);
                    Metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.SystemPagedMemorySizeGauge,
                        () => process.PagedSystemMemorySize64);
                },
                cancellationTokenSource.Token);
        }

        private static void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.LiterateConsole()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            Metrics = AppMetrics.CreateDefaultBuilder().Build();

            // TODO: Filter at global level and allow each reporter to override?
            var reportingBuilder = new MetricsReportingBuilder(Metrics)
                .Options.Configure(options => options.Enabled = true)
                .ReportMetrics.ToConsole(
                    options =>
                    {
                        options.ReportInterval = TimeSpan.FromSeconds(3);
                    })
                .ReportMetrics.ToTextFile(
                    options =>
                    {
                        options.ReportInterval = TimeSpan.FromSeconds(5);
                        options.OutputPathAndFileName = @"C:\metrics\sample.txt";
                        options.AppendMetricsToTextFile = true;
                    })
                .ReportMetrics.OverHttp(
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
                    });

            MetricsReporter = reportingBuilder.Build();
        }
    }
}