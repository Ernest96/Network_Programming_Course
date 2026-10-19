using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Module4
{
    // A server that serves one client at a time. Each request takes it 1 second.
    // The client is the Ex05_Ex06_Client project: three clients at the same moment.
    // All three get connected at once (the OS does the handshake) - but they are answered one by one.
    // After the three clients, the server stops.
    class Ex05_OneClientAtATime : IExample
    {
        const int PORT = 5050;
        const int CLIENTS = 3;

        public Task RunAsync()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, PORT));
            listener.Listen();
            Console.WriteLine("server: waiting for Ex05_Ex06_Client");

            for (int i = 0; i < CLIENTS; i++)
            {
                Socket client = listener.Accept();
                Handle(client);                     // the next Accept waits until this client is done
            }

            listener.Close();
            return Task.CompletedTask;
        }

        static void Handle(Socket client)
        {
            byte[] buffer = new byte[1024];
            int count = client.Receive(buffer);
            Thread.Sleep(1000);                     // 1 s of work for this request
            client.Send(buffer, 0, count, SocketFlags.None);
            client.Close();
        }
    }
}
