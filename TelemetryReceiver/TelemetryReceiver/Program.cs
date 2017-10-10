using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TelemetryReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Setting up UDP client...");

            var listenPort = 666;

            // Creates a UdpClient for reading incoming data.
            UdpClient receivingUdpClient = new UdpClient(listenPort);

            // Creates an IPEndPoint to record the IP Address and port number of the sender. 
            // The IPEndPoint will allow you to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, listenPort);

            var loop = true;

            while (loop)
            {
                try
                {
                    Console.WriteLine("Waiting for data...");

                    // Blocks until a message returns on this socket from a remote host.
                    Byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);

                    Console.WriteLine("Received data!");

                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    Console.WriteLine("This is the message you received " +
                                                 returnData.ToString());
                    Console.WriteLine("This message was sent from " +
                                                RemoteIpEndPoint.Address.ToString() +
                                                " on their port number " +
                                                RemoteIpEndPoint.Port.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    loop = false;
                }

            }

            Console.Read();
        }
    }
}
