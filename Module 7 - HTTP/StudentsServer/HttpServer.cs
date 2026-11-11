using System;
using System.IO;
using System.Net;
using System.Text;

namespace StudentsServer;

// A thin layer over HttpListener: receive a request, read it, send the answer.
public class HttpServer
{
    private readonly HttpListener _listener;
    private readonly string _address;

    public HttpServer(string address)
    {
        _address = address;
        _listener = new HttpListener();
        _listener.Prefixes.Add(address);
    }

    public void Start()
    {
        _listener.Start();
        Console.WriteLine($"server: Students API listens on {_address}students");
    }

    // Waits for the next request.
    public HttpListenerContext ServeRequest()
    {
        return _listener.GetContext();
    }

    public string GetBody(HttpListenerRequest request)
    {
        using StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    // "/students/2" -> ["students", "2"]
    public string[] GetUrlParts(HttpListenerRequest request)
    {
        return request.Url.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
    }

    // Status code, headers, body: the response.
    public void Send(HttpListenerResponse response, int status, string json)
    {
        response.StatusCode = status;
        if (json != null)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            response.ContentType = "application/json; charset=utf-8";
            response.ContentLength64 = bytes.Length;
            response.OutputStream.Write(bytes, 0, bytes.Length);
        }
        response.Close();
    }
}
