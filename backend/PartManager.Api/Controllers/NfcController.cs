using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartManager.Api.Data;

namespace PartManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NfcController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public NfcController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("scan/{tagId}")]
    public async Task<ActionResult<object>> ScanTag(string tagId)
    {
        // Check if it's a drawer
        var drawer = await _context.Drawers
            .Include(d => d.Parts)
            .FirstOrDefaultAsync(d => d.NfcTagId == tagId);
        
        if (drawer != null)
        {
            return Ok(new
            {
                type = "drawer",
                data = drawer
            });
        }

        // Check if it's a part
        var part = await _context.Parts
            .Include(p => p.Drawer)
            .Include(p => p.Category)
            .Include(p => p.Attachments)
            .FirstOrDefaultAsync(p => p.NfcTagId == tagId);
        
        if (part != null)
        {
            return Ok(new
            {
                type = "part",
                data = part
            });
        }

        return NotFound(new { message = "NFC tag not found" });
    }

    [HttpPost("write/drawer/{drawerId}")]
    public async Task<ActionResult> WriteDrawerTag(int drawerId, [FromBody] WriteTagRequest request)
    {
        var drawer = await _context.Drawers.FindAsync(drawerId);
        if (drawer == null)
        {
            return NotFound();
        }

        drawer.NfcTagId = request.TagId;
        await _context.SaveChangesAsync();

        return Ok(new { message = "NFC tag assigned to drawer" });
    }

    [HttpPost("write/part/{partId}")]
    public async Task<ActionResult> WritePartTag(int partId, [FromBody] WriteTagRequest request)
    {
        var part = await _context.Parts.FindAsync(partId);
        if (part == null)
        {
            return NotFound();
        }

        part.NfcTagId = request.TagId;
        await _context.SaveChangesAsync();

        return Ok(new { message = "NFC tag assigned to part" });
    }
}

public class WriteTagRequest
{
    public string TagId { get; set; } = string.Empty;
}
