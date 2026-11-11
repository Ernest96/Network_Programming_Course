using System;
using System.Net;
using Newtonsoft.Json;
using StudentsServer;

//   GET    /students       all the students
//   GET    /students/2     one student
//   POST   /students       add a student (JSON in the body)
//   PUT    /students/2     replace a student (JSON in the body)
//   DELETE /students/2     delete a student

HttpServer server = new HttpServer("http://*:5050/");
StudentsRepository studentsRepo = new StudentsRepository();

server.Start();

while (true)
{
    HttpListenerContext context = server.ServeRequest();
    HttpListenerRequest request = context.Request;
    HttpListenerResponse response = context.Response;

    string body = server.GetBody(request);
    string[] parts = server.GetUrlParts(request);

    int status;
    string responseData;

    // CORS: a page from another origin (Ex03_JavaScript) may read our answers.
    if (request.Headers["Origin"] != null)
        response.AddHeader("Access-Control-Allow-Origin", "*");

    if (request.HttpMethod == "OPTIONS")
    {
        // before a POST, PUT or DELETE, such a page asks first: may I send this method, this header?
        response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
        response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
        status = 204;
        responseData = null;
    }
    else if (parts.Length == 1 && IsStudentsPath(parts[0])) // /students
    {
        
        if (request.HttpMethod == "GET")
        {
            status = 200;
            responseData = ToJson(studentsRepo.GetStudents());
        }
        else if (request.HttpMethod == "POST")
        {
            try
            {
                Student student = studentsRepo.CreateStudent(body);

                response.AddHeader("Location", "/students/" + student.Id);
                status = 201;
                responseData = ToJson(student);
            }
            catch (ArgumentException e)
            {
                status = 400;
                responseData = ToJson(new Error(e.Message));
            }
        }
        else
        {
            status = 405;
            responseData = ToJson(new Error("use GET or POST"));
        }
    }
    else if (parts.Length == 2 && IsStudentsPath(parts[0]) && int.TryParse(parts[1], out int id))
    {
        // /students/{id}
        Student student = studentsRepo.GetStudent(id);

        if (student == null)
        {
            status = 404;
            responseData = ToJson(new Error("no student with id " + id));
        }
        else if (request.HttpMethod == "GET")
        {
            status = 200;
            responseData = ToJson(student);
        }
        else if (request.HttpMethod == "PUT")
        {
            try
            {
                studentsRepo.UpdateStudent(student, body);

                status = 200;
                responseData = ToJson(student);
            }
            catch (ArgumentException e)
            {
                status = 400;
                responseData = ToJson(new Error(e.Message));
            }
        }
        else if (request.HttpMethod == "DELETE")
        {
            studentsRepo.DeleteStudent(student);

            status = 204;
            responseData = null;
        }
        else
        {
            status = 405;
            responseData = ToJson(new Error("use GET, PUT or DELETE"));
        }
    }
    else
    {
        status = 404;
        responseData = ToJson(new Error("not found"));
    }

    if (body.Length > 0)
        Console.WriteLine($"server: {request.HttpMethod} {request.Url.AbsolutePath} {body} -> {status}");
    else
        Console.WriteLine($"server: {request.HttpMethod} {request.Url.AbsolutePath} -> {status}");

    server.Send(response, status, responseData);
}

static bool IsStudentsPath(string part)
{
    return part.Equals("students", StringComparison.OrdinalIgnoreCase);
}

static string ToJson(object value)
{
    return JsonConvert.SerializeObject(value);
}
