// <copyright file="Startup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.IO;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace ReportingSandbox
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class Startup
        // ReSharper restore ClassNeverInstantiated.Global
    {
        // ReSharper disable UnusedMember.Global
        public void ConfigureServices(IServiceCollection services)
            // ReSharper restore UnusedMember.Global
        {
            // TODO: At the moment using the IHostedService implemented in App.Metrics.AspNetCore.Reporting,
            // similar will be provided when the "Generic Host" is available - https://github.com/aspnet/Hosting/issues/1163
            services.AddMetricsReportScheduler();
            services.AddMetrics(Program.Metrics);
        }

        // ReSharper disable UnusedMember.Global
        public void Configure(IApplicationBuilder app)
            // ReSharper restore UnusedMember.Global
        {
            var fileName = EnsureMetricsDumpFile();

            app.Run(
                async context =>
                {
                    if (context.Request.Method == "POST" && context.Request.Path == "/metrics-receive")
                    {
                        var req = context.Request;
                        req.EnableRewind();

                        using (var sr = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
                        {
                            await File.WriteAllTextAsync(fileName, sr.ReadToEnd());
                        }

                        req.Body.Position = 0;

                        context.Response.StatusCode = 200;
                        await context.Response.WriteAsync("dumped metrics");
                    }
                });
        }

        private static string EnsureMetricsDumpFile()
        {
            const string fileName = @"C:\metrics\http_received.txt";
            var file = new FileInfo(fileName);
            file.Directory?.Create();
            return fileName;
        }
    }
}