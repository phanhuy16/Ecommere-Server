

namespace Server.Entities;
public class Promotion
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Value { get; set; }
    public int NumOfAvailable { get; set; } = 100;
    public string Type { get; set; } = "discount";
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public string? ImageURL { get; set; } = string.Empty;
}
