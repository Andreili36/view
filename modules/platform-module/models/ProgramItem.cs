namespace View.Modules.MainModule.Models
{
    public class ProgramItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int DurationHours { get; set; }
        public string Category { get; set; } = "";
    }
}