// <copyright file="SocketSettings.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Net.Sockets;

namespace App.Metrics.Reporting.Socket.Client
{
    public class SocketSettings
    {
        public SocketSettings(ProtocolType protocolType, string address, int port)
        {
            Validate(protocolType, address, port);
            ProtocolType = protocolType;
            Address = address;
            Port = port;
        }

        public SocketSettings() { }

        /// <summary>
        ///     Gets or sets Protocol to send data.
        /// </summary>
        /// <value>
        ///     Possible variants are TCP and UDP.
        /// </value>
        public ProtocolType ProtocolType { get; set; }

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

        public string Endpoint
        {
            get
            {
                if (ProtocolType == ProtocolType.Tcp)
                {
                    return $"tcp://{Address}:{Port}";
                }

                if (ProtocolType == ProtocolType.Udp)
                {
                    return $"udp://{Address}:{Port}";
                }

                return "Failed Setings Instance";
            }
        }

        public static void Validate(
            ProtocolType protocolType,
            string address,
            int port)
        {
            if (protocolType != ProtocolType.Tcp && protocolType != ProtocolType.Udp)
            {
                throw new ArgumentOutOfRangeException(nameof(protocolType), "Only available TCP and UDP");
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (port <= IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(port),
                    port,
                    $"Port should be in ({IPEndPoint.MinPort}; {IPEndPoint.MaxPort}) range.");
            }

            string endpoint = $"{address}:{port}";
            if (!Uri.TryCreate(endpoint, UriKind.Absolute, out var uri))
            {
                throw new InvalidOperationException($"{endpoint} must be a valid absolute URI.");
            }
        }
    }
}
