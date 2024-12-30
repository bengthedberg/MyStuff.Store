using System.Text.Json.Serialization;

namespace MyStuff.Stores.API.Models;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ServiceType Type { get; set; }
    
    // Store property should not be exposed externally, use DTO.
    [JsonIgnore] // to solve object cylce json exception 
    public ICollection<Store> Stores { get; set; } = new HashSet<Store>();
}