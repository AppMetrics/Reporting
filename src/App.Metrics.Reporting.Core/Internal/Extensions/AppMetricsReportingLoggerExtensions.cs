// <copyright file="AppMetricsReportingLoggerExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.Reporting;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Logging
    // ReSharper restore CheckNamespace
{
    // ReSharper disable RedundantStringInterpolation

    [ExcludeFromCodeCoverage]
    internal static class AppMetricsReportingLoggerExtensions
    {
        private static readonly Action<ILogger, string, double, Exception> ReportRanAction;
        private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;

        static AppMetricsReportingLoggerExtensions()
        {
            ReportRanAction = LoggerMessage.Define<string, double>(
                LogLevel.Trace,
                AppMetricsEventIds.Reports.Schedule,
                $"Report {{reportType}} ran in {{elapsedMilliseconds}}ms");
        }

        public static void ReportFailed(this ILogger logger, IReporterProvider reporter, Exception ex)
        {
            logger.LogError(AppMetricsEventIds.Reports.Schedule, ex, $"{reporter.GetType()} failed during execution");
        }

        public static void ReportFailed(this ILogger logger, IReporterProvider reporter)
        {
            logger.LogError(AppMetricsEventIds.Reports.Schedule, $"{reporter.GetType()} failed during execution");
        }

        public static void ReportingCancelled(this ILogger logger, OperationCanceledException ex)
        {
            logger.LogError(AppMetricsEventIds.Reports.Schedule, ex, "Report execution cancelled");
        }

        public static void ReportingDisposedDuringExecution(this ILogger logger, ObjectDisposedException ex)
        {
            logger.LogError(AppMetricsEventIds.Reports.Schedule, ex, "Report execution stopped");
        }

        public static void ReportingFailedDuringExecution(this ILogger logger, AggregateException ex)
        {
            logger.LogError(AppMetricsEventIds.Reports.Schedule, ex.Flatten(), "Report execution stopped");
        }

        public static void ReportRan(this ILogger logger, IReporterProvider reporter, long startTimestamp)
        {
            if (!logger.IsEnabled(LogLevel.Trace))
            {
                return;
            }

            if (startTimestamp == 0)
            {
                return;
            }

            var currentTimestamp = Stopwatch.GetTimestamp();
            var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));

            ReportRanAction(logger, reporter.GetType().FullName, elapsed.TotalMilliseconds, null);
        }

        public static void ReportRunning(this ILogger logger, IReporterProvider reporterProvider)
        {
            logger.LogTrace(AppMetricsEventIds.Reports.Schedule, $"Running {reporterProvider.GetType()}");
        }

        internal static class AppMetricsEventIds
        {
            private const int MetricsStart = 1000;

            public static class Reports
            {
                public const int Schedule = MetricsStart + 1;
            }
        }
    }

    // ReSharper restore RedundantStringInterpolation
}