// <copyright file="IMetricsReportingCoreBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     An interface for configuring App Metrics Reporting services.
    /// </summary>
    public interface IMetricsReportingCoreBuilder
    {
        /// <summary>
        ///     Gets the <see cref="IServiceCollection"/> where Metrics Reporting services are configured.
        /// </summary>
        IServiceCollection Services { get; }
    }
}