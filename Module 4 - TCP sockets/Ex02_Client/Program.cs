using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Ex02, the client - a separate program. Run the Ex02_Server example first.

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Connect(new IPEndPoint(IPAddress.Loopback, 5050));

socket.Send(Encoding.UTF8.GetBytes("hello from client A"));
byte[] buffer = new byte[1024];
int count = socket.Receive(buffer);     // waits for the echo
Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, count));

socket.Shutdown(SocketShutdown.Send);   // "I am done sending" - sends FIN
count = socket.Receive(buffer);         // 0: the server has closed too
socket.Close();
