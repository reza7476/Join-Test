namespace Join_script.Entities;
public class Equipment
{
    public Equipment()
    {
        Options = new HashSet<PropertyOptions>();
    }
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public HashSet<PropertyOptions> Options { get; set; }
}
