using System.Collections.Generic;

namespace Module5;

public class GradesRequest
{
    public Student Student { get; set; } 
    public List<int> Grades { get; set; }
}