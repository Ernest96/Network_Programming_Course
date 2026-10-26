using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Module5
{
    class Ex05_JsonArray : IExample
    {
        public Task RunAsync()
        {
            List<Student> students = new List<Student>
            {
                new Student { Name = "Ana", Group = "TI-231", Year = 3 },
                new Student { Name = "Ion", Group = "TI-231", Year = 3 },
                new Student { Name = "Maria", Group = "TI-242", Year = 2 },
            };

            string json = JsonConvert.SerializeObject(students, Formatting.Indented);
            Console.WriteLine(json);

            List<Student> copy = JsonConvert.DeserializeObject<List<Student>>(json);
            Console.WriteLine($"{copy.Count} students, the last one is {copy[2].Name}");

            return Task.CompletedTask;
        }
    }
}
