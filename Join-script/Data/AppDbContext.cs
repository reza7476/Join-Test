using Join_script.Entities;
using Microsoft.EntityFrameworkCore;

namespace Join_script.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyType> PropertyTypes { get; set; }
    public DbSet<PropertyOptions> PropertyOptions { get; set; }
    public DbSet<Equipment> Equipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
