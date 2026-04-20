namespace ftpms.Models;

public class Project : BaseEntity
{
    public int CustomerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "Planned";
    public DateTime? DueDateUtc { get; set; }
}
