using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Ex07_Client;

GradesRequest request = new GradesRequest
{
    Student = new Student { Name = "Ana", Group = "TI-231", Year = 3 },
    Grades = new List<int> { 9, 10, 7 },
};
string json = JsonConvert.SerializeObject(request);

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Connect(new IPEndPoint(IPAddress.Loopback, 5050));
socket.Send(Encoding.UTF8.GetBytes(json));
socket.Shutdown(SocketShutdown.Send);           // "the request is complete" - must come before ReceiveAll

string replyJson = ReceiveAll(socket);
socket.Close();

Console.WriteLine($"client: sent {json}");
Console.WriteLine($"client: got  {replyJson}");

GradesReply reply = JsonConvert.DeserializeObject<GradesReply>(replyJson);
Console.WriteLine($"{reply.Name}: average {reply.Average}, passed: {reply.Passed}");

// Reads until the other side is done sending (Receive returns 0).
static string ReceiveAll(Socket socket)
{
    List<byte> data = new List<byte>();
    byte[] buffer = new byte[1024];
    while (true)
    {
        int count = socket.Receive(buffer);
        if (count == 0)
            break;
        for (int i = 0; i < count; i++)
            data.Add(buffer[i]);
    }
    return Encoding.UTF8.GetString(data.ToArray());
}
