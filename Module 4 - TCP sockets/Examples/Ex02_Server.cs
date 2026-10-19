using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Module4
{
    // Ex02, the server. The client is a separate program: the Ex02_Client project.
    //
    // Listen(BACKLOG): the OS does the handshake with new clients by itself and keeps
    // them in a queue until the server calls Accept. BACKLOG is the size of that queue.
    class Ex02_Server : IExample
    {
        const int PORT = 5050;
        const int BACKLOG = 2;

        public Task RunAsync()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, PORT));
            listener.Listen(BACKLOG);
            Console.WriteLine($"server: listening on {listener.LocalEndPoint}, backlog = {BACKLOG}");
            Console.WriteLine("server: no Accept yet - run Ex02_Client, then press Enter here");
            Console.ReadLine();

            while (true)
            {
                Socket client = listener.Accept();          // the oldest waiting client
                Console.WriteLine($"server: accepted {client.RemoteEndPoint}");

                byte[] buffer = new byte[1024];
                while (true)
                {
                    int count = client.Receive(buffer);
                    Console.WriteLine($"server: Receive returned {count}");
                    if (count == 0)
                        break;                              // the client is done sending (FIN)

                    client.Send(buffer, 0, count, SocketFlags.None);   // echo it back
                }

                client.Close();
                Console.WriteLine("server: connection closed");
            }
        }
    }
}
