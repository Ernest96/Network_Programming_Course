namespace Module5
{
    // The object every example sends, saves and reads back.
    // Public class, public properties, a parameterless constructor:
    // that is all JsonConvert and XmlSerializer need.
    public class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public int Year { get; set; }
    }
}
