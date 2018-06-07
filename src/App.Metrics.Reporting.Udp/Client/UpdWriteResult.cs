// <copyright file="UdpWriteResult.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Reporting.Udp.Client
{
    public struct UdpWriteResult
    {
        public UdpWriteResult(bool success)
        {
            Success = success;
            ErrorMessage = null;
        }

        public UdpWriteResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public bool Success { get; }
    }
}