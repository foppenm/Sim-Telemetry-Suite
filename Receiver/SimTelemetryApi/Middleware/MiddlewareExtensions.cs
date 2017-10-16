using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimTelemetryApi.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseUdpReceiver(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UdpReceiverMiddleware>();
        }
    }
}
