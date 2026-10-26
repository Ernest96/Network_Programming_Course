using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Module5
{
    class Ex04_XmlFromFile : IExample
    {
        public Task RunAsync()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Student));

            string xml = File.ReadAllText("Data/student.xml");             // file -> text
            StringReader reader = new StringReader(xml);
            Student student = (Student)serializer.Deserialize(reader);         // text -> object

            Console.WriteLine($"{student.Name}, {student.Group}, year {student.Year}");

            return Task.CompletedTask;
        }
    }
}
