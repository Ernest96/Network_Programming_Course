using System.Collections.Generic;

namespace Ex07_Client
{
    // What the client sends:
    //   {"Student":{"Name":"Ana","Group":"TI-231","Year":3},"Grades":[9,10,7]}
    public class GradesRequest
    {
        public Student Student { get; set; }            // a nested object
        public List<int> Grades { get; set; }           // an array
    }
}
