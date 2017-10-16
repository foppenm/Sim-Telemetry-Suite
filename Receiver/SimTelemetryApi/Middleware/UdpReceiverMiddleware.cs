using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using UdpReceiver;

namespace SimTelemetryApi.Middleware
{
    public class UdpReceiverMiddleware
    {
        readonly RequestDelegate _next;
        TelemetryHub _hub;

        public UdpReceiverMiddleware(RequestDelegate next)
        {
            _next = next;
            _hub = new TelemetryHub();
        }

        public Task Invoke(HttpContext context)
        {
            // We do not need the call back
            Task.Run(ReceiveUdpTraffic);

            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }

        private async Task ReceiveUdpTraffic()
        {
            using (var listener = new UdpListener(666))
            {
                var loop = true;
                while (loop)
                {
                    try
                    {
                        Console.WriteLine("Waiting for Bridge data...");

                        // Blocks until a message returns on this socket from a remote host.
                        var udpResult = await listener.GetDatagram();
                        var receiveBytes = udpResult.Buffer;

                        Console.WriteLine("Received data:");

                        string returnData = Encoding.ASCII.GetString(receiveBytes);

                        Console.WriteLine(
                            $"Message received from" +
                            $" {udpResult.RemoteEndPoint.Address.ToString()}:{udpResult.RemoteEndPoint.Port.ToString()} :\n" +
                            returnData.ToString());

                        // send the json string to somewhere
                        //await _hub.Status(returnData);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        loop = false;
                    }
                }
            }
        }
    }
}
