using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Module5
{
    class Ex07_JsonServer : IExample
    {
        const int PORT = 5050;

        public Task RunAsync()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, PORT));
            listener.Listen();
            Console.WriteLine("server: waiting for Ex07_Client");

            while (true)
            {
                Socket client = listener.Accept();
                string requestJson = ReceiveAll(client);
                Console.WriteLine($"server: got  {requestJson}");

                GradesRequest request = JsonConvert.DeserializeObject<GradesRequest>(requestJson);

                int sum = 0;
                foreach (int grade in request.Grades)
                    sum += grade;

                GradesReply reply = new GradesReply();
                reply.Name = request.Student.Name;
                reply.Average = Math.Round((double)sum / request.Grades.Count, 2);
                reply.Passed = reply.Average >= 5;

                string replyJson = JsonConvert.SerializeObject(reply);
                client.Send(Encoding.UTF8.GetBytes(replyJson));
                Console.WriteLine($"server: sent {replyJson}");
                client.Close();
            }
        }

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
    }
}
