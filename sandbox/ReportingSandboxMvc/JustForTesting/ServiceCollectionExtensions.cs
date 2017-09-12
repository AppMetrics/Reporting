// <copyright file="ServiceCollectionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Tracking;
using Microsoft.Extensions.Options;
using ReportingSandboxMvc.JustForTesting;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTestStuff(this IServiceCollection services)
        {
            services.AddTransient<Func<double, RequestDurationForApdexTesting>>(
                serviceProvider => { return apdexTSeconds => new RequestDurationForApdexTesting(apdexTSeconds); });

            services.AddSingleton<RandomValuesForTesting>();

            services.AddTransient(
                serviceProvider =>
                {
                    var optionsAccessor = serviceProvider.GetRequiredService<IOptions<MetricsWebTrackingOptions>>();
                    return new RequestDurationForApdexTesting(optionsAccessor.Value.ApdexTSeconds);
                });

            return services;
        }
    }
}