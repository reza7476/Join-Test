using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Join_script.Entities;
public class Property
{

    public Property()
    {
        Options = new HashSet<PropertyOptions>();
    }

    public long Id { get; set; }
    public int Size { get; set; }
    public int PricePerMeter { get; set; }
    public string Location { get; set; } = default!;
    public int RegisterNumber { get; set; }
    public string Owner { get; set; } = default!;
   
    public long? TypeId { get; set; }
    public PropertyType PropertyType { get; set; } = default!;
    public HashSet<PropertyOptions> Options { get; set; }
}
