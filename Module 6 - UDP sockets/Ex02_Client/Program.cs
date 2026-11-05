using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// Ex02, the client. Run the Ex02_Datagrams example first.

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
IPEndPoint server = new IPEndPoint(IPAddress.Loopback, 6060);

// Part 1: three messages, three datagrams
socket.SendTo(Encoding.UTF8.GetBytes("HELLO"), server);
socket.SendTo(Encoding.UTF8.GetBytes("WORLD"), server);
socket.SendTo(Encoding.UTF8.GetBytes("BYE"), server);
Console.WriteLine("client: 3 x SendTo   \"HELLO\"  \"WORLD\"  \"BYE\"");

Thread.Sleep(1000);

// Part 2: how big can one datagram be?
try
{
    socket.SendTo(new byte[100_000], server);
}
catch (SocketException ex)
{
    Console.WriteLine($"client: SendTo of 100 000 bytes - {ex.SocketErrorCode}");    // too big for one datagram
}
socket.SendTo(new byte[65_507], server);                                            // the largest one (IPv4)
Console.WriteLine("client: SendTo of 65 507 bytes - sent");

socket.Close();
