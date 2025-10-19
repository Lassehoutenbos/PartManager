using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartManager.Api.Data;
using PartManager.Api.Models;

namespace PartManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PartsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PartsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Part>>> GetParts()
    {
        return await _context.Parts
            .Include(p => p.Drawer)
            .Include(p => p.Category)
            .Include(p => p.Attachments)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Part>> GetPart(int id)
    {
        var part = await _context.Parts
            .Include(p => p.Drawer)
            .Include(p => p.Category)
            .Include(p => p.Attachments)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (part == null)
        {
            return NotFound();
        }

        return part;
    }

    [HttpPost]
    public async Task<ActionResult<Part>> CreatePart(Part part)
    {
        part.CreatedAt = DateTime.UtcNow;
        part.UpdatedAt = DateTime.UtcNow;
        
        _context.Parts.Add(part);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPart), new { id = part.Id }, part);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePart(int id, Part part)
    {
        if (id != part.Id)
        {
            return BadRequest();
        }

        var existingPart = await _context.Parts.FindAsync(id);
        if (existingPart == null)
        {
            return NotFound();
        }

        existingPart.Name = part.Name;
        existingPart.Description = part.Description;
        existingPart.PartNumber = part.PartNumber;
        existingPart.Manufacturer = part.Manufacturer;
        existingPart.ManufacturerPartNumber = part.ManufacturerPartNumber;
        existingPart.Package = part.Package;
        existingPart.Footprint = part.Footprint;
        existingPart.Value = part.Value;
        existingPart.Tolerance = part.Tolerance;
        existingPart.Voltage = part.Voltage;
        existingPart.Current = part.Current;
        existingPart.Power = part.Power;
        existingPart.Temperature = part.Temperature;
        existingPart.Notes = part.Notes;
        existingPart.Quantity = part.Quantity;
        existingPart.MinQuantity = part.MinQuantity;
        existingPart.DrawerId = part.DrawerId;
        existingPart.CategoryId = part.CategoryId;
        existingPart.NfcTagId = part.NfcTagId;
        existingPart.QrCode = part.QrCode;
        existingPart.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await PartExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePart(int id)
    {
        var part = await _context.Parts.FindAsync(id);
        if (part == null)
        {
            return NotFound();
        }

        _context.Parts.Remove(part);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> PartExists(int id)
    {
        return await _context.Parts.AnyAsync(e => e.Id == id);
    }
}
