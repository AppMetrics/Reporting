using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Extensions.Reporting.Graphite.Extentions;

namespace App.Metrics.Extensions.Reporting.Graphite.Client
{
    public abstract class GraphiteSender : IDisposable
    {
        protected readonly string _host;
        protected readonly int _port;
        private readonly Queue<string> _dataQueue = new Queue<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphiteSender"/> class.
        /// </summary>
        /// <param name="host">graphite server host</param>
        /// <param name="port">graphite server port</param>
        protected GraphiteSender(string host, int port)
        {
            _host = host;
            _port = port;
        }

        /// <inheritdoc />
        public abstract void Dispose();

        /// <summary>
        /// queues metrics to be sent to the server
        /// </summary>
        /// <param name="time">time of metric</param>
        /// <param name="name">name of metric</param>
        /// <param name="value">metric value</param>
        public void Send(DateTime time, string name, string value)
        {
            var timestamp = time.ToUnixTime().ToString("D", CultureInfo.InvariantCulture);
            _dataQueue.Enqueue($"{name} {value} {timestamp}\n");
        }

        /// <summary>
        /// flushes the metrics that were queued
        /// </summary>
        /// <returns></returns>
        public virtual async Task Flush()
        {
            while (_dataQueue.Any())
            {
                var data = _dataQueue.Dequeue();
                var bytes = Encoding.UTF8.GetBytes(data);
                await SendData(bytes);
            }
        }

        /// <summary>
        /// implement sending the data to the server
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        protected abstract Task SendData(byte[] bytes);
    }
}
