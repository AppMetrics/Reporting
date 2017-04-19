using System;
using System.Collections.Generic;
using System.Text;

namespace App.Metrics.Extensions.Reporting.Graphite
{
    public enum LoggingEvents
    {
        /// <summary>
        /// Error while sending metrics using TcpClient
        /// </summary>
        TcpSendError,

        /// <summary>
        /// Error while creating TcpClient
        /// </summary>
        TcpClientCreateError,

        /// <summary>
        /// Error while sending metrics using UdpClient
        /// </summary>
        UdpSendError
    }
}
