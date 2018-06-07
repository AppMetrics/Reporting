// <copyright file="UdpSettings.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net;

namespace App.Metrics.Reporting.Udp.Client
{
    public class UdpSettings
    {
        public UdpSettings(string address, int port)
        {
            Validate(address, port);
            Address = address;
            Port = port;
        }

        public UdpSettings() { }

        /// <summary>
        ///     Gets or sets Address to send data.
        /// </summary>
        /// <value>
        ///     Name of remote host.
        /// </value>
        public string Address { get; set; }

        /// <summary>
        ///     Gets or sets port to send data.
        /// </summary>
        /// <value>
        ///     The remote port.
        /// </value>
        public int Port { get; set; }

        public static void Validate(
            string address,
            int port)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (port <= IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(port),
                    port,
                    "Port should be in ({IPEndPoint.MinPort}; {IPEndPoint.MaxPort}) range.");
            }

            string endpoint = "{address}:{port}";
            if (!Uri.TryCreate(endpoint, UriKind.Absolute, out var uri))
            {
                throw new InvalidOperationException($"{endpoint} must be a valid absolute URI.");
            }
        }

        public override string ToString()
        {
            return "udp://{Address}:{Port}";
        }
    }
}
