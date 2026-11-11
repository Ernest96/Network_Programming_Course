using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using StudentsServer;

//   GET    /students       all the students
//   GET    /students/2     one student
//   POST   /students       add a student (JSON in the body)
//   PUT    /students/2     replace a student (JSON in the body)
//   DELETE /students/2     delete a student

List<Student> students = new List<Student>
{
    new Student { Id = 1, Name = "Ana", Group = "TI-231", Year = 3 },
    new Student { Id = 2, Name = "Ion", Group = "TI-232", Year = 2 },
};
int nextId = 3;

string address = "http://*:5050/";
HttpListener listener = new HttpListener();
listener.Prefixes.Add(address);
listener.Start();
Console.WriteLine($"server: Students API on {address}students");

while (true)
{
    HttpListenerContext context = listener.GetContext(); // waits for the next request
    HttpListenerRequest request = context.Request;
    HttpListenerResponse response = context.Response;

    StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8);
    string body = reader.ReadToEnd(); 

    string[] parts = request.Url.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

    int status = 404;
    string json = "{\"error\":\"not found\"}";

    // CORS.
    if (request.Headers["Origin"] != null)
        response.AddHeader("Access-Control-Allow-Origin", "*");

    if (request.HttpMethod == "OPTIONS")
    {
        // before a POST, PUT or DELETE, such a page asks first: may I send this method, this header?
        response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
        response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
        status = 204;
        json = null;
    }
    else if (parts.Length == 1 && parts[0].Equals("students", StringComparison.OrdinalIgnoreCase))
    {
        if (request.HttpMethod == "GET")
        {
            status = 200;
            json = JsonConvert.SerializeObject(students);
        }
        else if (request.HttpMethod == "POST")
        {
            Student student = ReadStudent(body);
            if (student == null)
            {
                status = 400;
                json = "{\"error\":\"send a student as JSON\"}";
            }
            else
            {
                student.Id = nextId;
                nextId++;
                students.Add(student);
                response.AddHeader("Location", "/students/" + student.Id);
                status = 201;
                json = JsonConvert.SerializeObject(student);
            }
        }
        else
        {
            status = 405;
            json = "{\"error\":\"use GET or POST\"}";
        }
    }
    else if (parts.Length == 2 && parts[0].Equals("students", StringComparison.OrdinalIgnoreCase) && int.TryParse(parts[1], out int id))
    {
        Student student = students.Find(s => s.Id == id);
        if (student == null)
        {
            status = 404;
            json = "{\"error\":\"no student " + id + "\"}";
        }
        else if (request.HttpMethod == "GET")
        {
            status = 200;
            json = JsonConvert.SerializeObject(student);
        }
        else if (request.HttpMethod == "PUT")
        {
            Student changed = ReadStudent(body);
            if (changed == null)
            {
                status = 400;
                json = "{\"error\":\"send a student as JSON\"}";
            }
            else
            {
                student.Name = changed.Name;
                student.Group = changed.Group;
                student.Year = changed.Year;
                status = 200;
                json = JsonConvert.SerializeObject(student);
            }
        }
        else if (request.HttpMethod == "DELETE")
        {
            students.Remove(student);
            status = 204;
            json = null;
        }
        else
        {
            status = 405;
            json = "{\"error\":\"use GET, PUT or DELETE\"}";
        }
    }

    if (body.Length > 0)
        Console.WriteLine($"server: {request.HttpMethod} {request.Url.AbsolutePath} {body} -> {status}");
    else
        Console.WriteLine($"server: {request.HttpMethod} {request.Url.AbsolutePath} -> {status}");
    Send(response, status, json);
}

// The body as a student, or null when it is not a valid one.
static Student ReadStudent(string body)
{
    try
    {
        Student student = JsonConvert.DeserializeObject<Student>(body);
        if (student == null || string.IsNullOrEmpty(student.Name))
            return null;
        return student;
    }
    catch (JsonException)
    {
        return null;
    }
}

// Status code, headers, body: the response.
static void Send(HttpListenerResponse response, int status, string json)
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
