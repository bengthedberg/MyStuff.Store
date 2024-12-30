using System.Text.Json.Serialization;

namespace MyStuff.Stores.API.Models;

public class Store
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime OpenDate { get; set; }
    public string? Phone { get; set; }

    public Person? Owner { get; set; }
    public ICollection<Person> Staff { get; set; } 
    
    [JsonIgnore]
    public DateTime CreatedDate { get;  set; }
    public Service? Service { get; set; }
    public int ServiceId { get; set; }

    public bool IsArchived { get; set; }
}