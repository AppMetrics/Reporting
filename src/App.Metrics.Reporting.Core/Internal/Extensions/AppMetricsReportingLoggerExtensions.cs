// <copyright file="AppMetricsReportingLoggerExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.Reporting;

// ReSharper disable CheckNamespace
namespace App.Metrics.Logging
// ReSharper restore CheckNamespace
{
    [ExcludeFromCodeCoverage]
    internal static class AppMetricsReportingLoggerExtensions
    {
        private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;

        public static void ReportFailed(this ILog logger, IMetricsReporterProvider metricsReporter, Exception ex)
        {
            logger.Error(ex, $"{metricsReporter.GetType()} failed during execution");
        }

        public static void ReportFailed(this ILog logger, IMetricsReporterProvider metricsReporter)
        {
            logger.Error($"{metricsReporter.GetType()} failed during execution");
        }

        public static void ReportingCancelled(this ILog logger, OperationCanceledException ex)
        {
            logger.Error(ex, "Report execution cancelled");
        }

        public static void ReportingDisposedDuringExecution(this ILog logger, ObjectDisposedException ex)
        {
            logger.Error(ex, "Report execution stopped");
        }

        public static void ReportingFailedDuringExecution(this ILog logger, AggregateException ex)
        {
            logger.Error(ex.Flatten(), "Report execution stopped");
        }

        public static void ReportRan(this ILog logger, IMetricsReporterProvider metricsReporter, long startTimestamp)
        {
            if (!logger.IsTraceEnabled())
            {
                return;
            }

            if (startTimestamp == 0)
            {
                return;
            }

            var currentTimestamp = Stopwatch.GetTimestamp();
            var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));

            logger.Debug("Report {ReportType} ran in {ElapsedMilliseconds}ms", metricsReporter.GetType().FullName, elapsed.Milliseconds);
        }

        public static void ReportRunning(this ILog logger, IMetricsReporterProvider metricsReporterProvider)
        {
            logger.Trace($"Running {metricsReporterProvider.GetType()}");
        }
    }

    // ReSharper restore RedundantStringInterpolation
}