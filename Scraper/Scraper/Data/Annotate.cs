public class FieldAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
{
    public string Name { get; set;} = "Unknown";
    public bool AutoIncrement { get; set; } = false;
}

public class TableAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
{
    public string Name { get; set;} = "Unknown";
}