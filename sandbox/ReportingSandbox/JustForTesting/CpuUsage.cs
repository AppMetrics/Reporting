// <copyright file="CpuUsage.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;

namespace ReportingSandbox.JustForTesting
{
    public class CpuUsage
    {
        private static readonly DateTime StartTime = DateTime.UtcNow;
        private static TimeSpan _start;

        public double CpuUsageTotal { get; private set; }

        public void CallCpu()
        {
            var newCpuTime = Process.GetCurrentProcess().TotalProcessorTime - _start;
            CpuUsageTotal = newCpuTime.TotalSeconds / (Environment.ProcessorCount * DateTime.UtcNow.Subtract(StartTime).TotalSeconds);
        }

        public void Start() { _start = Process.GetCurrentProcess().TotalProcessorTime; }
    }
}