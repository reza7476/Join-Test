// See https://aka.ms/new-console-template for more information
using Join_script.Data;
using Join_script.Services;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");


var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer("Server=.;Database=Test-Join;Trusted_Connection=True;TrustServerCertificate=True;")
    .Options;

using var context = new AppDbContext(options);

var service = new PropertyService(context);

await service.Run();