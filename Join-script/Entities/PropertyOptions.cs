namespace Join_script.Entities;
public class PropertyOptions
{
    public long Id { get; set; }
    public long PropertyId { get; set; }
    public long EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = default!;
    public Property Property { get; set; }=default!;
}
