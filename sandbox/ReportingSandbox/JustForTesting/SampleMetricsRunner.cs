// <copyright file="SampleMetricsRunner.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Scheduling;
using ReportingSandbox.Metrics;

namespace ReportingSandbox.JustForTesting
{
    public static class SampleMetricsRunner
    {
        public static void ScheduleSomeSampleMetrics(IMetrics metrics)
        {
            var simpleMetrics = new SampleMetrics(metrics);
            var setCounterSample = new SetCounterSample(metrics);
            var setMeterSample = new SetMeterSample(metrics);
            var userValueHistogramSample = new UserValueHistogramSample(metrics);
            var process = Process.GetCurrentProcess();
            var cpuUsage = new CpuUsage();
            cpuUsage.Start();

            var samplesScheduler = new AppMetricsTaskScheduler(
                TimeSpan.FromMilliseconds(300),
                () =>
                {
                    using (metrics.Measure.Apdex.Track(AppMetricsRegistry.ApdexScores.AppApdex))
                    {
                        setCounterSample.RunSomeRequests();
                        setMeterSample.RunSomeRequests();
                        userValueHistogramSample.RunSomeRequests();
                        UserValueTimerSample.RunSomeRequests(metrics);
                        simpleMetrics.RunSomeRequests();
                    }

                    metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.Errors, () => 1);
                    metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.PercentGauge, () => 1);
                    metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.ApmGauge, () => 1);
                    metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.ParenthesisGauge, () => 1);
                    metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.GaugeWithNoValue, () => double.NaN);

                    metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.CpuUsageTotal,
                        () =>
                        {
                            cpuUsage.CallCpu();
                            return cpuUsage.CpuUsageTotal;
                        });
                    metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPagedMemorySizeGauge,
                        () => process.PagedMemorySize64);
                    metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPeekPagedMemorySizeGauge,
                        () => process.PeakPagedMemorySize64);
                    metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPeekVirtualMemorySizeGauge,
                        () => process.PeakVirtualMemorySize64);
                    metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPeekWorkingSetSizeGauge,
                        () => process.WorkingSet64);
                    metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessPrivateMemorySizeGauge,
                        () => process.PrivateMemorySize64);
                    metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.ProcessVirtualMemorySizeGauge,
                        () => process.VirtualMemorySize64);
                    metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.SystemNonPagedMemoryGauge,
                        () => process.NonpagedSystemMemorySize64);
                    metrics.Measure.Gauge.SetValue(
                        AppMetricsRegistry.ProcessMetrics.SystemPagedMemorySizeGauge,
                        () => process.PagedSystemMemorySize64);

                    return Task.CompletedTask;
                });

            samplesScheduler.Start();
        }
    }
}
