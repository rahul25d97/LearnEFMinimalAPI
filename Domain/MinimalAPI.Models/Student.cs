namespace MinimalAPI.Models
{
    public class Student
    {
        public int? StudentId { get; set; }
        public string? Name { get; set; }
        public string? Department { get; set; }
        public string? Address { get; set; }
        public string? ContactNo { get; set; }

    }

    public class vStudent
    {
        public int? StudentId { get; set; }
        public string? Name { get; set; }
        public string? Department { get; set; }
        public string? Address { get; set; }
        public string? ContactNo { get; set; }
        public int? NOR { get; set; }

    }
}
