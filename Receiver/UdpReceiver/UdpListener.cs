using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpReceiver
{
    public class UdpListener : IDisposable
    {
        int _port;
        UdpClient _client;

        public UdpListener(int port)
        {
            _port = port;
            _client = new UdpClient(_port);
        }

        public async Task<UdpReceiveResult> GetDatagram()
        {
            return await _client.ReceiveAsync();
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
