using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyStuff.Stores.API.Data.ValueConverters;
using MyStuff.Stores.API.Data.ValueGenerators;
using MyStuff.Stores.API.Models;

namespace MyStuff.Stores.API.Data.EntityMapping;

public class StoreMapping : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder
            .HasQueryFilter(s => s.IsArchived == false)
            .HasIndex(s => s.OpenDate); // Add another index
            
        builder
            .Property(s => s.Name)
            .HasColumnName("StoreName") // Change the name of the column
            .HasMaxLength(100) // Set Max length
            .IsRequired(); // Make column not null
        builder
            .Property(s => s.OpenDate)
            //.HasColumnType("date"); // Change the type
            .HasColumnType("varchar(23)")
            //.HasConversion<string>();
            .HasConversion(new DateTimeToChar8Converter());
        
        builder
            .Property(s => s.Phone)
            .HasMaxLength(20);

        // ComplexProperty is a simple grouping of properties 
        // builder
        //     .ComplexProperty(s => s.Owner);
        // OwnsOne is simpilar to ComplexProperty but it allows us to move thwo whole
        // group into its own table.
        builder
            .OwnsOne(s => s.Owner)
            .ToTable("StoreOwner");
            
        builder
            .OwnsMany(s => s.Staff)
            .ToTable("StoreStaff");

        builder.Property(s => s.CreatedDate)
            //.HasDefaultValueSql("now() at time zone 'utc'");
            .HasValueGenerator<CreatedDateGenerator>();  // Thos will not run for seeded data as it needs a context.
        
        builder.Property(s => s.IsArchived)
            .HasDefaultValue(false);
        
        builder.HasData(
            new Store { Id = 1, Name = "Main Store", OpenDate = new DateTime(2020, 07, 15), Phone = "(07) 1234 5678", ServiceId = 1 },
            new Store { Id = 2, Name = "Archived Store", OpenDate = new DateTime(2020, 07, 15), Phone = "(07) 1234 5678", ServiceId = 1, IsArchived = true}
        );
        builder.OwnsOne(s => s.Owner)
            .HasData(new { StoreId = 1, FirstName = "John", LastName = "Owner" });
        builder.OwnsMany(s => s.Staff)
            .HasData(
                new { StoreId = 1, Id = 1, FirstName = "Lisa", LastName = "Staff" },
                new { StoreId = 1, Id = 2, FirstName = "Peter", LastName = "Staff" }
            );
        
    }
    
}