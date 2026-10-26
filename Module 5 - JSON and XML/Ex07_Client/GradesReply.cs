namespace Ex07_Client
{
    // What the server answers:
    //   {"Name":"Ana","Average":8.67,"Passed":true}
    public class GradesReply
    {
        public string Name { get; set; }
        public double Average { get; set; }
        public bool Passed { get; set; }
    }
}
