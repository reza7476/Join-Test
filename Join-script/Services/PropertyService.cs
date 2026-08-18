using Join_script.Data;
using Join_script.Entities;
using Microsoft.EntityFrameworkCore;

namespace Join_script.Services;
public class PropertyService
{
    private readonly AppDbContext _context;


    public PropertyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task Run()
    {

        var run = true;
        while (run)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            await Console.Out.WriteLineAsync("" +
                "0. Exit\n" +
                "1. Add Property Types\n" +
                "2. Display property types\n" +
                "3. Add Properties\n" +
                "4. Add Equipments\n" +
                "5. Display properties\n" +
                "6. Display equipments\n" +
                "7. Assign option to  property \n" +
                "8. Display square\n" +
                "9. Display properties which have type.\n " +
                "10. Display properties whether have or not have type\n" +
                "11 Display property owner and its equipment\n" +
                "12. Display all property and their equipment"
                );
            Console.ResetColor();
            var a = Console.ReadLine();
            switch (a)
            {

                case "0":
                    run = false; break;
                case "1":
                    await AddPropertyType();
                    break;
                case "2":
                    await DisplayPropertyType();
                    break;
                case "3":
                    await AddProperty();
                    break;
                case "4":
                    await AddEquipment();
                    break;
                case "5":
                    await DisplayProperties();
                    break;
                case "6":
                    await DisplayEquipment();
                    break;
                case "7":
                    await AssignOPtionToProperty();
                    break;
                case "8":
                    Square();
                    break;

                case "9":
                    await DisplayPropertiesWhichHaveType();
                    break;
                case "10":
                    await DisplayPropertiesWhereHaveTypeOrNotHave();
                    break;

                case "11":
                    await DisplayPropertyOwnerAndItsEquipmentTitle();
                    break;

                case "12":
                    await DisplayAllPropertyAndTheirOwnerAndTheirEquipment();
                    break;
                default:
                    break;
            }
        }
    }

    public async Task DisplayAllPropertyAndTheirOwnerAndTheirEquipment()
    {
        var query = await (from pr in _context.Properties.DefaultIfEmpty()
                           join option in _context.PropertyOptions
                           on pr.Id equals option.PropertyId
                           join eq in _context.Equipments
                           on option.EquipmentId equals eq.Id
                           select new PropertyOptionsNameDto()
                           {
                               Id = pr.Id,
                               Owner = pr.Owner,
                               Location = pr.Location,
                               Size = pr.Size,
                               OptionTitle = new List<string>()
                                {
                                    eq.Title
                                }
                           }
                           ).ToListAsync();



        var qu = await (from pr in _context.Properties
                        join option in _context.PropertyOptions
                        on pr.Id equals option.PropertyId
                             into options
                        from option in options.DefaultIfEmpty()
                        join eq in _context.Equipments
                        on option.EquipmentId equals eq.Id
                        select new PropertyOptionsNameDto()
                        {
                            Id = pr.Id,
                            Location = pr.Location,
                            Owner = pr.Owner,
                            Size = pr.Size,
                            OptionTitle = new List<string>()
                        }).ToListAsync();

        var quop = await (from eq in _context.Equipments
                          join option in _context.PropertyOptions
                            on eq.Id equals option.EquipmentId
                            into options
                          from option in options.DefaultIfEmpty()
                          join pr in _context.Properties
                            on option.PropertyId equals pr.Id
                            into properties
                          from property in properties.DefaultIfEmpty()
                          select new OptionNameWithPropertyDto()
                          {
                              Id = eq.Id,
                              EquipmentTitle = eq.Title,
                              Location = property != null ? property.Location : null,
                              Owner = property != null ? property.Owner : null,
                              Size = property != null ? property.Size : 0
                          }).ToListAsync();

        var qupr = await (from pr in _context.Properties
                          join type in _context.PropertyTypes
                          on pr.TypeId equals type.Id
                          into types
                          from type in types.DefaultIfEmpty()
                          select new PropertiesDto()
                          {
                              Id = pr.Id,
                              Location = pr.Location,
                              Owner = pr.Owner,
                              PricePerMeter = pr.PricePerMeter,
                              RegisterNumber = pr.RegisterNumber,
                              Size = pr.Size
                          }).ToListAsync();

        var query1 = await (from pr in _context.Properties
                            join opr in _context.PropertyOptions
                                  on pr.Id equals opr.PropertyId
                                  into options
                            from option in options.DefaultIfEmpty()

                            join eq in _context.Equipments
                               on option.EquipmentId equals eq.Id
                               into equipments

                            from equipment in equipments.DefaultIfEmpty()
                            join types in _context.PropertyTypes
                                on pr.TypeId equals types.Id
                                into types

                            from pro in types.DefaultIfEmpty()
                            select new
                            {
                                pr.Owner,
                                pr.Size,
                                pr.Location,
                                equipment.Title,
                                typeTitle = pro.Title
                            }).ToListAsync();



        var data = await (from pr in _context.Properties
                          join po in _context.PropertyOptions
                            on pr.Id equals po.PropertyId
                            into options
                          from option in options.DefaultIfEmpty()
                          join eq in _context.Equipments
                            on option.EquipmentId equals eq.Id
                            into equipmentGroup
                          from equipment in equipmentGroup.DefaultIfEmpty()
                          join type in _context.PropertyTypes
                            on pr.TypeId equals type.Id
                            into ptGroup
                          from type in ptGroup.DefaultIfEmpty()
                          select new
                          {
                             PropertyId = pr.Id,
                             pr.Owner,
                             pr.Location,
                             pr.Size,
                             TypeTitle = type.Title,
                             EquipmentId = equipment != null ? equipment.Id : 0,
                             EquipmentTitle = equipment != null ? equipment.Title : "null"
                          })
                          .GroupBy(x => new
                          {
                              x.PropertyId,
                              x.Location,
                              x.Owner,
                              x.Size,
                              x.TypeTitle
                          })
                          .Select(g => new resultDto()
                          {
                              Owner = g.Key.Owner,
                              Location = g.Key.Location,
                              Size = g.Key.Size,
                              TypeTitle = g.Key.TypeTitle,
                              Equipments = g
                              .Where(x => x.EquipmentId != 0)
                              .Select(_ => new EquipmentDto
                              {
                                  Id = _.EquipmentId,
                                  Title = _.EquipmentTitle
                              }).ToList()
                          })

                          .ToListAsync();
        foreach (var item in data)
        {
            await Console.Out.WriteLineAsync($" {item.Owner} | {item.Location}" +
                $" {item.TypeTitle}  | {item.Size}  |" +
                $" {item.Equipments.Count} ");
        }

        



        var c = 1;
    }

    public class resultDto()
    {
        public string? Owner { get; set; }
        public string? Location { get; set; }
        public int Size { get; set; }
        public string? TypeTitle { get; set; }
        public List<EquipmentDto> Equipments { get; set; } = new();
    }

    public class EquipmentDto()
    {
        public long Id { get; set; }
        public string? Title { get; set; }
    }

    public class OptionNameWithPropertyDto()
    {
        public long Id { get; set; }
        public string? EquipmentTitle { get; set; }
        public string? Owner { get; set; }
        public string? Location { get; set; }
        public int Size { get; set; }
    }


    public async Task DisplayPropertyOwnerAndItsEquipmentTitle()
    {
        var query = await (from pr in _context.Properties



                           join opr in _context.PropertyOptions
                           on pr.Id equals opr.PropertyId
                           join eq in _context.Equipments
                           on opr.EquipmentId equals eq.Id
                           select new PropertyOptionsNameDto()
                           {
                               Owner = pr.Owner,
                               Location = pr.Location,
                               Size = pr.Size,
                               OptionTitle = new List<string>()
                                {
                                    eq.Title
                                }
                           }
                           ).ToListAsync();


        foreach (var item in query)
        {
            Console.WriteLine($"|   {item.Location}   |    {item.Owner}   |    {item.Size}    |    {item.OptionTitle}    ");
            Console.Out.WriteLine(" --------------");
        }
        Console.ResetColor();

        var a = 1;
    }

    public class PropertyOptionsNameDto
    {
        public long Id { get; set; }
        public string? Owner { get; set; }
        public string? Location { get; set; }
        public int Size { get; set; }
        public List<string> OptionTitle { get; set; } = new();
    }



    public async Task DisplayPropertiesWhichHaveType()
    {
        var query = await (from pr in _context.Properties
                           join type in _context.PropertyTypes
                           on pr.TypeId equals type.Id
                           select new PropertiesWhichHaveTypeDto()
                           {
                               Location = pr.Location,
                               Owner = pr.Owner,
                               Size = pr.Size,
                               TypeTitle = type.Title
                           }
                    ).ToListAsync();
        Console.ForegroundColor = ConsoleColor.Green;
        foreach (var item in query)
        {
            Console.WriteLine($"|   {item.Location}   |    {item.Owner}   |    {item.Size}    |    {item.TypeTitle}    ");
            Console.Out.WriteLine(" --------------");
        }
        Console.ResetColor();

    }

    public async Task DisplayPropertiesWhereHaveTypeOrNotHave()
    {
        var query = await (from pr in _context.Properties //DefaultIfEmpty(
                           join type in _context.PropertyTypes
                           on pr.TypeId equals type.Id
                           into propertyTypes
                           from type in propertyTypes.DefaultIfEmpty()
                           select new PropertiesWhichHaveTypeDto()
                           {
                               Location = pr.Location,
                               Owner = pr.Owner,
                               Size = pr.Size,
                               TypeTitle = type != null ? type.Title : "null"
                           }
                    ).ToListAsync();
        Console.ForegroundColor = ConsoleColor.Green;
        foreach (var item in query)
        {
            Console.WriteLine($"|   {item.Location}   |    {item.Owner}   |    {item.Size}    |    {item.TypeTitle}    ");
            Console.Out.WriteLine(" --------------");
        }
        Console.ResetColor();
    }



    public class PropertiesWhichHaveTypeDto()
    {
        public string? Owner { get; set; }
        public int Size { get; set; }
        public string? TypeTitle { get; set; }
        public string? Location { get; set; }
    }

    public async Task AddPropertyType()
    {
        await Console.Out.WriteLineAsync("Enter property type title");
        var title = Console.ReadLine();
        var types = new List<PropertyType>();
        if (title != null)
        {
            var pType = new PropertyType()
            {
                Title = title
            };
            types.Add(pType);
        }

        await _context.PropertyTypes.AddRangeAsync(types);
        await _context.SaveChangesAsync();
    }
    public async Task DisplayPropertyType()
    {
        var types = _context.PropertyTypes.Select(_ => new PropertyTypeListDto()
        {
            Id = _.Id,
            Title = _.Title
        }).ToList();

        Console.WriteLine(" ---id---------title-");
        Console.ForegroundColor = ConsoleColor.Green;
        foreach (var type in types)
        {
            Console.WriteLine($"|   {type.Id}   |    {type.Title}   |");
            Console.Out.WriteLine(" --------------");
            //await Console.Out.WriteLineAsync($" Id: {type.Id} -- Title: {type.Title}");
        }
        Console.ResetColor();
    }


    public async Task DisplayProperties()
    {
        var properties = await _context.Properties.Select(_ => new PropertiesDto()
        {
            Id = _.Id,
            Location = _.Location,
            Owner = _.Owner,
            PricePerMeter = _.PricePerMeter,
            RegisterNumber = _.RegisterNumber,
            Size = _.Size,
            TypeTitle = _.PropertyType.Title
        }).ToListAsync();

        await Console.Out.WriteLineAsync("|  ID   |  SIZE    |    LOCATION    |   REGISTER NUMBER  |  OWNER   |  TYPE TITLE");
        foreach (var property in properties)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"|   {property.Id}   |    {property.Size}   |    {property.Location}    |    {property.RegisterNumber}    |     {property.Owner}    |     {property.TypeTitle}     ");
            await Console.Out.WriteLineAsync("------------------------------------------------------------------------------------------------------------------------");
        }

        Console.ResetColor();
    }

    public class PropertiesDto
    {
        public long Id { get; set; }
        public int Size { get; set; }
        public int PricePerMeter { get; set; }
        public int RegisterNumber { get; set; }
        public string Location { get; set; } = default!;
        public string Owner { get; set; } = default!;
        public string TypeTitle { get; set; } = default!;

    }





    public void Square()
    {
        Console.WriteLine(" --------------");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine("|      |       |");
            Console.Out.WriteLine(" --------------");
        }
    }

    public async Task AddEquipment()
    {
        Console.WriteLine("enter equipment");
        var title = Console.ReadLine();

        var equipment = new Equipment()
        {
            Title = title!,
        };
        await _context.Equipments.AddAsync(equipment);
        await _context.SaveChangesAsync();
    }
    public async Task AddProperty()
    {
        await Console.Out.WriteLineAsync("enter property location.");
        var location = Console.ReadLine();
        await Console.Out.WriteLineAsync("enter property price per meter.");
        var priceMeter = int.Parse(Console.ReadLine()!);
        await Console.Out.WriteLineAsync("enter property size.");
        var size = int.Parse(Console.ReadLine()!);
        await Console.Out.WriteLineAsync("enter property register number.");
        var regNum = int.Parse(Console.ReadLine()!);
        await Console.Out.WriteLineAsync("enter property owner.");
        var owner = Console.ReadLine();
        await Console.Out.WriteLineAsync("enter property type.");
        var typeId = Console.ReadLine();
        var property = new Property()
        {
            Location = location!,
            Owner = owner!,
            PricePerMeter = priceMeter,
            RegisterNumber = regNum,
            Size = size,
            TypeId = typeId != "" ? int.Parse(typeId) : null
        };
        await _context.AddAsync(property);
        await _context.SaveChangesAsync();
    }

    public async Task DisplayEquipment()
    {
        var equipments = await _context.Equipments.Select(_ => new DisplayEquipmentDto()
        {
            Id = _.Id,
            Title = _.Title
        }).ToListAsync();
        Console.WriteLine(" ---id---------title-");
        Console.ForegroundColor = ConsoleColor.Green;
        foreach (var option in equipments)
        {
            Console.WriteLine($"|   {option.Id}   |    {option.Title}   |");
            Console.Out.WriteLine(" --------------");
            //await Console.Out.WriteLineAsync($" Id: {type.Id} -- Title: {type.Title}");
        }
        Console.ResetColor();

    }

    public async Task AssignOPtionToProperty()
    {
        await Console.Out.WriteLineAsync("enter equipment id");
        var eqId = long.Parse(Console.ReadLine()!);
        await Console.Out.WriteLineAsync("enter property id");
        var prId = long.Parse(Console.ReadLine()!);

        var equipmentOption = new PropertyOptions()
        {
            EquipmentId = eqId,
            PropertyId = prId
        };

        await _context.PropertyOptions.AddAsync(equipmentOption);
        await _context.SaveChangesAsync();
    }
    public class DisplayEquipmentDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = default!;
    }
    public class PropertyTypeListDto()
    {
        public long Id { get; set; }
        public string Title { get; set; } = default!;
    }
}

