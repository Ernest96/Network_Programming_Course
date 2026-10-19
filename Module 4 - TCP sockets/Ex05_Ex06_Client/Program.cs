using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

// The client for Ex05 and Ex06: three clients, A, B and C, at the same moment.
// Each one sends its name and waits for the answer. Run the server example first.

Stopwatch sw = Stopwatch.StartNew();

// all three connect and send their request right away...
Socket a = ConnectAndSend("client A");
Socket b = ConnectAndSend("client B");
Socket c = ConnectAndSend("client C");

// ...then each one waits for its answer
WaitForAnswer(a, "client A");
WaitForAnswer(b, "client B");
WaitForAnswer(c, "client C");

Socket ConnectAndSend(string name)
{
    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    socket.Connect(new IPEndPoint(IPAddress.Loopback, 5050));
    Console.WriteLine($"{name}: connected after {sw.ElapsedMilliseconds,5} ms");
    socket.Send(Encoding.UTF8.GetBytes(name));
    return socket;
}

void WaitForAnswer(Socket socket, string name)
{
    byte[] buffer = new byte[1024];
    socket.Receive(buffer);
    Console.WriteLine($"{name}: answer after    {sw.ElapsedMilliseconds,5} ms");
    socket.Close();
}
