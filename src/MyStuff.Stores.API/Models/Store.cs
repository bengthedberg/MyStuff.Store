namespace MyStuff.Stores.API.Models;

public class Store
{
    public int Id { get; set; }
    public string? Name { get; set; }    
    public DateTime OpenDate { get; set; }
    public string? Phone { get; set; }
}

public class StoreName
{
    public int Id  { get; set; }
    public string? Name { get; set; }
}