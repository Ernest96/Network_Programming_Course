using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Module4
{
    // The same 1000 clients as Ex07a, with async per client.
    // While a client is silent, no thread waits for it - a handful of threads is enough.
    class Ex07b_ThousandAsync : IExample
    {
        const int PORT = 5050;
        const int CLIENTS = 1000;

        public async Task RunAsync()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, PORT));
            listener.Listen();
            Console.WriteLine("server: waiting for Ex07_Client");

            Task[] handlers = new Task[CLIENTS];
            for (int i = 0; i < CLIENTS; i++)
            {
                Socket client = await listener.AcceptAsync();
                handlers[i] = HandleAsync(client);          // not awaited: back to Accept at once
            }

            await Task.Delay(500);                          // the clients are silent now
            int osThreads = Process.GetCurrentProcess().Threads.Count;
            Console.WriteLine($"server: {CLIENTS} clients waiting, {osThreads} OS threads");

            await Task.WhenAll(handlers);
            listener.Close();
        }

        static async Task HandleAsync(Socket client)
        {
            await client.SendAsync(Encoding.UTF8.GetBytes("hi"));
            byte[] buffer = new byte[16];
            int count = await client.ReceiveAsync(buffer);  // no thread waits here
            await client.SendAsync(buffer.AsMemory(0, count));
            client.Close();
        }
    }
}
