using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Ex01, the client. Run the Ex01_EchoServer example first.
// No Bind and no Connect: the first SendTo picks a free local port.

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
IPEndPoint server = new IPEndPoint(IPAddress.Loopback, 6060);

socket.SendTo(Encoding.UTF8.GetBytes("hello from the client"), server);
Console.WriteLine($"client: sent data");

byte[] buffer = new byte[65536];
EndPoint from = new IPEndPoint(IPAddress.Any, 0);
int count = socket.ReceiveFrom(buffer, ref from); // waits for the echo

Console.WriteLine($"client: {count} bytes from {from}: {Encoding.UTF8.GetString(buffer, 0, count)}");

socket.Close();
