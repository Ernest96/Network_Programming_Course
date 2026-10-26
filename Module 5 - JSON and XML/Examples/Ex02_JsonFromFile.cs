using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Module5
{
    class Ex02_JsonFromFile : IExample
    {
        public Task RunAsync()
        {
            string json = File.ReadAllText("Data/student.json"); // file -> text
            Student student = JsonConvert.DeserializeObject<Student>(json);    // text -> object

            Console.WriteLine($"{student.Name}, {student.Group}, year {student.Year}");

            return Task.CompletedTask;
        }
    }
}
