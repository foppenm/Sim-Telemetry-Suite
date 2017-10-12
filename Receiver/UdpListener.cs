using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Receiver
{
    public class UdpListener
    {
        int _port;
        UdpClient _client;
        IPEndPoint _remoteIpEndpoint;

        public IPEndPoint RemoteIpEndpoint
        {
            get { return _remoteIpEndpoint; }
            set { _remoteIpEndpoint = value; }
        }

        public UdpListener(int port)
        {
            _port = port;
            _client = new UdpClient(_port);

            // Create an IPEndPoint to record the IP Address and port number of the sender. 
            // The IPEndPoint will allow you to read datagrams sent from any source.
            _remoteIpEndpoint = new IPEndPoint(IPAddress.Any, _port);
        }

        public byte[] GetDatagram()
        {
            // Blocks until a message returns on this socket from a remote host.
            return _client.Receive(ref _remoteIpEndpoint);
        }
    }
}
