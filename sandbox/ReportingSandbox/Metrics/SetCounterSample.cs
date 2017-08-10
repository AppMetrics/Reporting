// <copyright file="SetCounterSample.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Counter;

namespace ReportingSandbox.Metrics
{
    public class SetCounterSample
    {
        private static IMetrics _metrics;
        private readonly ICounter _commandCounter;
        private readonly ICounter _commandCounterNoPercentages;
        private readonly ICounter _commandCounterNoReportSetItems;
        private readonly ICounter _commandCounterNotReset;

        public SetCounterSample(IMetrics metrics)
        {
            _metrics = metrics;

            _commandCounter = _metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.CommandCounter);
            _commandCounterNoPercentages = _metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.CommandCounterNoPercentages);
            _commandCounterNotReset = _metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.CommandCounterNotReset);
            _commandCounterNoReportSetItems = _metrics.Provider.Counter.Instance(SampleMetricsRegistry.Counters.CommandCounterDontReportSetItems);
        }

        public void Process(ICommand command)
        {
            _commandCounterNotReset.Increment(command.GetType().Name);
            _commandCounter.Increment(command.GetType().Name);
            _commandCounterNoPercentages.Increment(command.GetType().Name);
            _commandCounterNoReportSetItems.Increment(command.GetType().Name);
        }

        public void RunSomeRequests()
        {
            for (var i = 0; i < 30; i++)
            {
                var commandIndex = new Random().Next() % 5;
                if (commandIndex == 0)
                {
                    Process(new SendEmail());
                }

                if (commandIndex == 1)
                {
                    Process(new ShipProduct());
                }

                if (commandIndex == 2)
                {
                    Process(new BillCustomer());
                }

                if (commandIndex == 3)
                {
                    Process(new MakeInvoice());
                }

                if (commandIndex == 4)
                {
                    Process(new MarkAsPreffered());
                }
            }
        }

        public interface ICommand
        {
        }

        public class BillCustomer : ICommand
        {
        }

        public class MakeInvoice : ICommand
        {
        }

        public class MarkAsPreffered : ICommand
        {
        }

        public class SendEmail : ICommand
        {
        }

        public class ShipProduct : ICommand
        {
        }
    }
}