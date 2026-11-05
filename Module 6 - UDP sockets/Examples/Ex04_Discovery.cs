using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Module6
{
    // Broadcast: find each other on the local network.
    // Run it several times - in several windows or on several computers. Each server answers with its name.
    class Ex04_Discovery : IExample
    {
        const int PORT = 6060;

        public Task RunAsync()
        {
            Console.Write("Your name: ");
            string name = Console.ReadLine();

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            // several servers on one computer can bind the same port
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            // Any (0.0.0.0): a broadcast goes to 255.255.255.255, a socket bound to one IP never gets it
            socket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            Console.WriteLine($"server: {name} waiting on port {PORT}");

            byte[] buffer = new byte[65536];
            while (true)
            {
                EndPoint asker = new IPEndPoint(IPAddress.Any, 0);
                int count = socket.ReceiveFrom(buffer, ref asker);
                string message = Encoding.UTF8.GetString(buffer, 0, count);
                Console.WriteLine($"server: '{message}' from {asker}");

                if (message == "Who is there?")
                    socket.SendTo(Encoding.UTF8.GetBytes($"I am {name}"), asker);
            }
        }
    }
}
