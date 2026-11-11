using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Module7
{
    // HTTP is text over TCP: the request written by hand, sent with a TCP socket (Module 4).
    class Ex04_RawHttpClient : IExample
    {
        public Task RunAsync()
        {
            // no address family: the socket can use IPv4 or IPv6, whichever localhost turns out to be
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect("127.0.0.1", 5050);

            string request = "GET /students HTTP/1.1\r\n" +
                             "Host: 127.0.0.1:5050\r\n" +
                             "Connection: close\r\n" +
                             "\r\n";                                // an empty line: the end of the headers
            socket.Send(Encoding.ASCII.GetBytes(request));
            Console.Write(request);

            byte[] buffer = new byte[4096];
            StringBuilder response = new StringBuilder();
            while (true)
            {
                int count = socket.Receive(buffer);
                if (count == 0)
                    break;                                          // the server closed the connection: done
                response.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            socket.Close();

            Console.WriteLine(response);
            return Task.CompletedTask;
        }
    }
}
