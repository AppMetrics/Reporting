// <copyright file="MetricsController.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace MetricsReceiveSanboxApi.Controllers
{
    [Route("api/[controller]")]
    public class MetricsController : Controller
    {
        [HttpPost]
        public void Post([FromBody] MetricsModel metrics)
        {
        }
    }
}