using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Module4
{
    // TCP delivers a stream of bytes, not messages. The client is the Ex03_Client project.
    //   Part 1: the client calls Send three times - one Receive here gets all three, glued together.
    //   Part 2: the client sends 1 MB with one Send - here it takes many Receive calls.
    class Ex03_StreamNotMessages : IExample
    {
        const int PORT = 5050;
        const int BIG = 1_000_000;

        public Task RunAsync()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, PORT));
            listener.Listen();
            Console.WriteLine("server: waiting for Ex03_Client");

            while (true)
            {
                Socket client = listener.Accept();
                Thread.Sleep(500);                      // a busy server reads a moment later

                // Part 1
                byte[] buffer = new byte[64 * 1024];
                int count = client.Receive(buffer);
                Console.WriteLine($"server: 1 x Receive {count} bytes: \"{Encoding.UTF8.GetString(buffer, 0, count)}\"");

                // Part 2
                List<int> sizes = new List<int>();
                int total = 0;
                while (total < BIG)
                {
                    count = client.Receive(buffer);
                    if (count == 0)
                        break;
                    sizes.Add(count);
                    total += count;
                }
                Console.WriteLine($"server: {sizes.Count} x Receive for {total:N0} bytes");

                client.Close();
            }
        }
    }
}
