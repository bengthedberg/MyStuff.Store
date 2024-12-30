using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyStuff.Stores.API.Models;

namespace MyStuff.Stores.API.Data.EntityMapping;

public class ServiceMapping : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder
            .ToTable("StoreServices");
        builder
            .Property(s => s.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        // Save the service type as string
        // Hint, be careful if you use this property for comparision other than equal.
        // As in the code ServieType is of type Int but in database it is of type string,
        builder
            .Property(s => s.Type)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasData(
            new Service { Id = 1, Name = "Catering", Type = ServiceType.Pickup}
        );

    }
}