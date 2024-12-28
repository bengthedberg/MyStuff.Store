using Microsoft.EntityFrameworkCore;
using MyStuff.Stores.API.Models;

namespace MyStuff.Stores.API.Data;

public class StoresContext : DbContext
{
    // Use the Set method to get a non-nullable DbSet
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Service> Services => Set<Service>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID=postgres;Password=postgrespw;Host=localhost;Port=5432;Database=stores_db;Pooling=true;");
        //  Not proper logging, use for development to see 
        optionsBuilder.LogTo(Console.WriteLine);
        base.OnConfiguring(optionsBuilder);
    }
}