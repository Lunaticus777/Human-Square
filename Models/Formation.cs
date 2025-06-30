namespace Human_Evolution.Models
{
    public class Formation
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
    }
}
