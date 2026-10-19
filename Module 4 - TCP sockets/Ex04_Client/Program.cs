using System;
using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Ex04, the client. Run the Ex04_LengthPrefix example first.
// Every message goes out as [4 bytes: length][the bytes of the message].

Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Connect(new IPEndPoint(IPAddress.Loopback, 5050));

SendMessage(socket, "HELLO");
SendMessage(socket, "WORLD");
SendMessage(socket, "BYE");
Console.WriteLine("client: sent 3 messages");

socket.Shutdown(SocketShutdown.Send);
socket.Close();

static void SendMessage(Socket socket, string text)
{
    byte[] payload = Encoding.UTF8.GetBytes(text);
    byte[] header = new byte[4];
    BinaryPrimitives.WriteInt32BigEndian(header, payload.Length);

    socket.Send(header);
    socket.Send(payload);
}
