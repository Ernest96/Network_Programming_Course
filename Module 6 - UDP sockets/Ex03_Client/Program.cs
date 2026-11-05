using System;
using System.Net;
using System.Net.Sockets;

// Ex03, the client: 100 000 numbered datagrams, as fast as possible. Run the Ex03_Loss example first.

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
IPEndPoint server = new IPEndPoint(IPAddress.Loopback, 6060);

int refused = 0;
for (int i = 0; i < 100_000; i++)
{
    try
    {
        socket.SendTo(BitConverter.GetBytes(i), server); // 4 bytes: datagram index
    }
    catch (SocketException)
    {
        refused++; // some systems refuse when you send too fast
    }
}
Console.WriteLine($"client: sent 100 000 datagrams, {refused} refused by the OS");

socket.Close();
