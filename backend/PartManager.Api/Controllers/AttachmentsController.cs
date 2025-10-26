using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartManager.Api.Data;
using PartManager.Api.Models;

namespace PartManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttachmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public AttachmentsController(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet("part/{partId}")]
    public async Task<ActionResult<IEnumerable<PartAttachment>>> GetPartAttachments(int partId)
    {
        var attachments = await _context.PartAttachments
            .Where(a => a.PartId == partId)
            .ToListAsync();
        
        return Ok(attachments);
    }

    /// <summary>
    /// Upload an attachment for a specific part (Form data)
    /// </summary>
    /// <param name="partId">The ID of the part</param>
    /// <returns>The created attachment</returns>
    [HttpPost("part/{partId}/upload")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(PartAttachment), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<PartAttachment>> UploadAttachment(int partId, IFormFile file, string? description = null, AttachmentType type = AttachmentType.Other)
    {
        var part = await _context.Parts.FindAsync(partId);
        if (part == null)
        {
            return NotFound();
        }

        if (file == null || file.Length == 0)
        {
            return BadRequest("No file provided");
        }

        // Create uploads directory if it doesn't exist
        var uploadsPath = Path.Combine(_environment.ContentRootPath, "uploads", "attachments");
        Directory.CreateDirectory(uploadsPath);

        // Generate unique filename
        var fileExtension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{partId}_{Guid.NewGuid():N}{fileExtension}";
        var filePath = Path.Combine(uploadsPath, uniqueFileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Create attachment record
        var attachment = new PartAttachment
        {
            PartId = partId,
            FileName = file.FileName,
            FilePath = filePath,
            FileUrl = $"/api/attachments/download/{uniqueFileName}",
            Type = type,
            FileSize = file.Length,
            ContentType = file.ContentType,
            Description = description,
            UploadedAt = DateTime.UtcNow
        };

        _context.PartAttachments.Add(attachment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPartAttachments), new { partId }, attachment);
    }

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadAttachment(string fileName)
    {
        var uploadsPath = Path.Combine(_environment.ContentRootPath, "uploads", "attachments");
        var filePath = Path.Combine(uploadsPath, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var memory = new MemoryStream();
        using (var stream = new FileStream(filePath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;

        var attachment = await _context.PartAttachments
            .FirstOrDefaultAsync(a => a.FilePath == filePath);

        var contentType = attachment?.ContentType ?? "application/octet-stream";
        var downloadFileName = attachment?.FileName ?? fileName;

        return File(memory, contentType, downloadFileName);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttachment(int id)
    {
        var attachment = await _context.PartAttachments.FindAsync(id);
        if (attachment == null)
        {
            return NotFound();
        }

        // Delete physical file
        if (!string.IsNullOrEmpty(attachment.FilePath) && System.IO.File.Exists(attachment.FilePath))
        {
            System.IO.File.Delete(attachment.FilePath);
        }

        _context.PartAttachments.Remove(attachment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
