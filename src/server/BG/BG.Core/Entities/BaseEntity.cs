namespace BG.Core.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public bool Enabled { get; set; }
    public bool IsDeleted { get; set; }
    public int UserId { get; set; }
}