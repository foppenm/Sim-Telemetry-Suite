using AutoMapper;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Receiver.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Receiver
{
    public class HubSender : IDisposable
    {
        private readonly string _hubBaseUrl;
        private readonly string _hubName;
        private readonly HubConnection _hubConnection;

        public HubSender(string hubBaseUrl, string hubName)
        {
            _hubBaseUrl = hubBaseUrl;
            _hubName = hubName;
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{_hubBaseUrl}{_hubName}")
                .WithConsoleLogger()
                .WithJsonProtocol()
                .WithTransport(Microsoft.AspNetCore.Sockets.TransportType.WebSockets)
                .Build();
        }

        public async Task Start()
        {
            // Wait for the hub to be online
            await WaitForHubOnline();

            // Start the hub connection
            await _hubConnection.StartAsync();

            var loop = true;
            do
            {
                try
                {
                    if (Globals.Messages.Count == 0)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    // Send the latest message
                    var message = Globals.Messages.Last();
                    if (message != null)
                    {
                        // Map json to our model
                        var mappedTrack = Mapper.Map<Track>(message);
                        var trackAsJson = JsonConvert.SerializeObject(mappedTrack, Formatting.None);

                        // Send the json string to the clients
                        await _hubConnection.InvokeAsync("status", trackAsJson);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    loop = false;
                }

                Thread.Sleep(200);
            } while (loop);

        }

        private async Task WaitForHubOnline()
        {
            var waiting = true;
            do
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_hubBaseUrl);
                    request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
                    request.Method = "HEAD";

                    Console.WriteLine("Checking Hub status...");
                    var response = await request.GetResponseAsync();
                    waiting = false;
                }
                catch (WebException)
                {
                    Console.WriteLine("Hub not online yet, trying again in 5 seconds...");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Thread.Sleep(5000);
            } while (waiting);

        }

        public void Dispose()
        {
            _hubConnection.DisposeAsync();
        }
    }
}
