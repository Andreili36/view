namespace View.Modules.PlatformModule.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string LastName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public DateTime BirthDate { get; set; } = DateTime.Today;
        public string Gender { get; set; } = "м";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime HireDate { get; set; } = DateTime.Today;
        public string Specialization { get; set; } = "";
        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    }
}