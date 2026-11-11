using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StudentsServer;

// Keeps the students in memory: everything the API can do with them, without any HTTP.
public class StudentsRepository
{
    private static readonly JsonSerializerSettings IgnoreNulls =
        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

    private int _nextId = 3;

    private readonly List<Student> _students = new List<Student>
    {
        new Student { Id = 1, Name = "Ana", Group = "TI-231", Year = 3 },
        new Student { Id = 2, Name = "Ion", Group = "TI-232", Year = 2 },
    };

    public List<Student> GetStudents()
    {
        return _students;
    }

    // The student with this id, or null when there is none.
    public Student GetStudent(int id)
    {
        return _students.Find(s => s.Id == id);
    }

    // Adds a new student from JSON; the server gives it the next id.
    public Student CreateStudent(string studentJson)
    {
        Student student = ParseStudent(studentJson);
        student.Id = _nextId++;

        _students.Add(student);

        return student;
    }

    // Replaces the data of an existing student; the id stays the same.
    public Student UpdateStudent(Student student, string studentJson)
    {
        Student changed = ParseStudent(studentJson);

        student.Name = changed.Name;
        student.Group = changed.Group;
        student.Year = changed.Year;

        return student;
    }

    public void DeleteStudent(Student student)
    {
        _students.Remove(student);
    }

    // The JSON as a student; throws ArgumentException when it is not a valid one.
    private static Student ParseStudent(string studentJson)
    {
        Student student;
        try
        {
            // a client may send "Id": null (Ex02 does): the server picks the id anyway, so nulls are skipped
            student = JsonConvert.DeserializeObject<Student>(studentJson, IgnoreNulls);
        }
        catch (JsonException)
        {
            student = null;
        }

        if (student == null || string.IsNullOrEmpty(student.Name) || string.IsNullOrEmpty(student.Group))
            throw new ArgumentException("send a student as JSON, with a Name and a Group");

        return student;
    }
}
