using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Module5
{
    class Ex03_XmlSimple : IExample
    {
        public Task RunAsync()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Student));
            Student ana = new Student { Name = "Ana", Group = "TI-231", Year = 3 };

            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, ana);   // object -> text
            string xml = writer.ToString();
            Console.WriteLine(xml);

            StringReader reader = new StringReader(xml);
            Student copy = (Student)serializer.Deserialize(reader); // text -> object
            Console.WriteLine($"{copy.Name}, {copy.Group}, year {copy.Year}");

            return Task.CompletedTask;
        }
    }
}
