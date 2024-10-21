

namespace Server.Entities;

public class FormField
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Placeholder { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool Required { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Default_value { get; set; } = string.Empty;
    public string[] Default_values { get; set; } = { };
    public string[] Lookup_items { get; set; } = { };
    public int DisplayLength { get; set; }
}