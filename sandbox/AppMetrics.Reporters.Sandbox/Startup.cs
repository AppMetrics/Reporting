// <copyright file="Startup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.IO;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace AppMetrics.Reporters.Sandbox
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            var fileName = @"C:\metrics\http_received.txt";
            var file = new FileInfo(fileName);
            file.Directory?.Create();

            app.Run(
                async (context) =>
                {
                    if (context.Request.Method == "POST" && context.Request.Path == "/metrics-receive")
                    {
                        var req = context.Request;
                        req.EnableRewind();

                        using (var sr = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
                        {
                            File.WriteAllText(fileName, sr.ReadToEnd());
                        }

                        req.Body.Position = 0;

                        context.Response.StatusCode = 200;
                        await context.Response.WriteAsync("dumped metrics");
                    }
                });
        }
    }
}