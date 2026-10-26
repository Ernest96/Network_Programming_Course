using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Module5
{

    class Ex01_JsonSimple : IExample
    {
        public Task RunAsync()
        {
            Student ana = new Student { Name = "Ana", Group = "TI-231", Year = 3 };

            string json = JsonConvert.SerializeObject(ana);                     // object -> text
            Console.WriteLine(json);

            Student copy = JsonConvert.DeserializeObject<Student>(json);        // text -> object
            Console.WriteLine($"{copy.Name}, {copy.Group}, year {copy.Year}");

            return Task.CompletedTask;
        }
    }
}
