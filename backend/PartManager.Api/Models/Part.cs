namespace PartManager.Api.Models;

public class Part
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PartNumber { get; set; }
    public string? Manufacturer { get; set; }
    public string? ManufacturerPartNumber { get; set; }
    
    // Electronics-specific metadata
    public string? Package { get; set; }
    public string? Footprint { get; set; }
    public string? Value { get; set; }
    public string? Tolerance { get; set; }
    public string? Voltage { get; set; }
    public string? Current { get; set; }
    public string? Power { get; set; }
    public string? Temperature { get; set; }
    public string? Notes { get; set; }
    
    public int Quantity { get; set; }
    public int? MinQuantity { get; set; }
    
    // Storage location
    public int? DrawerId { get; set; }
    public Drawer? Drawer { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    
    // NFC/QR code support
    public string? NfcTagId { get; set; }
    public string? QrCode { get; set; }
    
    // File attachments
    public ICollection<PartAttachment> Attachments { get; set; } = new List<PartAttachment>();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
