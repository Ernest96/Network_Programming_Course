using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// Ex03, the client. Run the Ex03_StreamNotMessages example first.

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Connect(new IPEndPoint(IPAddress.Loopback, 5050));

// Part 1: three messages, three Sends
socket.Send(Encoding.UTF8.GetBytes("HELLO"));
socket.Send(Encoding.UTF8.GetBytes("WORLD"));
socket.Send(Encoding.UTF8.GetBytes("BYE"));
Console.WriteLine("client: 3 x Send   \"HELLO\"  \"WORLD\"  \"BYE\"");

Thread.Sleep(1000);

// Part 2: one big message, one Send
byte[] big = new byte[1_000_000];
socket.Send(big);
Console.WriteLine($"client: 1 x Send   {big.Length:N0} bytes");

socket.Shutdown(SocketShutdown.Send);
socket.Close();
