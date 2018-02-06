// <copyright file="Program.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace MetricsReceiveSanboxApi
{
    public static class Program
    {
        public static IMetricsRoot Metrics { get; private set; }

        public static Task Main()
        {
            Init();

            var host = new WebHostBuilder()
                       .UseKestrel()
                       .UseStartup<Startup>()
                       .UseUrls("http://localhost:50002")
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
        }
    }
}