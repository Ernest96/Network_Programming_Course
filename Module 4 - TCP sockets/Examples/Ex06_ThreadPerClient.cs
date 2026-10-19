using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Module4
{
    // The same server as Ex05, with one change: every client gets its own thread.
    // Same client too: the Ex05_Ex06_Client project.
    // The three answers now come together, after about 1 second.
    class Ex06_ThreadPerClient : IExample
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
                Thread thread = new Thread(() => Handle(client));
                thread.Start();                     // back to Accept right away
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
