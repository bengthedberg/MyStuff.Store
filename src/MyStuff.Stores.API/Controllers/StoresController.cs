using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyStuff.Stores.API.Data;
using MyStuff.Stores.API.Models;

namespace MyStuff.Stores.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StoresController : Controller
{
    private readonly StoresContext _context;

    public StoresController(StoresContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<Stores.API.Models.Store>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
       return Ok(await _context.Stores.ToListAsync<Store>());
    }
    
    [HttpGet("names")]
    [ProducesResponseType(typeof(List<Stores.API.Models.StoreName>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllStoreNames()
    {
        // Project data into a different set, (will only select required columns)
        return Ok(await _context.Stores
            .Select(store => new StoreName{ Id = store.Id, Name = store.Name})
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Stores.API.Models.Store), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        // Always query database, and return first row found or null if doesnt exist.
        // var store = await _context.Stores.FirstOrDefaultAsync(x => x.Id == id);
        // Similar to FirstOrDefaultAsync but will throw exception if multiple rows matched.
        // var store = await _context.Stores.SingleOrDefaultAsync(x => x.Id == id);
        
        // Same as above except will not query database if row already exists in db context.
        var store = await _context.Stores
            .Include(s => s.Service)
            .SingleOrDefaultAsync(x => x.Id == id);

        return store is null
            ? NotFound()
            : Ok(store);
    }
    
    
    [HttpGet("by-year/{year:int}")]
    [ProducesResponseType(typeof(List<Stores.API.Models.Store>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByYear([FromRoute] int year)
    {
        // filter data using where
        var filteredStores = await _context.Stores
            .Where(x => x.OpenDate.Year == year)
            .ToListAsync();
        return Ok(filteredStores);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Stores.API.Models.Store), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] Stores.API.Models.Store store)
    {
        await _context.Stores.AddAsync(store);
        
        // Store has no Id
        await _context.SaveChangesAsync();
        //  Store has Id
        
        return CreatedAtAction(nameof(Get), new { id = store.Id }, store);  
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Stores.API.Models.Store), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Stores.API.Models.Store store)
    {
        var existingStore = await _context.Stores.FindAsync(id);
        
        if (existingStore is null)
        {
            return NotFound();
        }
        existingStore.Name = store.Name;
        existingStore.OpenDate  = store.OpenDate;
        existingStore.Phone = store.Phone;

        await _context.SaveChangesAsync();

        return Ok(existingStore);
        
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int id)
    {
        var existingStore = await _context.Stores.FindAsync(id);
        
        if (existingStore is null)
        {
            return NotFound();
        }
        _context.Stores.Remove(existingStore);
        // You can also simplify the remove as the context know the type 
        // _context.Remove(existingStore);
        // If you dont need to return NotFound then you can save yourself a 
        //_context.Remove(new Store { Id = id} );
        
        await _context.SaveChangesAsync();
        
        return Ok();
    }
}