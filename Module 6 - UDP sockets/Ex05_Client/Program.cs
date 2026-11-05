using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Ex05, the client: send to the group. Run Ex05_Multicast first - in two windows.
IPAddress group = IPAddress.Parse("239.1.2.3");
Console.Write("IP of your network card (127.0.0.1 = this computer only): ");
IPAddress localIp = IPAddress.Parse(Console.ReadLine());

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
// send through that network card - the receivers joined the group there
socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, localIp.GetAddressBytes());

for (int i = 1; i <= 3; i++)
{
    string message = $"news {i}";
    socket.SendTo(Encoding.UTF8.GetBytes(message), new IPEndPoint(group, 6060));
    Console.WriteLine($"sender: \"{message}\" to {group}:6060");
}

socket.Close();
