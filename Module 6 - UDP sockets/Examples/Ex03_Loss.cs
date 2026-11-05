using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Module6
{
    // Ex03_Client sends 100 000 numbered datagrams as fast as it can; this server is busy for 2 s
    class Ex03_Loss : IExample
    {
        const int PORT = 6060;
        const int TOTAL = 100_000;

        public Task RunAsync()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Loopback, PORT));
            Console.WriteLine($"server: receive buffer {socket.ReceiveBufferSize:N0} bytes, waiting for Ex03_Client");

            byte[] buffer = new byte[65536];
            EndPoint client = new IPEndPoint(IPAddress.Any, 0);
            socket.ReceiveFrom(buffer, ref client); // datagram 0: the client has started
            Thread.Sleep(2000); // busy - the client keeps sending

            int received = 1;
            int outOfOrder = 0;
            int last = 0;
            socket.ReceiveTimeout = 1000;                                   // 1 s of silence: the client is done
            while (true)
            {
                try
                {
                    socket.ReceiveFrom(buffer, ref client);
                    int number = BitConverter.ToInt32(buffer, 0);
                    if (number < last)
                        outOfOrder++;
                    last = number;
                    received++;
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut)
                {
                    break;
                }
            }

            Console.WriteLine($"server: received {received} of {TOTAL}");
            Console.WriteLine($"out of order: {outOfOrder}");
            socket.Close();
            return Task.CompletedTask;
        }
    }
}
