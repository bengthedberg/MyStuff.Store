## Dependency Injection

ASP.NET Core applications are configured using dependency injection. EF Core can be added to this configuration using AddDbContext in Program.cs. For example:

```csharp
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>({ options =>
        options
            // .LogTo(Console.WriteLine)
            .UseSqlServer(connectionString);
    },
    ServiceLifetime.Scoped,
    ServiceLifetime.Singleton);
```

Add a ConnectionStrings setting in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  }
}
```

The preceding code registers ApplicationDbContext, a subclass of DbContext, as a scoped service in the ASP.NET Core app service provider.

The ApplicationDbContext class must expose a public constructor with a DbContextOptions<ApplicationDbContext> parameter. This is how context configuration from AddDbContext is passed to the DbContext. For example:

```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
```

ApplicationDbContext can be used in ASP.NET Core controllers or other services through constructor injection:

```csharp
public class MyController
{
    private readonly ApplicationDbContext _context;

    public MyController(ApplicationDbContext context)
    {
        _context = context;
    }
}
```

The final result is an ApplicationDbContext instance created for each request and passed to the controller to perform a unit-of-work before being disposed when the request ends.
