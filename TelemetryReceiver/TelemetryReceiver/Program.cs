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
            var listener = new UdpListener(666);

            var loop = true;
            while (loop)
            {
                try
                {
                    Console.WriteLine("Waiting for data...");

                    // Blocks until a message returns on this socket from a remote host.
                    Byte[] receiveBytes = listener.GetDatagram();

                    Console.WriteLine("Received data:");

                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    Console.WriteLine(
                        $"Message received from" +
                        $" {listener.RemoteIpEndpoint.Address.ToString()}:{listener.RemoteIpEndpoint.Port.ToString()} :\n" +
                        returnData.ToString());
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
