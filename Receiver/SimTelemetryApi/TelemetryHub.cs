using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimTelemetryApi
{
    public class TelemetryHub : Hub
    {
        public Task Status(string message)
        {
            return Clients.All.InvokeAsync("Status", message);
        }
    }
}
