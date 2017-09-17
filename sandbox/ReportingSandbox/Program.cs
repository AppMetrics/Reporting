// <copyright file="Program.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Reporting.Http.Client;
using Microsoft.AspNetCore.Hosting;
using ReportingSandbox.JustForTesting;
using Serilog;
using Serilog.Events;

namespace ReportingSandbox
{
    public static class Program
    {
        public static IMetricsRoot Metrics { get; private set; }

        public static Task Main()
        {
            Init();

            SampleMetricsRunner.ScheduleSomeSampleMetrics(Metrics);

            // Using AspNet Core to host a HTTP endpoint which receives metrics as JSON via the HTTP reporter.
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            host.Run();

            return Task.CompletedTask;
        }

        private static void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                .WriteTo.LiterateConsole()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            Metrics = AppMetrics.CreateDefaultBuilder()
                .Report.ToConsole(TimeSpan.FromSeconds(3))
                .Report.ToTextFile(@"C:\metrics\sample.txt", TimeSpan.FromSeconds(5))
                .Report.OverHttp("http://localhost:5000/metrics-receive", TimeSpan.FromSeconds(10))
                .Build();
        }
    }
}