using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Module4
{
    // Heartbeat: the client sends "PING" every second. If the server hears nothing
    // for 3 seconds, it decides the client is dead and closes the connection.
    // The client is the Ex08_Client project.
    class Ex08_Heartbeat : IExample
    {
        const int PORT = 5050;

        public Task RunAsync()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, PORT));
            listener.Listen();
            Console.WriteLine("server: waiting for Ex08_Client");

            while (true)
            {
                Socket client = listener.Accept();
                client.ReceiveTimeout = 3000;                   // the longest silence we accept
                Stopwatch sw = Stopwatch.StartNew();

                byte[] buffer = new byte[16];
                while (true)
                {
                    try
                    {
                        int count = client.Receive(buffer);
                        if (count == 0)
                            break;                              // a normal close
                        Console.WriteLine($"server: {Encoding.UTF8.GetString(buffer, 0, count)} at {sw.ElapsedMilliseconds,5} ms");
                    }
                    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut)
                    {
                        Console.WriteLine($"server: nothing for 3 s at {sw.ElapsedMilliseconds,5} ms - the client is gone, closing");
                        break;
                    }
                }
                client.Close();
            }
        }
    }
}
