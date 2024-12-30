## Seed Data

Data can be associated with an entity type as part of the model configuration.

The EF Core migrations can automatically compute what insert, update or delete operations need to be applied when
upgrading the database to a new version of the model.

```csharp
modelBuilder.Entity<Country>(b =>
{
    b.Property(x => x.Name).IsRequired();
    b.HasData(
        new Country { CountryId = 1, Name = "USA" },
        new Country { CountryId = 2, Name = "Canada" },
        new Country { CountryId = 3, Name = "Mexico" });
});
```
