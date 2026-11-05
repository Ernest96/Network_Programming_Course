using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Module6
{
    // Multicast: one datagram to a group address, a copy for every member of the group.
    class Ex05_Multicast : IExample
    {
        const int PORT = 6060;

        public Task RunAsync()
        {
            IPAddress group = IPAddress.Parse("239.1.2.3");
            Console.Write("IP of your network card (127.0.0.1 = this computer only): ");
            IPAddress localIp = IPAddress.Parse(Console.ReadLine());

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            // two receivers on the same machine can bind the same port
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            socket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            // join the group on that network card: from now on, datagrams sent to 239.1.2.3 reach this socket
            MulticastOption membership = new MulticastOption(group, localIp);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, membership);
            Console.WriteLine($"receiver: joined {group} on {localIp}, port {PORT}");

            byte[] buffer = new byte[65536];
            while (true)
            {
                EndPoint from = new IPEndPoint(IPAddress.Any, 0);
                int count = socket.ReceiveFrom(buffer, ref from);
                Console.WriteLine($"receiver: \"{Encoding.UTF8.GetString(buffer, 0, count)}\" from {from}");
            }
        }
    }
}
