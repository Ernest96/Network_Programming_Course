using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Module4
{
    // A TCP echo server: whatever a client sends, the server sends back.
    // It serves one client at a time.

    // Try it from a terminal while it runs:
    //   macOS / Linux : nc localhost 5050
    //   Windows       : telnet localhost 5050
    class Ex01_EchoServer : IExample
    {
        const int PORT = 5050;

        public Task RunAsync()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, PORT));
            listener.Listen(2);
            Console.WriteLine($"server: listening on {listener.LocalEndPoint}");

            while (true)
            {
                Socket client = listener.Accept();          // waits for a client
                Console.WriteLine($"server: {client.RemoteEndPoint} connected");

                byte[] buffer = new byte[1024];
                while (true)
                {
                    int count = client.Receive(buffer);     // waits for data
                    if (count == 0)
                        break;                              // the client closed the connection

                    string text = Encoding.UTF8.GetString(buffer, 0, count);
                    Console.WriteLine($"server: received {count} bytes: {text.TrimEnd()}");

                    client.Send(buffer, 0, count, SocketFlags.None);   // echo it back
                }

                Console.WriteLine($"server: {client.RemoteEndPoint} closed the connection");
                client.Close();
            }
        }
    }
}
