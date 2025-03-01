namespace Domain.Data;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = "Default";
    public string? Description { get; set; }
    public bool IsFinished { get; set; } = false;

    public ICollection<BomItem> BomItems { get; set; } = [];

    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }
}