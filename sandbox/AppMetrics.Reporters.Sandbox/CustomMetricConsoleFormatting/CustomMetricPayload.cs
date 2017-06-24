// <copyright file="CustomMetricPayload.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AppMetrics.Reporters.Sandbox.CustomMetricConsoleFormatting
{
    public class CustomMetricPayload
    {
        private readonly List<CustomMetricPoint> _points = new List<CustomMetricPoint>();

        public void Add(CustomMetricPoint point)
        {
            if (point == null)
            {
                return;
            }

            _points.Add(point);
        }

        public void Format(TextWriter textWriter)
        {
            if (textWriter == null)
            {
                return;
            }

            var points = _points.ToList();

            textWriter.Write("---Start Report---");

            foreach (var point in points)
            {
                point.Format(textWriter);
                textWriter.Write('\n');
            }

            textWriter.Write("---End Report---");
        }
    }
}