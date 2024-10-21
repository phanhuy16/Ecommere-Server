
using Server.Entities;

namespace Server.Utilities.Form;


public static class FormProduct
{
    public static FormField Title => new FormField
    {
        Key = "title",
        Value = "title",
        Label = "Products",
        Placeholder = "Enter product name",
        Type = "default",
        Required = true,
        Message = "Enter product name",
        Default_value = "",
        DisplayLength = 300
    };

    public static FormField Price => new FormField
    {
        Key = "price",
        Value = "price",
        Label = "Price",
        Placeholder = "",
        Type = "number",
        Message = "Enter price",
        Default_value = "",
        DisplayLength = 200
    };
}