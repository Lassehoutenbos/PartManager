namespace PartManager.Api.Models;

public class PartAttachment
{
    public int Id { get; set; }
    public int PartId { get; set; }
    public Part Part { get; set; } = null!;
    
    public string FileName { get; set; } = string.Empty;
    public string? FilePath { get; set; }
    public string? FileUrl { get; set; }
    public AttachmentType Type { get; set; }
    public long FileSize { get; set; }
    public string? ContentType { get; set; }
    public string? Description { get; set; }
    
    public DateTime UploadedAt { get; set; }
}

public enum AttachmentType
{
    Datasheet,
    Pinout,
    Photo,
    Schematic,
    Other
}
