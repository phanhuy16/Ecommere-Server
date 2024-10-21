
using Server.Entities;

namespace Server.Utilities.Form;

public static class Forms
{
    public static FormField Name => new FormField
    {
        Key = "name",
        Value = "name",
        Label = "Supplier name",
        Placeholder = "Enter supplier name",
        Type = "default",
        Required = true,
        Message = "Enter supplier name",
        Default_value = "",
        DisplayLength = 300
    };

    public static FormField Email => new FormField
    {
        Key = "email",
        Value = "email",
        Label = "Supplier email",
        Placeholder = "Enter supplier email",
        Type = "default",
        Default_value = "",
        DisplayLength = 150
    };

    public static FormField Active => new FormField
    {
        Key = "active",
        Value = "active",
        Label = "Supplier active",
        Placeholder = "Enter supplier active number",
        Type = "number",
        Default_value = "",
        DisplayLength = 150
    };

    public static FormField Product => new FormField
    {
        Key = "product",
        Value = "product",
        Label = "Supplier product",
        Placeholder = "Enter supplier product",
        Type = "default",
        Message = "Enter supplier product",
        Default_value = "",
        DisplayLength = 150
    };

    public static FormField Category => new FormField
    {
        Key = "categories",
        Value = "categories",
        Label = "Categories",
        Placeholder = "Enter supplier category",
        Default_values = [],
        Type = "select",
        Message = "",
        Lookup_items = [],
        DisplayLength = 150
    };

    public static FormField Price => new FormField
    {
        Key = "price",
        Value = "price",
        Label = "Buying price",
        Placeholder = "Enter Buying price",
        Type = "number",
        Message = "",
        Default_value = "",
        DisplayLength = 150
    };

    public static FormField Contact => new FormField
    {
        Key = "contact",
        Value = "contact",
        Label = "Supplier number",
        Placeholder = "Enter supplier number",
        Type = "tel",
        Message = "",
        Default_value = "",
        DisplayLength = 150
    };

    public static FormField Type => new FormField
    {
        Key = "type",
        Value = "isTaking",
        Label = "Taking",
        Placeholder = "Enter Buying price",
        Type = "checkbox",
        Message = "",
        Default_value = null!,
        DisplayLength = 150
    };


    public static List<FormField> GetFormFields()
    {
        return new List<FormField>
        {
            Name,
            Email,
            Active,
            Product,
            Category,
            Price,
            Contact,
            Type
        };
    }
}
