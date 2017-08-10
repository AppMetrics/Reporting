// <copyright file="ReporterTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Reporting.Facts.Fixtures;
using App.Metrics.Reporting.Facts.TestHelpers;
using App.Metrics.Reporting.Internal;
using App.Metrics.Scheduling;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace App.Metrics.Reporting.Facts
{
    public class ReporterTests : IClassFixture<MetricsReportingFixture>
    {
        private readonly IEnumerable<IReporterProvider> _reporterProviders;
        private readonly MetricsReportingFixture _fixture;
        private readonly ILogger<DefaultMetricsReporter> _logger;

        public ReporterTests(MetricsReportingFixture fixture)
        {
            var loggerFactory = new LoggerFactory();
            _fixture = fixture;
            _logger = loggerFactory.CreateLogger<DefaultMetricsReporter>();
            _reporterProviders = new[] { new TestReportProvider() };
        }

        [Fact]
        public void Can_generate_report_successfully()
        {
            var scheduler = new DefaultTaskScheduler();
            var reporter = new DefaultMetricsReporter(_reporterProviders, _fixture.Metrics(), scheduler, _logger);
            var token = new CancellationTokenSource();
            token.CancelAfter(100);

            Action action = () => { reporter.ScheduleReports(token.Token); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void Imetrics_is_required()
        {
            Action action = () =>
            {
                var scheduler = new DefaultTaskScheduler();
                var unused = new DefaultMetricsReporter(_reporterProviders, null, scheduler, _logger);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Logger_is_required()
        {
            Action action = () =>
            {
                var scheduler = new DefaultTaskScheduler();
                var unused = new DefaultMetricsReporter(_reporterProviders, _fixture.Metrics(), scheduler, null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Report_providers_are_required()
        {
            Action action = () =>
            {
                var loggerFactory = new LoggerFactory();
                var scheduler = new DefaultTaskScheduler();
                var unused = new DefaultMetricsReporter(null, _fixture.Metrics(), scheduler, _logger);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Scheduler_is_required()
        {
            Action action = () =>
            {
                var unused = new DefaultMetricsReporter(_reporterProviders, _fixture.Metrics(), null, _logger);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Schedules_reports_to_run_at_the_specified_interval()
        {
            var interval = TimeSpan.FromSeconds(60);
            var provider = new Mock<IReporterProvider>();
            provider.SetupGet(s => s.ReportInterval).Returns(TimeSpan.FromSeconds(60));
            var scheduler = new Mock<IScheduler>();
            provider.Setup(p => p.FlushAsync(It.IsAny<MetricsDataValueSource>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var metrics = _fixture.Metrics();

            var reporter = new DefaultMetricsReporter(new[] { provider.Object }, metrics, scheduler.Object, _logger);

            reporter.ScheduleReports(CancellationToken.None);

            scheduler.Verify(
                p => p.Interval(interval, TaskCreationOptions.LongRunning, It.IsAny<Action>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void When_metric_report_fails_should_not_throw()
        {
            var provider = new TestReportProvider(TimeSpan.FromMilliseconds(10), false, new Exception());
            var scheduler = new DefaultTaskScheduler();
            var metrics = _fixture.Metrics();

            var reporter = new DefaultMetricsReporter(new[] { provider }, metrics, scheduler, _logger);
            var token = new CancellationTokenSource();
            token.CancelAfter(100);

            Action action = () => { reporter.ScheduleReports(token.Token); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void When_metric_reporter_fails_continues_to_retry()
        {
            var provider = new TestReportProvider(TimeSpan.FromMilliseconds(10), false);
            var scheduler = new DefaultTaskScheduler();
            var metrics = _fixture.Metrics();
            var reporter = new DefaultMetricsReporter(new[] { provider }, metrics, scheduler, _logger);
            var token = new CancellationTokenSource();
            token.CancelAfter(100);

            Action action = () => { reporter.ScheduleReports(token.Token); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void When_null_providers_doest_throw()
        {
            var scheduler = new Mock<IScheduler>();
            var metrics = _fixture.Metrics();
            var reporter = new DefaultMetricsReporter(_reporterProviders, metrics, scheduler.Object, _logger);

            reporter.ScheduleReports(CancellationToken.None);
        }
    }
}