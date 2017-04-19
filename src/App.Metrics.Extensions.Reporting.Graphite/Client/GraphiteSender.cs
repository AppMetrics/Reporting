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
        private readonly Queue<string> _dataQueue = new Queue<string>();

        protected readonly string _host;
        protected readonly int _port;

        protected GraphiteSender(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public void Send(DateTime time, string name, string value)
        {
            var timestamp = time.ToUnixTime().ToString("D", CultureInfo.InvariantCulture);
            _dataQueue.Enqueue($"{name} {value} {timestamp}\n");
        }

        public virtual async Task Flush()
        {
            while (_dataQueue.Any())
            {
                var data = _dataQueue.Dequeue();
                var bytes = Encoding.UTF8.GetBytes(data);
                await SendData(bytes);
            }
        }

        protected abstract Task SendData(byte[] bytes);

        /// <inheritdoc />
        public abstract void Dispose();
    }
}