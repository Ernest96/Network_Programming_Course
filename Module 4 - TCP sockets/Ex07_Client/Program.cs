using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// Ex07, the client: 1000 clients. Run the Ex07a_ThousandThreads or the Ex07b_ThousandAsync example first.

const int CLIENTS = 1000;
Stopwatch sw = Stopwatch.StartNew();
Socket[] sockets = new Socket[CLIENTS];
byte[] buffer = new byte[16];

// 1000 clients connect, one by one, and wait for the greeting "hi"
for (int i = 0; i < CLIENTS; i++)
{
    sockets[i] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    sockets[i].Connect(new IPEndPoint(IPAddress.Loopback, 5050));
    sockets[i].Receive(buffer);
}

Thread.Sleep(1000);                     // all 1000 are silent for 1 s

// then each one sends "ping" and gets it back
for (int i = 0; i < CLIENTS; i++)
{
    sockets[i].Send(Encoding.UTF8.GetBytes("ping"));
    sockets[i].Receive(buffer);
    sockets[i].Close();
}
Console.WriteLine($"client: {CLIENTS} clients in {sw.ElapsedMilliseconds} ms");
