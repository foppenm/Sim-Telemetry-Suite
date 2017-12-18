using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Web.Hubs
{
    public class TelemetryHub : Hub
    {
        public Task Status(string message)
        {
            return Clients.AllExcept(new[] { Context.ConnectionId }).InvokeAsync("status", message);
        }

        public Task TrackPath(string message)
        {
            return Clients.AllExcept(new[] { Context.ConnectionId }).InvokeAsync("trackpath", message);
        }
    }
}
