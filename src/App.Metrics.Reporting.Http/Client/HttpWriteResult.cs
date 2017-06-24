// <copyright file="HttpWriteResult.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Reporting.Http.Client
{
    public struct HttpWriteResult
    {
        public HttpWriteResult(bool success)
        {
            Success = success;
            ErrorMessage = null;
        }

        public HttpWriteResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public bool Success { get; }
    }
}