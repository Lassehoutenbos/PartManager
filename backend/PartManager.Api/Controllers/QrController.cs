using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartManager.Api.Data;

namespace PartManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class QrController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public QrController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Scan a QR code to find associated drawer or part
    /// </summary>
    /// <param name="code">The QR code to scan</param>
    /// <returns>Drawer or part information associated with the QR code</returns>
    /// <response code="200">Returns the drawer or part information</response>
    /// <response code="404">QR code not found</response>
    [HttpGet("scan/{code}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
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

    /// <summary>
    /// Generate a QR code for a specific drawer
    /// </summary>
    /// <param name="drawerId">The ID of the drawer</param>
    /// <returns>Generated QR code for the drawer</returns>
    /// <response code="200">Returns the generated QR code</response>
    /// <response code="404">Drawer not found</response>
    [HttpPost("generate/drawer/{drawerId}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
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

    /// <summary>
    /// Generate a QR code for a specific part
    /// </summary>
    /// <param name="partId">The ID of the part</param>
    /// <returns>Generated QR code for the part</returns>
    /// <response code="200">Returns the generated QR code</response>
    /// <response code="404">Part not found</response>
    [HttpPost("generate/part/{partId}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
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
