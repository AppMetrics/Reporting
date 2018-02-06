// <copyright file="HttpSettings.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Reporting.Http.Client
{
    public class HttpSettings
    {
        public HttpSettings(Uri requireUri) { RequestUri = requireUri ?? throw new ArgumentNullException(nameof(requireUri)); }

        public HttpSettings() { }

        /// <summary>
        ///     Gets or sets the password use to basic auth.
        /// </summary>
        /// <value>
        ///     The basic auth password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        ///     Gets or sets the request uri, where to POST metrics.
        /// </summary>
        /// <value>
        ///     The InfluxDB host.
        /// </value>
        public Uri RequestUri { get; set; }

        /// <summary>
        ///     Gets or sets the username use to basic auth.
        /// </summary>
        /// <value>
        ///     The basic auth username.
        /// </value>
        public string UserName { get; set; }
    }
}