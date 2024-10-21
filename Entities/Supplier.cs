using System.ComponentModel.DataAnnotations;

namespace Server.Entities;

public class Supplier
{
    [Required]
    public Guid id { get; set; }
    public Guid? category_id { get; set; }
    public string name { get; set; } = string.Empty;
    public string slug { get; set; } = string.Empty;
    public string product { get; set; } = string.Empty;
    public float price { get; set; }
    public string contact { get; set; } = string.Empty;
    public bool? isTalking { get; set; } = false;
    public string? email { get; set; } = string.Empty;
    public int active { get; set; }
    public string? photoUrl { get; set; } = string.Empty;
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public Category? category { get; set; } = null;
}