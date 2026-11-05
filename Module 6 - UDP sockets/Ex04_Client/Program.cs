using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Ex04, the client: one broadcast, many answers. Run Ex04_Discovery first - in several windows or on several machines.
Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
socket.EnableBroadcast = true; // without it, SendTo to a broadcast address is refused
socket.ReceiveTimeout = 1000;  // collect the answers for 1 s

string message = "Who is there?";
socket.SendTo(Encoding.UTF8.GetBytes(message), new IPEndPoint(IPAddress.Broadcast, 6060));
Console.WriteLine("client: asked 255.255.255.255:6060");

byte[] buffer = new byte[65536];
int answers = 0;
while (true)
{
    try
    {
        EndPoint from = new IPEndPoint(IPAddress.Any, 0);
        int count = socket.ReceiveFrom(buffer, ref from);
        answers++;
        Console.WriteLine($"client: {from} says \"{Encoding.UTF8.GetString(buffer, 0, count)}\"");
    }
    catch (SocketException)
    {
        break; // 1 s of silence: everyone has answered
    }
}
Console.WriteLine($"client: {answers} answer(s)");

socket.Close();
