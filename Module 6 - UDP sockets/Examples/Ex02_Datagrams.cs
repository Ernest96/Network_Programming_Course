using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Module6
{
    // UDP keeps the boundaries: one SendTo is one ReceiveFrom. The client is the Ex02_Client project.
    class Ex02_Datagrams : IExample
    {
        const int PORT = 6060;

        public Task RunAsync()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Loopback, PORT));
            Console.WriteLine("server: waiting for Ex02_Client");

            byte[] buffer = new byte[65536];
            EndPoint client = new IPEndPoint(IPAddress.Any, 0);

            // Part 1
            int count = socket.ReceiveFrom(buffer, ref client);
            string message;
            
            Thread.Sleep(500); // a busy server reads the rest later
            message = Encoding.UTF8.GetString(buffer, 0, count);
            Console.WriteLine($"server: ReceiveFrom #1: {count} bytes '{message}'");
            
            for (int i = 2; i <= 3; i++)
            {
                count = socket.ReceiveFrom(buffer, ref client);
                message = Encoding.UTF8.GetString(buffer, 0, count);
                Console.WriteLine($"server: ReceiveFrom #{i}: {count} bytes '{message}'");
            }

            // Part 2
            count = socket.ReceiveFrom(buffer, ref client);
            Console.WriteLine($"server: ReceiveFrom #4: {count:N0} bytes - the whole datagram at once");

            socket.Close();
            return Task.CompletedTask;
        }
    }
}
