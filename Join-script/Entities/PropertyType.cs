namespace Join_script.Entities;
public class PropertyType
{
    public PropertyType()
    {
        Properties = new HashSet<Property>();    
    }

    public long  Id { get; set; }
    public string  Title{ get; set; } = default!;
    public HashSet<Property> Properties { get; set; }
}
