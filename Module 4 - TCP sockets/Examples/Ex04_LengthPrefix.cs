using System;
using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Module4
{
    // Framing: we decide where a message ends - TCP will not.
    // Every message is sent as [4 bytes: length][the bytes of the message].
    // The receiver reads exactly 4 bytes, then exactly 'length' bytes.
    class Ex04_LengthPrefix : IExample
    {
        const int PORT = 5050;

        public Task RunAsync()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, PORT));
            listener.Listen();
            Console.WriteLine("server: waiting for Ex04_Client");

            while (true)
            {
                Socket client = listener.Accept();
                Thread.Sleep(500);

                while (true)
                {
                    string message = ReceiveMessage(client);
                    if (message == null)
                        break;                          // the client closed
                    Console.WriteLine($"server: message \"{message}\"");
                }

                Console.WriteLine("server: the client closed");
                client.Close();
            }
        }

        // Returns null when the other side has closed.
        static string ReceiveMessage(Socket socket)
        {
            byte[] header = new byte[4];
            if (!ReceiveExactly(socket, header))
                return null;

            int length = BinaryPrimitives.ReadInt32BigEndian(header);
            byte[] payload = new byte[length];
            if (!ReceiveExactly(socket, payload))
                return null;

            return Encoding.UTF8.GetString(payload);
        }

        static bool ReceiveExactly(Socket socket, byte[] buffer)
        {
            int received = 0;
            while (received < buffer.Length)
            {
                int count = socket.Receive(buffer, received, buffer.Length - received, SocketFlags.None);
                if (count == 0)
                    return false;                   // closed before the buffer was full
                received += count;
            }
            return true;
        }
    }
}
