using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Module6
{
    // A UDP echo server: whatever a client sends, the server sends back.
    class Ex01_EchoServer : IExample
    {
        const int PORT = 6060;

        public Task RunAsync()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Loopback, PORT));
            Console.WriteLine($"server: waiting for datagrams on {socket.LocalEndPoint}");

            byte[] buffer = new byte[65536];
            while (true)
            {
                EndPoint client = new IPEndPoint(IPAddress.Any, 0);
                int count = socket.ReceiveFrom(buffer, ref client);
                string text = Encoding.UTF8.GetString(buffer, 0, count);
                Console.WriteLine($"server: {count} bytes from {client}: {text}");

                socket.SendTo(buffer, 0, count, SocketFlags.None, client); 
            }
        }
    }
}
