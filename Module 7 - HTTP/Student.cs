namespace Module7
{
    // A student as both servers send it. Id is a string: mockapi.io sends "3", our server sends 3 -
    // Newtonsoft reads both.
    public class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public int Year { get; set; }

        public Student(string name, string group, int year)
        {
            Name = name;
            Group = group;
            Year = year;
        }
    }
}
