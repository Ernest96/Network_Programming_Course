using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Module5
{
    // A list of objects in XML: XML has no arrays, so the element repeats.
    // XmlSerializer names the list <ArrayOfStudent>.
    class Ex06_XmlArray : IExample
    {
        public Task RunAsync()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));

            List<Student> students = new List<Student>
            {
                new Student { Name = "Ana", Group = "TI-231", Year = 3 },
                new Student { Name = "Ion", Group = "TI-231", Year = 3 },
                new Student { Name = "Maria", Group = "TI-242", Year = 2 },
            };

            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, students);
            string xml = writer.ToString();
            Console.WriteLine(xml);

            StringReader reader = new StringReader(xml);
            List<Student> copy = (List<Student>)serializer.Deserialize(reader);
            Console.WriteLine($"{copy.Count} students, the last one is {copy[2].Name}");

            return Task.CompletedTask;
        }
    }
}
