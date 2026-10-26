namespace Ex07_Client
{
    // The client's own copy of Student: same names as the server's, so the same JSON.
    public class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public int Year { get; set; }
    }
}
