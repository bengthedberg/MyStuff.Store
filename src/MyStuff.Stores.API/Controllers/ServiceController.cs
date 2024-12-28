using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyStuff.Stores.API.Data;
using MyStuff.Stores.API.Models;

namespace MyStuff.Stores.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceController : Controller
{
    private readonly StoresContext _context;

    public ServiceController(StoresContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<Service>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Services.ToListAsync<Service>());
    }
    
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Service), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var service = await _context.Services.FindAsync(id);

        return service is null
            ? NotFound()
            : Ok(service);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Service), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] Service service)
    {
        await _context.Services.AddAsync(service);
        
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(Get), new { id = service.Id }, service);  
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Service), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Service service)
    {
        var existingService = await _context.Services.FindAsync(id);
        
        if (existingService is null)
        {
            return NotFound();
        }
        existingService.Name = service.Name;

        await _context.SaveChangesAsync();

        return Ok(existingService);
        
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Remove([FromRoute] int id)
    {
        _context.Remove(new Service() { Id = id} );
        
        await _context.SaveChangesAsync();
        
        return Ok();
    }

    
}