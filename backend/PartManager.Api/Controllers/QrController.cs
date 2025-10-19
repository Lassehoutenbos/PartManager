using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartManager.Api.Data;

namespace PartManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QrController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public QrController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("scan/{code}")]
    public async Task<ActionResult<object>> ScanCode(string code)
    {
        // Check if it's a drawer
        var drawer = await _context.Drawers
            .Include(d => d.Parts)
            .FirstOrDefaultAsync(d => d.QrCode == code);
        
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
            .FirstOrDefaultAsync(p => p.QrCode == code);
        
        if (part != null)
        {
            return Ok(new
            {
                type = "part",
                data = part
            });
        }

        return NotFound(new { message = "QR code not found" });
    }

    [HttpPost("generate/drawer/{drawerId}")]
    public async Task<ActionResult> GenerateDrawerCode(int drawerId)
    {
        var drawer = await _context.Drawers.FindAsync(drawerId);
        if (drawer == null)
        {
            return NotFound();
        }

        // Generate a unique QR code
        drawer.QrCode = $"DRAWER-{drawerId}-{Guid.NewGuid():N}";
        await _context.SaveChangesAsync();

        return Ok(new { qrCode = drawer.QrCode });
    }

    [HttpPost("generate/part/{partId}")]
    public async Task<ActionResult> GeneratePartCode(int partId)
    {
        var part = await _context.Parts.FindAsync(partId);
        if (part == null)
        {
            return NotFound();
        }

        // Generate a unique QR code
        part.QrCode = $"PART-{partId}-{Guid.NewGuid():N}";
        await _context.SaveChangesAsync();

        return Ok(new { qrCode = part.QrCode });
    }
}
