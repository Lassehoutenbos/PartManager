using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartManager.Api.Data;
using PartManager.Api.Models;

namespace PartManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrawersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DrawersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Drawer>>> GetDrawers()
    {
        return await _context.Drawers
            .Include(d => d.Parts)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Drawer>> GetDrawer(int id)
    {
        var drawer = await _context.Drawers
            .Include(d => d.Parts)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (drawer == null)
        {
            return NotFound();
        }

        return drawer;
    }

    [HttpPost]
    public async Task<ActionResult<Drawer>> CreateDrawer(Drawer drawer)
    {
        _context.Drawers.Add(drawer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDrawer), new { id = drawer.Id }, drawer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDrawer(int id, Drawer drawer)
    {
        if (id != drawer.Id)
        {
            return BadRequest();
        }

        _context.Entry(drawer).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await DrawerExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDrawer(int id)
    {
        var drawer = await _context.Drawers.FindAsync(id);
        if (drawer == null)
        {
            return NotFound();
        }

        _context.Drawers.Remove(drawer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> DrawerExists(int id)
    {
        return await _context.Drawers.AnyAsync(e => e.Id == id);
    }
}
