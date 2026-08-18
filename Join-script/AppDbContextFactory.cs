// See https://aka.ms/new-console-template for more information
using Join_script.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionBuilder
            .UseSqlServer("Server=.;Database=Test-Join;Trusted_Connection=True;TrustServerCertificate=True;");
        return new AppDbContext(optionBuilder.Options);
    }
}