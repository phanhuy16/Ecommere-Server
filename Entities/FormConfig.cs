

namespace Server.Entities;

public class FormConfig
{
    public string Title { get; set; } = string.Empty;
    public string Layout { get; set; } = string.Empty;
    public int LabelCol { get; set; }
    public int WrapperCol { get; set; }
    public List<FormField> FormItem { get; set; }
}