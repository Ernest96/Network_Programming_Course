using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Module4
{
    // 1000 clients, a thread per client - no async.
    // Each client stays silent for 1 second, so 1000 threads wait in Receive at the same time.
    class Ex07a_ThousandThreads : IExample
    {
        const int PORT = 5050;
        const int CLIENTS = 1000;

        public Task RunAsync()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, PORT));
            listener.Listen();
            Console.WriteLine("server: waiting for Ex07_Client");

            Thread[] threads = new Thread[CLIENTS];
            for (int i = 0; i < CLIENTS; i++)
            {
                Socket client = listener.Accept();
                threads[i] = new Thread(() => Handle(client));
                threads[i].Start();
            }

            Thread.Sleep(500);                              // the clients are silent now
            int osThreads = Process.GetCurrentProcess().Threads.Count;
            Console.WriteLine($"server: {CLIENTS} clients waiting, {osThreads} OS threads");

            foreach (Thread t in threads)
                t.Join();
            listener.Close();
            return Task.CompletedTask;
        }

        static void Handle(Socket client)
        {
            client.Send(Encoding.UTF8.GetBytes("hi"));      // the client waits for this before the next one connects
            byte[] buffer = new byte[16];
            int count = client.Receive(buffer);             // this thread waits ~1 s
            client.Send(buffer, 0, count, SocketFlags.None);
            client.Close();
        }
    }
}
