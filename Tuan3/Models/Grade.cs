namespace Tuan3.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public string GradeName { get; set;}
        public IList<Student> Students { get; set; }
    }
}
