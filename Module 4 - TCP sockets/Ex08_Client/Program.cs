using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// Ex08, the client: PING every second, three times - then silence, as if the machine

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Connect(new IPEndPoint(IPAddress.Loopback, 5050));

for (int i = 0; i < 3; i++)
{
    socket.Send(Encoding.UTF8.GetBytes("PING"));
    Console.WriteLine("client: PING");
    Thread.Sleep(1000);
}

Console.WriteLine("client: silent now - no PING, no close. Press Enter to quit.");
Console.ReadLine();
socket.Close();
