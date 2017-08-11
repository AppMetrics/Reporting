// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ReportingSandboxMvc
{
    public static class Host
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseMetrics()
                .UseMetricsReporting(
                               options =>
                               {
                                   options.AddConsole();
                                   options.AddTextFile(
                                       textFileOptions =>
                                       {
                                           textFileOptions.OutputPathAndFileName = @"C:\metrics\metrics_web.txt";
                                           textFileOptions.ReportInterval = TimeSpan.FromSeconds(5);
                                           textFileOptions.AppendMetricsToTextFile = false;
                                       });
                               })
                .UseStartup<Startup>()
                .Build();
        }

        public static void Main(string[] args) { BuildWebHost(args).Run(); }
    }
}