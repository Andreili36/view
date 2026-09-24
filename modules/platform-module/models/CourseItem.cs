namespace View.Modules.PlatformModule.Models
{
    public class CourseItem
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public string ProgramName { get; set; } = "";
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today.AddMonths(2);
        public int? TeacherId { get; set; }
        public string TeacherName { get; set; } = "";
        public int MaxStudents { get; set; } = 20;
        public int Enrolled { get; set; }
    }
}